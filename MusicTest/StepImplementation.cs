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

        // ================== OPEN APP (GIỮ NGUYÊN CODE CŨ CỦA BẠN) ==================
        [Step("Mở App Music Player")]
        public void OpenApp()
        {
            if (_driver != null) return;

            // Logic tìm đường dẫn tương đối (Rất chuẩn, không cần sửa)
            string appPath = Path.GetFullPath(
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "..", "MusicApp", "bin", "Debug", "net9.0-windows", "MusicApp.exe"));

            Console.WriteLine($"--> LEAD CHECK PATH: {appPath}"); // In ra để kiểm tra

            var opt = new AppiumOptions();
            opt.AddAdditionalCapability("app", appPath);
            opt.AddAdditionalCapability("deviceName", "WindowsPC");

            _driver = new WindowsDriver<WindowsElement>(new Uri(Url), opt);
            
            // Tăng thời gian chờ lên 3s để App trên ổ E kịp load
            Thread.Sleep(3000); 
        }

        // ================== MENU (SỬA LỖI TẠI ĐÂY) ==================
        [Step("Kiểm tra nút Menu tồn tại")]
        public void MenuExists()
        {
            _driver.FindElementByAccessibilityId("btnMenu");
        }

// --- SỬA HÀM CLICK MENU (THÊM CƠ CHẾ TỰ BẤM LẠI) ---
        [Step("Bấm nút Menu")]
        public void ClickMenu()
        {
            var btnMenu = _driver.FindElementByAccessibilityId("btnMenu");
            
            Console.WriteLine("--> [Lần 1] Đang bấm nút Menu...");
            btnMenu.Click();
            Thread.Sleep(2000); // Đợi 2s

            // KIỂM TRA NGAY: Liệu Menu đã thực sự mở chưa?
            // Mẹo: Thử tìm nút Open File ngay tại đây. Nếu ko thấy nghĩa là bấm xịt.
            try
            {
                _driver.FindElementByAccessibilityId("btnOpenFile");
                Console.WriteLine("--> (OK) Menu đã mở thành công ngay lần 1.");
            }
            catch
            {
                Console.WriteLine("--> (!) Bấm lần 1 chưa thấy Menu ra. Đang thử bấm lại lần 2...");
                btnMenu.Click(); // Bấm lại phát nữa
                Thread.Sleep(2000);
            }
        }

        // --- SỬA HÀM CHECK OPEN FILE (DÙNG XPATH - VŨ KHÍ CUỐI) ---
        [Step("Kiểm tra nút Open File tồn tại")]
        public void OpenFileExists()
        {
            try
            {
                // Cách 1: Tìm bằng ID (Chuẩn nhất)
                _driver.FindElementByAccessibilityId("btnOpenFile");
                Console.WriteLine("--> (OK) Tìm thấy nút bằng ID: btnOpenFile");
            }
            catch
            {
                Console.WriteLine("--> (!) ID thất bại, thử tìm bằng XPath...");
                try
                {
                    // Cách 2: Dùng XPath (Vũ khí hạng nặng)
                    // Tìm tất cả các nút có AutomationId là 'btnOpenFile'
                    _driver.FindElementByXPath("//Button[@AutomationId='btnOpenFile']");
                    Console.WriteLine("--> (OK) Tìm thấy nút bằng XPath");
                }
                catch
                {
                    Console.WriteLine("--> (!) XPath thất bại, thử tìm bằng Tên...");
                    try 
                    {
                         // Cách 3: Tìm bằng Tên
                        _driver.FindElementByName("Open File");
                        Console.WriteLine("--> (OK) Tìm thấy nút bằng Name");
                    }
                    catch
                    {
                        // Cách 4: In ra mã nguồn giao diện để Lead debug
                        Console.WriteLine("❌ LỖI NGHIÊM TRỌNG: WinAppDriver bị mù!");
                        Console.WriteLine("Dưới đây là danh sách các nút mà máy nhìn thấy:");
                        var allButtons = _driver.FindElementsByClassName("Button");
                        foreach (var btn in allButtons)
                        {
                            try { Console.WriteLine($" - Found Button: {btn.Text} (ID: {btn.GetAttribute("AutomationId")})"); } catch {}
                        }
                        throw new Exception("Không tìm thấy nút Open File bằng mọi cách!");
                    }
                }
            }
        }
        
[Step("Kiểm tra nút List tồn tại")]
        public void ListExists()
        {
            try
            {
                // Cách 1: Tìm bằng ID
                _driver.FindElementByAccessibilityId("btnList");
                Console.WriteLine("--> (OK) Tìm thấy nút List bằng ID");
            }
            catch
            {
                // Cách 2: Tìm bằng Tên hiển thị
                Console.WriteLine("--> (!) Không thấy ID btnList, đang tìm bằng tên...");
                _driver.FindElementByName("List"); 
                Console.WriteLine("--> (OK) Tìm thấy nút List bằng Name");
            }
        }
        // ================== PLAYLIST (GIỮ NGUYÊN) ==================
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

        // ================== CONTROLS (GIỮ NGUYÊN) ==================
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

        // ================== CLEANUP ==================
        [AfterScenario]
        public void Cleanup()
        {
            if (_driver != null)
            {
                _driver.Quit();
                _driver = null;
            }
        }
    }
}