namespace Ezy.Module.Selenium.Share
{
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using System;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading;

    public static class SeleniumHelper
    {
        public static string BackTracking(ChromeDriver chrome, int n, int MaxN, int optionOpenExpander, string ScreenShotPath, int amountOfExpander)
        {
            string sMessage = "";
            try
            {
                ReadOnlyCollection<IWebElement> onlys = chrome.FindElementsByClassName("tablv-" + n.ToString());
                if ((onlys == null) || (onlys.Count <= 0))
                {
                    sMessage = "NoTab";
                }
                else
                {
                    int lastErrorCount = 0;
                    foreach (IWebElement element in onlys)
                    {
                        if ((((element.Text != "") && element.Displayed) && (element.Location.X > 0)) && (element.Location.Y > 0))
                        {
                            element.Click();
                            Thread.Sleep(5000);
                            sMessage = VerifyError(chrome, ScreenShotPath, lastErrorCount);
                            if (string.IsNullOrEmpty(sMessage))
                            {
                                if (optionOpenExpander == 1)
                                {
                                    OpenAllExpander(chrome, ScreenShotPath, amountOfExpander);
                                }
                            }
                            else lastErrorCount++;
                            if (n < MaxN)
                            {
                                BackTracking(chrome, n + 1, MaxN, optionOpenExpander, ScreenShotPath, amountOfExpander);
                            }
                        }
                    }
                }
                return sMessage;
            }
            catch
            {
                return sMessage;
            }
        }

        public static string ChangeServerApi(ChromeDriver chrome, string UserName, string Password, string Server, string ScreenShotPath)
        {
            string str = "";
            if (Server != "Local")
            {
                str = "Server run online\n";
            }
            else
            {
                chrome.FindElementByXPath("//*[@id='root']/div/div[3]/header/button[2]").Click();
                Thread.Sleep(0x3e8);
                chrome.FindElementByXPath("//*[@id='root']/div/div[3]/div[1]/aside/div/div/div[1]/div[2]/input[1]").Click();
                Thread.Sleep(0xbb8);
                chrome.Navigate().GoToUrl("http://soliddevapp.allianceitsc.com/#/login/up");
                chrome.Navigate().Refresh();
                Thread.Sleep(0x1388);
                Login(chrome, UserName, Password, ScreenShotPath);
                str = "Server run on localhost\n";
            }
            return str;
        }

        public static string Login(ChromeDriver chrome, string Name, string Pass, string ScreenShotPath)
        {
            IWebElement element2 = chrome.FindElementById("ip_password");
            IWebElement element3 = chrome.FindElementByCssSelector("[class='btn btn-primary']");
            chrome.FindElementById("ip_username").SendKeys(Name);
            element2.SendKeys(Pass);
            element3.Click();
            Thread.Sleep(5000);
            return VerifyError(chrome, ScreenShotPath, 0);
        }

        public static string OpenAllExpander(ChromeDriver chrome, string ScreenShotPath, int amountOfExpander)
        {
            try
            {
                ReadOnlyCollection<IWebElement> expanderList = chrome.FindElementsByCssSelector("i[class='fa fa-chevron-circle-down']");
                return OpenExpander_Base(chrome, expanderList, ScreenShotPath, amountOfExpander);
            }
            catch (Exception exception1)
            {
                return exception1.Message;
            }
        }

        public static string OpenAllTabInPage(ChromeDriver chrome, int optionOpenExpander, string ScreenShotPath, int amountOfExpander)
        {
            int maxN = 3;
            return BackTracking(chrome, 1, maxN, optionOpenExpander, ScreenShotPath, amountOfExpander);
        }

        public static string OpenExpander_Base(ChromeDriver chrome, ReadOnlyCollection<IWebElement> ExpanderList, string ScreenShotPath, int amountOfExpander)
        {
            string sMessage = string.Empty;
            if ((ExpanderList != null) && (ExpanderList.Count > 0))
            {
                long num = chrome.Manage().Window.Size.Height - 500;
                long num2 = num;
                int lastErrorCount = 0;
                var amount = 0;
                if (amountOfExpander == 0) amountOfExpander = int.MaxValue;
                foreach (IWebElement element in ExpanderList)
                {
                    if (amount < amountOfExpander)
                    {
                        amount++;
                        IJavaScriptExecutor executor = chrome;
                        if (element.Location.Y >= num)
                        {
                            Point location = element.Location;
                            executor.ExecuteScript($"window.scrollBy(0, {location.Y - 150});", Array.Empty<object>());
                            num += num2;
                        }
                        try
                        {
                            Thread.Sleep(1000);
                            element.Click();
                            Thread.Sleep(5000);
                            sMessage = VerifyError(chrome, ScreenShotPath, lastErrorCount);
                            if (!string.IsNullOrEmpty(sMessage))
                            {
                                lastErrorCount++;
                            }
                            element.Click();
                            Thread.Sleep(1000);
                        }
                        catch
                        {
                        }
                    }
                    else break;
                }
            }
            return sMessage;
        }

        public static void TakeScreenShot(ChromeDriver driver, string ScreenShotName, string ErrorName, string ScreenShotPath)
        {
            if (!string.IsNullOrEmpty(ScreenShotPath))
            {
                string path = Path.Combine(ScreenShotPath, ErrorName, ScreenShotName);
                Directory.CreateDirectory(path);
                driver.GetScreenshot().SaveAsFile(Path.Combine(path, ScreenShotName + ".jpg"));
                Thread.Sleep(1000);
            }
        }
        public static string VerifyError(ChromeDriver chrome, string ScreenShotPath, int lastErrorCount)
        {
            string str = "";
            try
            {
                var attributes = chrome.FindElementsById("error_api");
                if (attributes.Count() > lastErrorCount)
                {
                    string attribute = attributes.LastOrDefault().GetAttribute("innerHTML");
                    if (attribute != "")
                    {
                        string screenShotName = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string[] textArray1 = new string[] { "Error_API: ", attribute, "\nScreenShot saved as: ", screenShotName, "\nLink: ", chrome.Url, "\n" };
                        str = string.Concat(textArray1);
                        TakeScreenShot(chrome, screenShotName, "Error", ScreenShotPath);
                    }
                }
                
            }
            catch
            {
            }
            try
            {
                var attributes = chrome.FindElementsById("error_ui");
                if (attributes.Count > lastErrorCount)
                {
                    string attribute = attributes.LastOrDefault().GetAttribute("innerHTML");
                    if (attribute != "")
                    {
                        string screenShotName = DateTime.Now.ToString("yyyyMMddHHmmss");
                        string[] textArray2 = new string[] { "Error_UI: ", attribute, "\nScreenShot saved as: ", screenShotName, "\nLink: ", chrome.Url, "\n" };
                        str = string.Concat(textArray2);
                        TakeScreenShot(chrome, screenShotName, "Error", ScreenShotPath);
                    }
                }
            }
            catch
            {
            }
            try
            {
                Thread.Sleep(0x3e8);
                chrome.FindElementById("btn-ls-close").Click();
                Thread.Sleep(0x5dc);
            }
            catch
            {
            }
            return str;
        }
    }
}

