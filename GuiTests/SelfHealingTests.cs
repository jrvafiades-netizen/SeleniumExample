using System;
using OpenQA.Selenium;
using Structura.GuiTests.PageObjects;
using Structura.GuiTests.SeleniumHelpers;
using Structura.GuiTests.Utilities;

namespace Structura.GuiTests
{
    public class SelfHealingTests : IDisposable
    {
        private IWebDriver _driver;
        private string _baseUrl;

        public SelfHealingTests()
        {
            _driver = new DriverFactory().Create();
            _baseUrl = ConfigurationHelper.Get<string>("TargetUrl");
            Console.WriteLine("Setup: Driver created");
        }

        public void Dispose()
        {
            try
            {
                // Log healing report if self-healing is enabled
                if (HealeniumUtils.IsHealingDriver(_driver))
                {
                    HealeniumUtils.LogHealingReport(_driver);
                }

                _driver.Quit();
                _driver.Close();
            }
            catch (Exception)
            {
                // Ignore errors if we are unable to close the browser
            }
        }

        public void DemonstrateSelfHealingCapabilities()
        {
            Console.WriteLine("TEST: DemonstrateSelfHealingCapabilities");
            // This test demonstrates the self-healing capabilities.
            // When using the SelfHealingDriver, if a locator fails to find an element,
            // it will attempt alternative strategies before failing.

            // Arrange
            var homepage = new NewBalanceHomepage(_driver);

            // Act
            homepage.Navigate(_baseUrl);

            // Assert
            if (!homepage.IsLoaded())
                throw new InvalidOperationException("Homepage should load successfully");
            
            // If using self-healing driver, the healing report will be logged
        }

        public void SelfHealingDriverCanRecoverFromBrokenLocators()
        {
            Console.WriteLine("TEST: SelfHealingDriverCanRecoverFromBrokenLocators");
            // This test shows how the self-healing driver can recover
            // from locator changes by trying alternative strategies

            // Arrange
            var homepage = new NewBalanceHomepage(_driver);
            var isHealingEnabled = HealeniumUtils.IsHealingDriver(_driver);

            // Act
            homepage.Navigate(_baseUrl);
            var searchBar = homepage.GetSearchBar();

            // Assert
            if (searchBar == null)
                throw new InvalidOperationException("Search bar should not be null");
            
            if (!searchBar.Displayed)
                throw new InvalidOperationException("Search bar should be visible");

            // If healing was enabled, show what was healed
            if (isHealingEnabled)
            {
                var report = HealeniumUtils.GetHealingReport(_driver);
                Console.WriteLine($"Healing report: {report.TotalHealedLocators} locators healed");
            }
        }
    }
}
