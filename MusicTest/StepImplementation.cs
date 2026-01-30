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
            // --- CÁCH MỚI: TỰ ĐỘNG TÌM ĐƯỜNG DẪN ---
            
            // 1. Lấy thư mục nơi bạn đang đứng chạy lệnh (thường là folder MusicTest)
            string baseDir = Directory.GetCurrentDirectory();

            // 2. Dùng ".." để lùi ra ngoài folder MusicTest, rồi đi vào MusicApp
            // Cấu trúc: MusicTest/../MusicApp/bin/Debug/net9.0-windows/MusicApp.exe
            string relativePath = Path.Combine(baseDir, "..", "MusicApp", "bin", "Debug", "net9.0-windows", "MusicApp.exe");

            // 3. Chuyển nó thành đường dẫn tuyệt đối sạch sẽ
            string appPath = Path.GetFullPath(relativePath);

            Console.WriteLine("--------------------------------------------------");
            Console.WriteLine("Đang tìm App tại: " + appPath); 
            Console.WriteLine("--------------------------------------------------");

            if (!File.Exists(appPath))
            {
                 // Gợi ý lỗi chi tiết hơn để bạn dễ sửa
                 throw new FileNotFoundException(
                    $"KHÔNG TÌM THẤY APP!\n" +
                    $"Đường dẫn máy đang tìm là: {appPath}\n" +
                    $"--> Hãy chắc chắn bạn đã Build dự án MusicApp (chạy 'dotnet build' trong thư mục MusicApp).");
            }

            // --- CẤU HÌNH KẾT NỐI (Giữ nguyên) ---
            var appiumOptions = new AppiumOptions();
            appiumOptions.AddAdditionalCapability("app", appPath);
            appiumOptions.AddAdditionalCapability("deviceName", "WindowsPC");
            // ... (Phần còn lại giữ nguyên) ...
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