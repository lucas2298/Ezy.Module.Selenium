using Ezy.Module.MSSQLRepository.Connection;
using Ezy.Module.Selenium.Core;
using Ezy.Module.Selenium.Interface;
using Ezy.Module.Selenium.Share;
using Ezy.Module.Selenium.Share.Models;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading;

namespace Ezy.Module.Selenium.OpenInMenu
{
    public class OpenWebInMenu : ISelenium_Base
    {
        public ISeleniumOption_Base GetDefaultSetting()
        {
            string path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OpenInMenu");
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string str2 = Path.Combine(path, "ScreenShot");
            if (!File.Exists(str2))
            {
                Directory.CreateDirectory(str2);
            }
            string str3 = Path.Combine(path, "Config");
            if (!File.Exists(str3))
            {
                Directory.CreateDirectory(str3);
            }
            string str4 = Path.Combine(path, "Log");
            if (!File.Exists(str4))
            {
                Directory.CreateDirectory(str4);
            }
            SeleniumOpenInMenuOptionModel model1 = new SeleniumOpenInMenuOptionModel();
            model1.ScreenShotPath = str2;
            model1.ConfigPath = str3;
            model1.LogPath = str4;
            SeleniumOpenInMenuOptionModel model = model1;
            model.FolderName = "OpenInMenu";
            return model;
        }
        public string stringMetadata = "SeleniumModel.csdl|res://*/SeleniumModel.ssdl|res://*/SeleniumModel.msl";
        public string StartClick(ChromeDriver chrome, object dataObject)
        {
            string str = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "OpenInMenu");
            int lastErrorCount = 0;
            SeleniumOpenInMenuOptionModel model = dataObject as SeleniumOpenInMenuOptionModel;
            ReadOnlyCollection<IWebElement> onlys = chrome.FindElementsByTagName("a");
            Dictionary<string, bool> dictionary = new Dictionary<string, bool>();

            foreach (IWebElement element in onlys)
            {
                if (element.Text != "")
                {
                    dictionary[element.Text] = true;
                }
            }
            string path = Path.Combine(str, "ScreenShot");
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            string str3 = Path.Combine(str, "Log");
            if (!File.Exists(str3))
            {
                Directory.CreateDirectory(str3);
            }
            try
            {
                chrome.FindElementByCssSelector("span[class='navbar-toggler-icon']").Click();
                int linkOpenCount = 0;
                var connectionString = DataConnectionManager.GetSimpleConnectionString("Setting.txt");
                var sConnect = DataConnectionManager.GetDataConnectionString_With_ConnectionString(connectionString, stringMetadata);

                using (var repo = new AlliancePos_DevEntities(sConnect))
                {
                    var parent = new AutomationTestData()
                    {
                        StartTime = DateTime.Now,
                        LinkOpenCount = 0
                    };
                    repo.AutomationTestDatas.Add(parent);
                    repo.SaveChanges();

                    foreach (IWebElement element3 in onlys)
                    {
                        if (element3.Text != "")
                        {
                            element3.Click();
                            Thread.Sleep(1000);
                            foreach (IWebElement element4 in chrome.FindElementsByTagName("a"))
                            {
                                if ((element4.Text != "") && !dictionary.ContainsKey(element4.Text))
                                {
                                    linkOpenCount++;

                                    element4.Click();
                                    Thread.Sleep(5000);
                                    if (chrome.WindowHandles.Count > 1)
                                    {
                                        chrome.SwitchTo().Window(chrome.WindowHandles[1]);
                                    }
                                    chrome.Url = "https://pos.allianceitsc.com/#/config-company-bank-account-activity?tab=ui-ctrl-154";
                                    chrome.Navigate();
                                    Thread.Sleep(5000);
                                    var verify = SeleniumHelper.VerifyError(chrome, path, lastErrorCount);
                                    var sMessage = verify.Error;
                                    string openInTabResult = string.Empty;
                                    if (model.IsOpenAllTab)
                                    {
                                        if (model.IsOpenAllExpander)
                                        {
                                            openInTabResult = SeleniumHelper.OpenAllTabInPage(chrome, 1, path, model.AmountOfExpander, parent.Id, repo);
                                        }
                                        else openInTabResult = SeleniumHelper.OpenAllTabInPage(chrome, 0, path, model.AmountOfExpander, parent.Id, repo);
                                    }
                                    if (openInTabResult == "NoTab" && model.IsOpenAllExpander)
                                    {
                                        var type = SeleniumHelper.OpenAllExpander(chrome, path, model.AmountOfExpander, parent.Id, repo);
                                        if (type == "NoExpander")
                                        {
                                            var item = new AutomationTestData()
                                            {
                                                StartTime = DateTime.Now,
                                                ParentId = parent.Id,
                                                ErrorLink = chrome.Url,
                                                Error = sMessage,
                                                LocalImagePath = verify.FilePath,
                                                IsLinkError = true
                                            };
                                            repo.AutomationTestDatas.Add(item);
                                            repo.SaveChanges();
                                        }
                                    }
                                    if (chrome.WindowHandles.Count > 1)
                                    {
                                        chrome.Close();
                                        chrome.SwitchTo().Window(chrome.WindowHandles[0]);
                                    }
                                    Thread.Sleep(1000);
                                    if (!string.IsNullOrEmpty(sMessage))
                                    {
                                        lastErrorCount++;
                                    }
                                }
                            }
                            element3.Click();
                            Thread.Sleep(5000);
                        }
                    }
                    parent.LinkOpenCount = linkOpenCount;
                    repo.SaveChanges();
                }
                return string.Empty;
            }
            catch (Exception exception1)
            {
                return exception1.Message;
            }

        }
    }
}

