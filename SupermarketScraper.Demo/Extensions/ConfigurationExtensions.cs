using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium.Chrome;
using SupermarketScraper.Demo.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace SupermarketScraper.Demo.Extensions
{
    public static class ConfigurationExtensions
    {
        public static ISupermarketScraper ConfigureApplication(this IConfiguration config, List<CommandOption> commandOptions)
        {
            ServiceCollection serviceColletion = new ServiceCollection();
            serviceColletion.AddSingleton(config);

            if (commandOptions.Where(x => x.HasValue()).Count() == 0)
                throw new ArgumentException("Must specicy Supermarket arg");

            if (!string.IsNullOrEmpty(commandOptions.FirstOrDefault(x => x.LongName == "tesco")?.Value()))
                serviceColletion.AddSingleton<ISupermarketScraper, TescoScraper>();


            IRandomWaitTimeService randomWaitTimeService = new RandomWaitTimeService(
                                        config.GetValue<int>("WaitTime:Short:Min"),
                                        config.GetValue<int>("WaitTime:Short:Max"),
                                        config.GetValue<int>("WaitTime:Long:Min"),
                                        config.GetValue<int>("WaitTime:Long:Max"));
            serviceColletion.AddSingleton(randomWaitTimeService);

            serviceColletion.AddSingleton(new ChromeDriver($@"{Assembly.GetExecutingAssembly().Location.Replace($@"\{nameof(SupermarketScraper)}.{nameof(Demo)}.dll", string.Empty)}"));

            IServiceProvider serviceProvider = serviceColletion.BuildServiceProvider();
            
            ISupermarketScraper applicationController = serviceProvider.GetService<ISupermarketScraper>();

            return applicationController;
        }
    }
}
