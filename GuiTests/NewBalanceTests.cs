using System;
using System.Text;
using System.Threading;
using OpenQA.Selenium;
using Structura.GuiTests.PageObjects;
using Structura.GuiTests.SeleniumHelpers;
using Structura.GuiTests.Utilities;

namespace Structura.GuiTests
{
    public class NewBalanceTests : IDisposable
    {
        private IWebDriver _driver;
        private string _baseUrl;

        public NewBalanceTests()
        {
            var enableHealing = ConfigurationHelper.Get<bool>("EnableHealenium");
            _driver = enableHealing 
                ? new DriverFactory().CreateWithHealing() 
                : new DriverFactory().Create();
            _baseUrl = ConfigurationHelper.Get<string>("TargetUrl");
            Console.WriteLine("Setup: Driver created");
        }

        public void Dispose()
        {
            try
            {
                _driver.Quit();
                _driver.Close();
            }
            catch (Exception)
            {
                // Ignore errors if we are unable to close the browser
            }
        }

        public void NewBalanceHomepageShouldLoad()
        {
            Console.WriteLine("TEST: NewBalanceHomepageShouldLoad");
            // Arrange
            var homepage = new NewBalanceHomepage(_driver);

            // Act
            homepage.Navigate(_baseUrl);

            // Assert
            if (!homepage.IsLoaded())
                throw new InvalidOperationException("New Balance homepage should load successfully");
        }

        public void ShouldFindSearchBarOnHomepage()
        {
            Console.WriteLine("TEST: ShouldFindSearchBarOnHomepage");
            // Arrange
            var homepage = new NewBalanceHomepage(_driver);
            homepage.Navigate(_baseUrl);

            // Act
            var searchElement = homepage.GetSearchBar();

            // Assert
            if (!searchElement.Displayed)
                throw new InvalidOperationException("Search bar should be visible on the homepage");
        }

        public void ShouldBeAbleToNavigateToMensShoes()
        {
            Console.WriteLine("TEST: ShouldBeAbleToNavigateToMensShoes");
            // Arrange
            var homepage = new NewBalanceHomepage(_driver);
            homepage.Navigate(_baseUrl);

            // Act
            var mensPage = homepage.NavigateToMens();

            // Assert
            if (!mensPage.IsLoaded())
                throw new InvalidOperationException("Mens page should load successfully");
        }

        public void ShouldFindProductsOnSite()
        {
            Console.WriteLine("TEST: ShouldFindProductsOnSite");
            // Arrange
            var homepage = new NewBalanceHomepage(_driver);
            homepage.Navigate(_baseUrl);

            // Act
            var productsVisible = homepage.AreProductsVisible();

            // Assert
            if (!productsVisible)
                throw new InvalidOperationException("Products should be visible on the New Balance site");
        }
    }
}


