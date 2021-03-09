namespace Ezy.Module.Selenium.Interface
{
    using System;

    public interface ISeleniumOption_Base
    {
        string LoginUrl { get; set; }

        string ScreenShotPath { get; set; }

        string ConfigPath { get; set; }

        string LogPath { get; set; }

        string Username { get; set; }

        string Password { get; set; }

        bool IsTakeScreenShot { get; set; }

        bool IsLocal { get; set; }

        bool IsLive { get; set; }

        bool IsText { get; set; }

        bool IsTextArea { get; set; }

        bool IsTextAreaFull { get; set; }

        bool IsOnlyNumber { get; set; }

        bool IsOnlyNormalChar { get; set; }

        bool IsOnlySpecialChar { get; set; }

        bool IsAllChar { get; set; }

        bool IsOpenAllExpander { get; set; }

        bool IsOpenAllTab { get; set; }

        bool IsAdd { get; set; }

        bool IsDelete { get; set; }

        bool IsEdit { get; set; }

        int AmountOfExpander { get; set; }

        bool IsOpenTopToBot { get; set; }

        bool IsOpenBotToTop { get; set; }

        bool IsOpenTopAndBot { get; set; }

        bool IsOpenRandom { get; set; }

        string FileName { get; set; }

        string FolderName { get; set; }
    }
}

