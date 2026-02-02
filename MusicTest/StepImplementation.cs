using System;
using System.IO;
using System.Threading;
using Gauge.CSharp.Lib.Attribute;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace MusicTest
{
    public class StepImplementation
    {
        private WindowsDriver<WindowsElement> _driver;
        private const string Url = "http://127.0.0.1:4723";

        // ================== OPEN APP ==================
        [Step("Mở App Music Player")]
        public void OpenApp()
        {
            if (_driver != null) return;

            string appPath = Path.GetFullPath(
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "..", "MusicApp", "bin", "Debug", "net9.0-windows", "MusicApp.exe"));

            var opt = new AppiumOptions();
            opt.AddAdditionalCapability("app", appPath);
            opt.AddAdditionalCapability("deviceName", "WindowsPC");

            _driver = new WindowsDriver<WindowsElement>(new Uri(Url), opt);
            Thread.Sleep(1500);
        }

        // ================== MENU ==================
        [Step("Kiểm tra nút Menu tồn tại")]
        public void MenuExists()
        {
            _driver.FindElementByAccessibilityId("btnMenu");
        }

        [Step("Bấm nút Menu")]
        public void ClickMenu()
        {
            var menu = _driver.FindElementByAccessibilityId("btnMenu");
            menu.Click();

            // Đợi menu con hiện ra
            WaitForElement("btnOpenFile", "Open File");
        }

        [Step("Kiểm tra nút Open File tồn tại")]
        public void OpenFileExists()
        {
            EnsureMenuOpened();
            _driver.FindElementByAccessibilityId("btnOpenFile");
        }

        [Step("Kiểm tra nút List tồn tại")]
        public void ListExists()
        {
            EnsureMenuOpened();
            _driver.FindElementByAccessibilityId("btnList");
        }

        // ================== PLAYLIST ==================
        [Step("Kiểm tra danh sách bài hát tồn tại")]
        public void PlaylistExists()
        {
            _driver.FindElementByAccessibilityId("lvPlaylist");
        }

        [Step("Kiểm tra cột STT tồn tại")]
        public void ColSTT() => _driver.FindElementByAccessibilityId("colSTT");

        [Step("Kiểm tra cột Title tồn tại")]
        public void ColTitle() => _driver.FindElementByAccessibilityId("colTitle");

        [Step("Kiểm tra cột Singer tồn tại")]
        public void ColSinger() => _driver.FindElementByAccessibilityId("colSinger");

        [Step("Kiểm tra cột Time tồn tại")]
        public void ColTime() => _driver.FindElementByAccessibilityId("colTime");

        // ================== CONTROLS ==================
        [Step("Kiểm tra thanh thời gian tồn tại")]
        public void TimelineExists()
        {
            _driver.FindElementByAccessibilityId("sldProgress");
        }

        [Step("Kiểm tra nút Prev tồn tại")]
        public void PrevExists() => _driver.FindElementByAccessibilityId("btnPrev");

        [Step("Kiểm tra nút Pause tồn tại")]
        public void PauseExists() => _driver.FindElementByAccessibilityId("btnPause");

        [Step("Kiểm tra nút Next tồn tại")]
        public void NextExists() => _driver.FindElementByAccessibilityId("btnNext");

        // ================== HELPERS ==================
        private void EnsureMenuOpened()
        {
            try
            {
                _driver.FindElementByAccessibilityId("btnOpenFile");
            }
            catch
            {
                _driver.FindElementByAccessibilityId("btnMenu").Click();
                WaitForElement("btnOpenFile", "Open File");
            }
        }

        private void WaitForElement(string id, string name)
        {
            for (int i = 0; i < 10; i++)
            {
                try
                {
                    _driver.FindElementByAccessibilityId(id);
                    return;
                }
                catch
                {
                    Thread.Sleep(300);
                }
            }

            throw new Exception($"❌ Không tìm thấy {name}");
        }

        [AfterScenario]
        public void Cleanup()
        {
            _driver?.Quit();
            _driver = null;
        }
    }
}
