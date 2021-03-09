namespace Ezy.Module.Selenium.Interface
{
    using System;
    using System.Windows;

    public interface ISeleniumNeed
    {
        void Button_UploadConfigFile_Click(object sender, RoutedEventArgs e);

        ISelenium_Base SeleniumInstance { get; set; }

        string ErrorMessage { get; set; }
    }
}

