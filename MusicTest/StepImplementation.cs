using System;
using System.IO;
using Gauge.CSharp.Lib.Attribute;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace MusicTest
{
    public class StepImplementation
    {
        private WindowsDriver<WindowsElement> _driver;
        private const string WindowsApplicationDriverUrl = "http://127.0.0.1:4723";

        [Step("Mở App Music Player")]
        public void OpenMusicPlayerApp()
        {
            // --- SỬA LỖI: CHỈ ĐỊNH ĐƯỜNG DẪN TUYỆT ĐỐI ---
            // Vì bạn đang dùng ổ D, ta điền thẳng đường dẫn vào đây để tránh bị Gauge lừa sang ổ C.
            string appPath = @"D:\MusicProject_Final\MusicApp\bin\Debug\net9.0-windows\MusicApp.exe";

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("Đang mở App tại: " + appPath);
            Console.WriteLine("--------------------------------------------------");

            if (!File.Exists(appPath))
            {
                 throw new FileNotFoundException($"KHÔNG TÌM THẤY APP!\nFile không tồn tại ở: {appPath}\n--> Hãy kiểm tra lại xem bạn đã 'dotnet build' MusicApp chưa?");
            }

            // --- CẤU HÌNH KẾT NỐI ---
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability("app", appPath);
            appiumOptions.AddAdditionalCapability("deviceName", "WindowsPC");

            try
            {
                _driver = new WindowsDriver<WindowsElement>(new Uri(WindowsApplicationDriverUrl), appiumOptions);
                System.Threading.Thread.Sleep(2000); 
                Console.WriteLine("--> MỞ APP THÀNH CÔNG!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("--> LỖI KẾT NỐI WINAPPDRIVER: " + ex.Message);
                throw;
            }
        }

        [AfterScenario]
        public void Cleanup()
        {
            if (_driver != null)
            {
                _driver.Quit();
            }
        }
    }
}