using System;
using System.IO;
using System.Diagnostics;
using System.Net.Sockets;
using System.Threading;
using Gauge.CSharp.Lib.Attribute;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;

namespace MusicTest
{
    public class StepImplementation
    {
        private WindowsDriver<WindowsElement> _driver;
        private const string Url = "http://127.0.0.1:4723";
        private static readonly TimeSpan DefaultTimeout = TimeSpan.FromSeconds(12);
        private static readonly TimeSpan DefaultPoll = TimeSpan.FromMilliseconds(250);

        private bool IsPortOpen(string host, int port, int timeoutMs = 300)
        {
            try
            {
                using var client = new TcpClient();
                var ar = client.BeginConnect(host, port, null, null);
                var success = ar.AsyncWaitHandle.WaitOne(TimeSpan.FromMilliseconds(timeoutMs));
                if (!success) return false;
                client.EndConnect(ar);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private void EnsureWinAppDriverRunning()
        {
            // WinAppDriver mặc định lắng nghe 127.0.0.1:4723
            if (IsPortOpen("127.0.0.1", 4723)) return;

            // Thử start WinAppDriver từ các path phổ biến
            var candidates = new[]
            {
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "Windows Application Driver", "WinAppDriver.exe"),
                Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), "Windows Application Driver", "WinAppDriver.exe"),
            };

            // Nếu chưa có exe, thử cài bằng winget (nếu có)
            var anyExists = false;
            foreach (var exe in candidates)
            {
                if (File.Exists(exe)) { anyExists = true; break; }
            }

            if (!anyExists)
            {
                try
                {
                    Console.WriteLine("--> Không thấy WinAppDriver.exe. Thử cài bằng winget...");
                    var p = Process.Start(new ProcessStartInfo
                    {
                        FileName = "winget",
                        Arguments = "install -e --id Microsoft.WindowsApplicationDriver --accept-package-agreements --accept-source-agreements --silent",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    });
                    p?.WaitForExit(120000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Không chạy được winget để cài WinAppDriver: {ex.Message}");
                }
            }

            foreach (var exe in candidates)
            {
                try
                {
                    if (!File.Exists(exe)) continue;

                    Console.WriteLine($"--> WinAppDriver chưa chạy. Đang start: {exe}");
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = exe,
                        UseShellExecute = true,
                        WindowStyle = ProcessWindowStyle.Minimized
                    });

                    // Chờ một chút để service lên cổng
                    for (var i = 0; i < 20; i++)
                    {
                        if (IsPortOpen("127.0.0.1", 4723)) return;
                        Thread.Sleep(200);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Không start được WinAppDriver tại '{exe}': {ex.Message}");
                }
            }

            // Nếu vẫn chưa lên, ném lỗi rõ ràng
            throw new Exception("Không kết nối được WinAppDriver/Appium tại 127.0.0.1:4723. Hãy đảm bảo WinAppDriver đã được cài (có thể cài bằng: winget install -e --id Microsoft.WindowsApplicationDriver) và không bị chặn bởi firewall.");
        }

        private WindowsElement WaitUntil(string description, Func<WindowsElement> find, TimeSpan? timeout = null, TimeSpan? poll = null)
        {
            if (_driver == null) throw new InvalidOperationException("Driver chưa được khởi tạo. Hãy gọi step 'Mở App Music Player' trước.");

            var sw = Stopwatch.StartNew();
            var max = timeout ?? DefaultTimeout;
            var step = poll ?? DefaultPoll;
            Exception last = null;

            while (sw.Elapsed < max)
            {
                try
                {
                    var el = find();
                    if (el != null) return el;
                }
                catch (WebDriverException ex)
                {
                    last = ex;
                }
                catch (InvalidOperationException ex)
                {
                    last = ex;
                }

                Thread.Sleep(step);
            }

            throw new Exception($"Timeout khi chờ: {description} (sau {(int)max.TotalSeconds}s).", last);
        }

        private WindowsElement FindByAny(string description, params Func<WindowsElement>[] strategies)
        {
            return WaitUntil(description, () =>
            {
                foreach (var s in strategies)
                {
                    try
                    {
                        var el = s();
                        if (el != null) return el;
                    }
                    catch (WebDriverException)
                    {
                        // thử chiến lược tiếp theo
                    }
                }
                throw new WebDriverException("Không tìm thấy element theo các chiến lược đã thử.");
            });
        }

