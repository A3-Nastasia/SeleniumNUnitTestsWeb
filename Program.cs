using NUnit.Framework;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;

namespace SeleniumTasks{
    public class BrowserSearchTests
    {
        IWebDriver driver;
        ChromeDriver options;
        WebDriverWait wait;
        
        [SetUp]
        public void Setup()
        {
            options = new ChromeOptions();
            // If test time takes more than 1 minute you can use this.
            // (changed time from ~90s to ~30s)
            // By default settings Selenium is waiting the page to load "fully".
            // Here you change PageLoadStrategy to work when main elements are loaded but extensions are still loading.
            options.PageLoadStrategy = PageLoadStrategy.Eager;
            driver = new ChromeDriver();
            wait = new WebDriverWait(driver, TimeSpan.FromSeconds(15));
            driver.Manage().Timeouts().PageLoad = TimeSpan.FromSeconds(20);
        }

        [Test]
        public async Task InternetConnectionAvailable()
        {
            // Using to escape manual Close()
            using var client = new HttpClient();

            try
            {
                var response = await client.GetAsync("https://www.google.com/");
                Assert.That(response.IsSuccessStatusCode, Is.True, "Internet is connected.");
            }
            catch
            {
                Assert.Fail("No Internet connection.");
            }
        }

        [Test]
        public void TitleOfBrowserSearch()
        {
            driver.Navigate().GoToUrl("https://www.google.com/");
            Assert.That(driver.Title, Is.EqualTo("Google"));
        }

        [Test]
        public void ProgrammerVacancies()
        {
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
            
            Thread.Sleep(1000);
            
            // Wait 'til it'll find the element
            IWebElement webElement = WaitForVisibleElement(By.Id("a11y-search-input"));
            string text = webElement.GetAttribute("value") ?? string.Empty;

            // Filter button
            WaitForVisibleElement(By.ClassName("magritte-button_with-icon__znqnP_5-3-3")).Click();

            // Work experience
            webElement = driver.FindElement(By.CssSelector("[data-qa='advanced-search__experience-item_noExperience']"));
            ScrollToElement(webElement);
            webElement.Click();

            // Work format
            webElement = driver.FindElement(By.CssSelector("[data-qa='advanced-search__work_format-item_REMOTE']"));
            ScrollToElement(webElement);
            webElement.Click();

            // Submit button
            webElement = driver.FindElement(By.CssSelector("[data-qa='advanced-search-submit-button']"));
            ScrollToElement(webElement);
            webElement.Click();
            


            webElement = FindHiddenInput(webElement, By.CssSelector("input[value='noExperience']"), By.CssSelector("input.magritte-radio-input-checked___eYMcP_3-0-49[value='noExperience']"));
            string workExperience = webElement.GetAttribute("value");
            bool isExperienceChecked = webElement.GetAttribute("checked") != null;


            webElement = FindHiddenInput(webElement, By.CssSelector("input[value='REMOTE']"), By.CssSelector("input.magritte-checkbox-input___Y41Ta_3-0-49[value='REMOTE']"));
            string workFormat = webElement.GetAttribute("value");
            bool isFormatChecked = webElement.GetAttribute("checked") != null;

            using (Assert.EnterMultipleScope())
            {
                Assert.That(text, Is.EqualTo(searchSentence));

                Assert.That(workExperience, Is.EqualTo("noExperience"));
                Assert.That(workFormat, Is.EqualTo("REMOTE"));

                Assert.That(isExperienceChecked, Is.True);
                Assert.That(isFormatChecked, Is.True);
            }
        }

        [TearDown]
        public void Teardown()
        {
            driver.Quit();
        }

        /// <summary>
        /// Scroll page to see the element
        /// </summary>
        /// <param name="webElement">Element of the page</param>
        public void ScrollToElement(IWebElement webElement) => ((IJavaScriptExecutor)driver).ExecuteScript("arguments[0].scrollIntoView(true)", webElement);

        /// <summary>
        /// Get element if it's visible
        /// </summary>
        /// <param name="by">Locator of element</param>
        /// <returns>Founded and visible element or null</returns>
        public IWebElement WaitForVisibleElement(By by)
        {
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

        /// <summary>
        /// Scroll page until HTML element will not become visible
        /// </summary>
        /// <param name="webElement">Element of the page</param>
        /// <param name="amountAttempts">Amount attempts to find the element on the page</param>
        public void ScrollUntilElementVisible(IWebElement webElement, int amountAttempts = 10)
        {
            while(amountAttempts > 0)
            {
                try
                {
                    if(!webElement.Displayed)
                    {
                        ((IJavaScriptExecutor)driver).ExecuteScript("window.scrollBy(0, window.innerHeight)");
                        break;
                    }
                }
                catch(NoSuchElementException)
                {
                    
                }
                Thread.Sleep(500);
                amountAttempts--;
            }
        }

        /// <summary>
        /// Find hidden input (Selenium can't scroll to find input with "hidden")
        /// </summary>
        /// <param name="webElement">Element of the page</param>
        /// <param name="byGeneralInputLocator">Locator that is general for the element (will be with "hidden")</param>
        /// <param name="bySpecifiedInputLocator">Locator that is the element (without "hidden")</param>
        /// <returns>Returns IWedElement (Element of the page)</returns>
        public IWebElement FindHiddenInput(IWebElement webElement, By byGeneralInputLocator, By bySpecifiedInputLocator)
        {
            webElement = driver.FindElement(byGeneralInputLocator);
            ScrollUntilElementVisible(webElement);

            webElement = driver.FindElement(bySpecifiedInputLocator);
            return webElement;
        }

    }
}