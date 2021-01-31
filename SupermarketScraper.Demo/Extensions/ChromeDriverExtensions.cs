using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using SupermarketScraper.Demo.Services;
using System;
using System.Threading.Tasks;

namespace SupermarketScraper.Demo.Extensions
{
    public static class ChromeDriverExtensions
    {
        public static async Task Login(this ChromeDriver chromeDriver, IRandomWaitTimeService randomWaitTimeService, string username, string password)
        {
            try
            {
                IWebElement cookieBannerElement = chromeDriver.FindElementByClassName("beans-cookies-notification__button");
                cookieBannerElement.Click();
                await Task.Delay(randomWaitTimeService.GetRandomShortWaitTime());
                Console.WriteLine($"Accepted Cookie to remove banner");
            }
            catch (Exception)
            {
                Console.WriteLine($"Cookie banner did not exist");
            }

            IWebElement usernameWebElement = chromeDriver.FindElementById("username");
            IWebElement passwordWebElement = chromeDriver.FindElementById("password");

            usernameWebElement.SendKeys(username);
            await Task.Delay(randomWaitTimeService.GetRandomShortWaitTime());
            passwordWebElement.SendKeys(password);
            await Task.Delay(randomWaitTimeService.GetRandomShortWaitTime());

            IWebElement loginForm = chromeDriver.FindElementByClassName("ui-component__panel");
            IWebElement loginFormButton = loginForm.FindElement(By.ClassName("ui-component__button"));

            loginFormButton.Click();
        }

        public static void AddItemToBasket(this ChromeDriver chromeDriver)
        {
            IWebElement productDetailsElement = chromeDriver.FindElementByClassName("product-details-tile");

            IWebElement addBtnElement = productDetailsElement.FindElement(By.ClassName("add-control"));

            addBtnElement.Click();
        }
    }
}