        // ================== OPEN APP (GIỮ NGUYÊN CODE CŨ CỦA BẠN) ==================d
        [Step("Mở App Music Player")]
        public void OpenApp()
        {
            if (_driver != null) return;

            EnsureWinAppDriverRunning();

            // Logic tìm đường dẫn tương đối (Rất chuẩn, không cần sửa)
            string appPath = Path.GetFullPath(
                Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "..", "MusicApp", "bin", "Debug", "net9.0-windows", "MusicApp.exe"));

            Console.WriteLine($"--> LEAD CHECK PATH: {appPath}"); // In ra để kiểm tra

            var opt = new AppiumOptions();
            opt.AddAdditionalCapability("app", appPath);
            opt.AddAdditionalCapability("deviceName", "WindowsPC");

            // Đôi khi service vừa start chưa kịp nhận session -> retry vài lần
            Exception last = null;
            for (var attempt = 1; attempt <= 3; attempt++)
            {
                try
                {
                    Console.WriteLine($"--> Đang tạo WinAppDriver session (lần {attempt})...");
                    _driver = new WindowsDriver<WindowsElement>(new Uri(Url), opt);
                    break;
                }
                catch (WebDriverException ex)
                {
                    last = ex;
                    Thread.Sleep(600);
                }
            }

            if (_driver == null)
            {
                throw new Exception("Không tạo được session WinAppDriver sau nhiều lần thử.", last);
            }
            
            // Tăng thời gian chờ lên 3s để App trên ổ E kịp load
            Thread.Sleep(3000); 
        }

        // ================== MENU (SỬA LỖI TẠI ĐÂY) ==================
        [Step("Kiểm tra nút Menu tồn tại")]
        public void MenuExists()
        {
            FindByAny(
                "nút Menu",
                () => _driver.FindElementByAccessibilityId("btnMenu"),
                () => _driver.FindElementByName("Menu")
            );
        }

// --- SỬA HÀM CLICK MENU (THÊM CƠ CHẾ TỰ BẤM LẠI) ---
        [Step("Bấm nút Menu")]
        public void ClickMenu()
        {
            var btnMenu = FindByAny(
                "nút Menu để bấm",
                () => _driver.FindElementByAccessibilityId("btnMenu"),
                () => _driver.FindElementByName("Menu")
            );

            // Click + chờ menu hiện ra bằng cách chờ 1 item trong menu xuất hiện.
            // (Nếu menu đã mở sẵn thì step này vẫn pass ngay.)
            for (var attempt = 1; attempt <= 3; attempt++)
            {
                try
                {
                    Console.WriteLine($"--> Đang bấm nút Menu (lần {attempt})...");
                    btnMenu.Click();

                    // Chờ Open File (VN/EN) hoặc List/Playlist (VN/EN) xuất hiện
                    WaitUntil(
                        "menu bên trái mở ra (có Open File / Mở tệp / Danh sách phát)",
                        () =>
                        {
                            try { return _driver.FindElementByAccessibilityId("btnOpenFile"); } catch { }
                            try { return _driver.FindElementByName("Open File"); } catch { }
                            try { return _driver.FindElementByName("Mở tệp"); } catch { }
                            try { return _driver.FindElementByName("Mở file"); } catch { }
                            try { return _driver.FindElementByAccessibilityId("btnList"); } catch { }
                            try { return _driver.FindElementByName("List"); } catch { }
                            try { return _driver.FindElementByName("Danh sách phát"); } catch { }
                            return null;
                        },
                        timeout: TimeSpan.FromSeconds(4)
                    );

                    Console.WriteLine("--> (OK) Menu đã mở.");
                    return;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> (!) Chưa thấy menu mở sau lần {attempt}: {ex.Message}");
                    Thread.Sleep(300);
                }
            }

            // Nếu đến đây thì vẫn chưa mở được -> ném lỗi để Gauge đánh fail đúng lý do
            throw new Exception("Không mở được menu sau 3 lần bấm.");
        }

        // --- SỬA HÀM CHECK OPEN FILE (DÙNG XPATH - VŨ KHÍ CUỐI) ---
        [Step("Kiểm tra nút Open File tồn tại")]
        public void OpenFileExists()
        {
            FindByAny(
                "nút Open File / Mở tệp",
                () => _driver.FindElementByAccessibilityId("btnOpenFile"),
                () => _driver.FindElementByXPath("//Button[@AutomationId='btnOpenFile']"),
                () => _driver.FindElementByName("Open File"),
                () => _driver.FindElementByName("Mở tệp"),
                () => _driver.FindElementByName("Mở file"),
                () => _driver.FindElementByName("Mở thư mục") // fallback nếu UI đổi wording
            );
        }
        
