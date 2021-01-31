using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OpenQA.Selenium.Chrome;
using SupermarketScraper.Demo.Extensions;
using SupermarketScraper.Demo.Services;
using System;
using System.IO;

namespace SupermarketScraper.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            CommandLineApplication app = new CommandLineApplication()
            {
                Name = "Supermarket Scraper Demo"
            };

            IConfiguration config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();            

            app.OnExecute(() =>
            {                
                return 0;
            });

            app.Command("scrape", command =>
            {
                command.Option("-t|--tesco", "Tesco", CommandOptionType.NoValue);

                command.OnExecute(async () =>
                {
                    try
                    {
                        ISupermarketScraper appController = config.ConfigureApplication(command.Options);

                        await appController.RunDemoAsync();
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine("Missing args. Use a valid supermarket flag (\"e.g. -t for Tesco\")");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"There was an error during Execution: {ex.Message}.\nStack Trace: {ex.StackTrace}");
                    }

                    return 0;
                });
            });

            try
            {
                app.Execute(args);
            }
            catch (CommandParsingException)
            {
                Console.WriteLine("No matching Command found");
            }          
        }        
    }
}
