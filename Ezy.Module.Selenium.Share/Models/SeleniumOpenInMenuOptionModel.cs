namespace Ezy.Module.Selenium.Share.Models
{
    using Ezy.Module.Selenium.Interface;
    using System;
    using System.Runtime.CompilerServices;

    public class SeleniumOpenInMenuOptionModel : ISeleniumOption_Base
    {
        public string ScreenShotPath { get; set; }

        public string ConfigPath { get; set; }

        public string LogPath { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public bool IsTakeScreenShot { get; set; }

        public bool IsLocal { get; set; }

        public bool IsLive { get; set; }

        public bool IsText { get; set; }

        public bool IsTextArea { get; set; }

        public bool IsTextAreaFull { get; set; }

        public bool IsOnlyNumber { get; set; }

        public bool IsOnlyNormalChar { get; set; }

        public bool IsOnlySpecialChar { get; set; }

        public bool IsAllChar { get; set; }

        public bool IsOpenAllExpander { get; set; }

        public bool IsOpenAllTab { get; set; }

        public bool IsAdd { get; set; }

        public bool IsDelete { get; set; }

        public bool IsEdit { get; set; }

        public string FolderName { get; set; }

        public string LoginUrl { get; set; }

        public int AmountOfExpander { get; set; }

        public bool IsOpenBotToTop { get; set; }

        public bool IsOpenTopAndBot { get; set; }

        public bool IsOpenRandom { get; set; }

        public string FileName { get; set; }

        public bool IsOpenTopToBot { get; set; }
    }
}

