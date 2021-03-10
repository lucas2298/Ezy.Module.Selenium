namespace Ezy.Module.Selenium.Share
{
    using Ezy.Module.Selenium.Core;
    using Ezy.Module.Selenium.Share.Models;
    using OpenQA.Selenium;
    using OpenQA.Selenium.Chrome;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading;

    public static class SeleniumHelper
    {
        public static string BackTracking(ChromeDriver chrome, int n, int MaxN, int optionOpenExpander, string ScreenShotPath, int amountOfExpander, long parentId, AlliancePos_DevEntities repo)
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
                            var verify = VerifyError(chrome, ScreenShotPath, lastErrorCount);
                            sMessage = verify.Error;
                            if (string.IsNullOrEmpty(sMessage))
                            {
                                if (optionOpenExpander == 1)
                                {
                                    var error = OpenAllExpander(chrome, ScreenShotPath, amountOfExpander, parentId, repo);
                                    if (!string.IsNullOrEmpty(error))
                                    {
                                        lastErrorCount++;
                                    }
                                }
                            }
                            else
                            {
                                var item = new AutomationTestData()
                                {
                                    StartTime = DateTime.Now,
                                    ParentId = parentId,
                                    Error = sMessage,
                                    ErrorLink = chrome.Url,
                                    LocalImagePath = verify.FilePath,
                                    IsLinkError = true
                                };
                                repo.AutomationTestDatas.Add(item);
                                repo.SaveChanges();
                                lastErrorCount++;
                            }
                            if (n < MaxN)
                            {
                                BackTracking(chrome, n + 1, MaxN, optionOpenExpander, ScreenShotPath, amountOfExpander, parentId, repo);
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
            var result = VerifyError(chrome, ScreenShotPath, 0);
            return result != null ? result.Error : string.Empty;
        }

        public static string OpenAllExpander(ChromeDriver chrome, string ScreenShotPath, int amountOfExpander, long parentId, AlliancePos_DevEntities repo)
        {
            try
            {
                ReadOnlyCollection<IWebElement> expanderList = chrome.FindElementsByCssSelector("i[class='fa fa-chevron-circle-down']");
                return OpenExpander_Base(chrome, expanderList, ScreenShotPath, amountOfExpander, parentId, repo);
            }
            catch (Exception exception1)
            {
                return exception1.Message;
            }
        }

        public static string OpenAllTabInPage(ChromeDriver chrome, int optionOpenExpander, string ScreenShotPath, int amountOfExpander, long parentId, AlliancePos_DevEntities repo)
        {
            int maxN = 3;
            return BackTracking(chrome, 1, maxN, optionOpenExpander, ScreenShotPath, amountOfExpander, parentId, repo);
        }

        public static string OpenExpander_Base(ChromeDriver chrome, ReadOnlyCollection<IWebElement> ExpanderList, string ScreenShotPath, int amountOfExpander, long parentId, AlliancePos_DevEntities repo)
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
                            var verify = VerifyError(chrome, ScreenShotPath, lastErrorCount);
                            sMessage = verify.Error;
                            if (!string.IsNullOrEmpty(sMessage))
                            {
                                lastErrorCount++;
                                var item = new AutomationTestData()
                                {
                                    StartTime = DateTime.Now,
                                    ParentId = parentId,
                                    ErrorLink = chrome.Url,
                                    Error = sMessage,
                                    LocalImagePath = verify.FilePath,
                                    IsExpanderError = true
                                };
                                repo.AutomationTestDatas.Add(item);
                                repo.SaveChanges();
                                break;
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
            else
            {
                sMessage = "NoExpander";
            }
            return sMessage;
        }

        public static string TakeScreenShot(ChromeDriver driver, string ScreenShotName, string ErrorName, string ScreenShotPath)
        {
            string filePath = string.Empty;
            if (!string.IsNullOrEmpty(ScreenShotPath))
            {
                string path = Path.Combine(ScreenShotPath, ErrorName, ScreenShotName);
                Directory.CreateDirectory(path);
                filePath = Path.Combine(path, ScreenShotName + ".jpg");
                driver.GetScreenshot().SaveAsFile(filePath);
                Thread.Sleep(1000);
            }
            return filePath;
        }
        public static VerifyErrorModel VerifyError(ChromeDriver chrome, string ScreenShotPath, int lastErrorCount)
        {
            var result = new VerifyErrorModel();
            Dictionary<string, string> dict = new Dictionary<string, string>();
            try
            {
                var attributes = chrome.FindElementsById("error_api");
                if (attributes.Count() > lastErrorCount)
                {
                    string attribute = attributes.LastOrDefault().GetAttribute("innerHTML");
                    if (attribute != "")
                    {
                        string screenShotName = DateTime.Now.ToString("yyyyMMddHHmmss");
                        result.Error = $"Error API: {attribute}";
                        result.FilePath = TakeScreenShot(chrome, screenShotName, "Error", ScreenShotPath);

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
                        result.Error = $"Error UI: {attribute}";
                        result.FilePath = TakeScreenShot(chrome, screenShotName, "Error", ScreenShotPath);
                    }
                }
            }
            catch
            {
            }
            try
            {
                Thread.Sleep(1000);
                chrome.FindElementById("btn-ls-close").Click();
                Thread.Sleep(1000);
            }
            catch
            {
            }
            return result;
        }
    }
}