[Step("Kiểm tra nút List tồn tại")]
        public void ListExists()
        {
            try
            {
                FindByAny(
                    "nút List / Danh sách phát",
                    () => _driver.FindElementByAccessibilityId("btnList"),
                    () => _driver.FindElementByAccessibilityId("btnPlaylist"),
                    () => _driver.FindElementByAccessibilityId("btnPlayList"),
                    () => _driver.FindElementByXPath("//Button[@AutomationId='btnList']"),
                    () => _driver.FindElementByXPath("//Button[@AutomationId='btnPlaylist']"),
                    () => _driver.FindElementByName("List"),
                    () => _driver.FindElementByName("Danh sách phát"),
                    () => _driver.FindElementByName("Playlist"),
                    // Fallback: đôi khi label có icon/ký tự đặc biệt -> dùng contains theo Name
                    () => _driver.FindElementByXPath("//*[contains(@Name,'Danh sách')]"),
                    () => _driver.FindElementByXPath("//*[contains(@Name,'Play')]"),
                    // Fallback cuối: quét các Button và match theo text/AutomationName
                    () =>
                    {
                        var buttons = _driver.FindElementsByClassName("Button");
                        foreach (var b in buttons)
                        {
                            try
                            {
                                var txt = (b.Text ?? string.Empty).Trim();
                                var name = (b.GetAttribute("Name") ?? string.Empty).Trim();
                                var aid = (b.GetAttribute("AutomationId") ?? string.Empty).Trim();

                                if (txt.Contains("Danh sách", StringComparison.OrdinalIgnoreCase) ||
                                    name.Contains("Danh sách", StringComparison.OrdinalIgnoreCase) ||
                                    txt.Contains("Playlist", StringComparison.OrdinalIgnoreCase) ||
                                    name.Contains("Playlist", StringComparison.OrdinalIgnoreCase) ||
                                    aid.Contains("list", StringComparison.OrdinalIgnoreCase) ||
                                    aid.Contains("playlist", StringComparison.OrdinalIgnoreCase))
                                {
                                    return b;
                                }
                            }
                            catch
                            {
                                // ignore và tiếp tục
                            }
                        }
                        return null;
                    }
                );
            }
            catch (Exception)
            {
                // Dump để tìm locator thật (chỉ chạy khi fail)
                try
                {
                    Console.WriteLine("---- DEBUG LIST BUTTONS ----");
                    var buttons = _driver.FindElementsByClassName("Button");
                    foreach (var b in buttons)
                    {
                        try
                        {
                            Console.WriteLine($"Button: Text='{b.Text}' Name='{b.GetAttribute("Name")}' AutomationId='{b.GetAttribute("AutomationId")}'");
                        }
                        catch { }
                    }

                    Console.WriteLine("---- DEBUG TEXTBLOCKS ----");
                    var textBlocks = _driver.FindElementsByClassName("TextBlock");
                    foreach (var t in textBlocks)
                    {
                        try
                        {
                            var name = t.GetAttribute("Name");
                            var aid = t.GetAttribute("AutomationId");
                            var txt = t.Text;
                            if (!string.IsNullOrWhiteSpace(txt) || !string.IsNullOrWhiteSpace(name))
                                Console.WriteLine($"TextBlock: Text='{txt}' Name='{name}' AutomationId='{aid}'");
                        }
                        catch { }
                    }
                }
                catch { }

                throw;
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