using Microsoft.Extensions.Configuration;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SupermarketScraper.Demo.Extensions;
using SupermarketScraper.Demo.Services;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SupermarketScraper.Demo
{
    class TescoScraper : ISupermarketScraper
    {
        private readonly ChromeDriver _chromeDriver;
        private readonly IRandomWaitTimeService _randomWaitTimeService;

        private readonly string _username;
        private readonly string _password;

        private readonly List<string> _itemUrls = new List<string>()
        {
            "https://www.tesco.com/groceries/en-GB/products/300810631",
            "https://www.tesco.com/groceries/en-GB/products/294263388",
            "https://www.tesco.com/groceries/en-GB/products/290921181"
        };

        private readonly string baseUrl = "https://www.tesco.com/groceries/?icid=dchp_groceriesshopgroceries";

        public TescoScraper(
            ChromeDriver chromeDriver,
            IRandomWaitTimeService randomWaitTimeService,
            IConfiguration config)
        {
            _chromeDriver = chromeDriver;
            _randomWaitTimeService = randomWaitTimeService;
            _username = config.GetValue<string>("Username");
            _password = config.GetValue<string>("Password");
        }

        public async Task<int> RunDemoAsync()
        {
            Console.WriteLine("Starting Scrape...");

            Console.WriteLine("Navigating to Tesco home page...");
            _chromeDriver.Navigate().GoToUrl(baseUrl);
            await Task.Delay(_randomWaitTimeService.GetRandomShortWaitTime());

            Console.WriteLine("Scraping Items...");
            foreach (string itemUrl in _itemUrls)
            {
                Console.WriteLine($"Scraping {itemUrl}");
                _chromeDriver.Navigate().GoToUrl(itemUrl);
                await Task.Delay(_randomWaitTimeService.GetRandomShortWaitTime());

                _chromeDriver.AddItemToBasket();
                Console.WriteLine($"Added Item");
                await Task.Delay(_randomWaitTimeService.GetRandomLongWaitTime());

                if (_chromeDriver.Url.Contains("secure.tesco.com/account") &&
                _chromeDriver.Url.Contains("login"))
                {
                    Console.WriteLine($"Redirected to Login. Logging in...");
                    await _chromeDriver.Login(_randomWaitTimeService, _username, _password);
                    Console.WriteLine($"Logged in successfully. Continuing to add next Item...");
                }

                await Task.Delay(_randomWaitTimeService.GetRandomShortWaitTime());
            }

            Console.WriteLine($"All {_itemUrls.Count} Items Scraped and added to Basket. Checking out...");

            IWebElement checkoutBtnElement = _chromeDriver.FindElementByClassName("mini-trolley__checkout");
            checkoutBtnElement.Click();
            await Task.Delay(_randomWaitTimeService.GetRandomShortWaitTime());

            Console.WriteLine($"At checkout.\n------Exiting Demo and Disposing of ChromeDriver------\n");

            _chromeDriver.Quit();

            Console.WriteLine($"ChromeDriver disposed of successfully");

            return 1;
        }
    }
}
