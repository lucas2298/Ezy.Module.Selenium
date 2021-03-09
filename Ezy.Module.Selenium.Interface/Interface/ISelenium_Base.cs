namespace Ezy.Module.Selenium.Interface
{
    using OpenQA.Selenium.Chrome;
    using System;

    public interface ISelenium_Base
    {
        ISeleniumOption_Base GetDefaultSetting();
        string StartClick(ChromeDriver chrome, object dataObject);
    }
}

