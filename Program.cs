using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumTasks{
    public class BrowserSearchTests{
        IWebDriver driver;
        ChromeDriver options;
        WebDriverWait wait;
        
        [SetUp]
        public void Setup(){
            options = new ChromeOptions();
            // If test time takes more than 1 minute you can use this.
            // (changed time from ~90s to ~30s)
            // By default settings Selenium is waiting the page to load "fully".
            // Here you change PageLoadStrategy to work when main elements are loaded but extensions are still loading.
            options.PageLoadStrategy = PageLoadStrategy.Eager;
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(10));
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(30);
        }

        [Test]
        public void TitleOfBrowserSearch(){
            driver.Navigate().GoToUrl("https://www.google.com/");
            Assert.That(driver.Title, Is.EqualTo("Google"));
        }

        [Test]
        public void ProgrammerVacancies(){
            driver.Navigate().GoToUrl("https://hh.ru/");

            string searchSentence = "программист";

            // Wait 'till it'll find the element
            IWebElement webElement = WaitForVisibleElement(By.Id("a11y-search-input"));
            webElement.SendKeys(searchSentence + Keys.Enter);
            
            try
            {
                driver.FindElement(By.ClassName("bloko-modal-close-button")).Click();
            }
            catch(NoSuchElementException)
            {
                // Nothing to do here now.
            }
            
            IWebElement webElement = WaitForVisibleElement(By.Id("a11y-search-input"));

            string text = webElement.GetAttribute("value") ?? string.Empty;
            Assert.That(text, Is.EqualTo(searchSentence));
        }

        [TearDown]
        public void Teardown(){
            driver.Quit();
        }


        /// <summary>
        /// Get element if it's visible
        /// </summary>
        /// <param name="by">Locator of element</param>
        /// <returns>Founded and visible element or null</returns>
        public IWebElement WaitForVisibleElement(By by){
            return wait.Until(d => {
                try
                {
                    var el = d.FindElement(by);
                    return el.Display ? el : null;
                }
                catch(NoSuchElementException)
                {
                    return null;
                }
            });
        }

    }
}