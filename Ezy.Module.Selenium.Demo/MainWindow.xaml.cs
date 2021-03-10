using Ezy.Module.Selenium.Interface;
using Ezy.Module.Selenium.Share.Models;
using Newtonsoft.Json;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using Ezy.Module.Selenium.OpenInMenu;
using OpenQA.Selenium.Chrome;
using System.Threading;
using Ezy.Module.Selenium.Share;

namespace Ezy.Module.Selenium.Demo
{    
    public partial class MainWindow : Window
    {
        public ChromeDriver chrome;
        public OpenWebInMenu openInMenu;
        public MainWindow()
        {
            this.Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.openInMenu = new OpenWebInMenu();
            this.StackPanel_Selenium_Data.DataContext = openInMenu.GetDefaultSetting();
        }

        private void btn_Login_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = this.StackPanel_Selenium_Data.DataContext as SeleniumOpenInMenuOptionModel;
            this.chrome = new ChromeDriver();
            this.chrome.Manage().Window.Maximize();
            this.chrome.Url = TextBox_LoginUrl.Text;
            this.chrome.Navigate();
            Thread.Sleep(2000);
            string username = dataContext.Username;
            string password = dataContext.Password;
            string screenShotPath = dataContext.ScreenShotPath;
            string logPath = dataContext.LogPath;
            string server = dataContext.IsLive ? "Live" : "Local";
            var sMessage = SeleniumHelper.Login(chrome, username, password, screenShotPath);
        }

        private void btn_Run_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = this.StackPanel_Selenium_Data.DataContext as SeleniumOpenInMenuOptionModel;
            openInMenu.StartClick(chrome, dataContext);
        }
        private void btn_SaveConfig_Click(object sender, RoutedEventArgs e)
        {
            var dataContext = this.StackPanel_Selenium_Data.DataContext as SeleniumOpenInMenuOptionModel;
            var pathConfig = dataContext.ConfigPath;
            File.WriteAllText(Path.Combine(pathConfig, $"{dataContext.FileName}_Config_{DateTime.Now.ToString("ddmmyyyy hhmmss")}.txt"), JsonConvert.SerializeObject(dataContext));
        }

        private void btn_UploadConfig_Click(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog openFileDlg = new Microsoft.Win32.OpenFileDialog();
            bool? result = openFileDlg.ShowDialog();
            if (result == true)
            {
                var config = File.ReadAllText(openFileDlg.FileName);
                this.StackPanel_Selenium_Data.DataContext = JsonConvert.DeserializeObject<SeleniumOpenInMenuOptionModel>(config);
            }
        }
    }
}

