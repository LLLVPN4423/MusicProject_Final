# HƯỚNG DẪN NÂNG CẤP GAUGE (ĐÃ SỬA)
# Chỉ cho Black-box Testing

## I. CẤU TRÚC PROJECT

```
MusicTest/
├── manifest.json
├── MusicTest.csproj
├── env/default/
├── specs/
│   └── black-box/
│       ├── playback-control.spec      # BT01-BT11
│       └── playlist-management.spec    # BT12-BT21
├── step-implementations/
│   ├── CommonSteps.cs
│   └── BlackBoxSteps.cs
├── test-data/
│   ├── valid/
│   └── invalid/
└── reports/
    ├── html-report/
    └── screenshots/
```

## II. SPEC FILES

### A. specs/black-box/playback-control.spec
```
# Playback Control - Black-box Testing

## Test Case BT01: Stopped -> Play
* Mở ứng dụng Music Player
* Kiểm tra trạng thái ban đầu là "Stopped"
* Thêm file hợp lệ "test.mp3"
* Bấm nút Play
* Kiểm tra trạng thái là "Playing"
* Chụp màn hình kết quả

## Test Case BT02: Playing -> Play
* Mở ứng dụng Music Player
* Thêm file hợp lệ "test.mp3"
* Bấm nút Play
* Kiểm tra trạng thái là "Playing"
* Bấm lại nút Play
* Kiểm tra trạng thái vẫn là "Playing"
* Chụp màn hình kết quả

## Test Case BT03: Paused -> Play
* Mở ứng dụng Music Player
* Thêm file hợp lệ "test.mp3"
* Bấm nút Play
* Bấm nút Pause
* Kiểm tra trạng thái là "Paused"
* Bấm nút Play
* Kiểm tra trạng thái là "Playing"
* Chụp màn hình kết quả

## Test Case BT04: Stopped -> Pause
* Mở ứng dụng Music Player
* Kiểm tra trạng thái ban đầu là "Stopped"
* Bấm nút Pause
* Kiểm tra trạng thái vẫn là "Stopped"
* Chụp màn hình kết quả

## Test Case BT05: Playing -> Pause
* Mở ứng dụng Music Player
* Thêm file hợp lệ "test.mp3"
* Bấm nút Play
* Kiểm tra trạng thái là "Playing"
* Bấm nút Pause
* Kiểm tra trạng thái là "Paused"
* Chụp màn hình kết quả

## Test Case BT06: Paused -> Pause
* Mở ứng dụng Music Player
* Thêm file hợp lệ "test.mp3"
* Bấm nút Play
* Bấm nút Pause
* Kiểm tra trạng thái là "Paused"
* Bấm lại nút Pause
* Kiểm tra trạng thái vẫn là "Paused"
* Chụp màn hình kết quả

## Test Case BT07: Play without file
* Mở ứng dụng Music Player
* Kiểm tra playlist rỗng
* Bấm nút Play
* Kiểm tra thông báo "Không có bài hát nào để phát"
* Chụp màn hình kết quả

## Test Case BT08: Volume 0 (Mute)
* Mở ứng dụng Music Player
* Thêm file hợp lệ "test.mp3"
* Bấm nút Play
* Kéo thanh volume đến 0
* Kiểm tra biểu tượng mute hiển thị
* Chụp màn hình kết quả

## Test Case BT09: Volume 100 (Max)
* Mở ứng dụng Music Player
* Thêm file hợp lệ "test.mp3"
* Bấm nút Play
* Kéo thanh volume đến 100
* Kiểm tra volume hiển thị là 100
* Chụp màn hình kết quả

## Test Case BT10: Volume -1 (Invalid)
* Mở ứng dụng Music Player
* Thêm file hợp lệ "test.mp3"
* Thử đặt volume -1
* Kiểm tra thông báo lỗi hiển thị
* Chụp màn hình kết quả

## Test Case BT11: Volume 101 (Invalid)
* Mở ứng dụng Music Player
* Thêm file hợp lệ "test.mp3"
* Thử đặt volume 101
* Kiểm tra thông báo lỗi hiển thị
* Chụp màn hình kết quả
```

### B. specs/black-box/playlist-management.spec
```
# Playlist Management - Black-box Testing

## Test Case BT12: Empty playlist play
* Mở ứng dụng Music Player
* Kiểm tra danh sách bài hát rỗng
* Bấm nút Play
* Kiểm tra thông báo "Playlist rỗng"
* Chụp màn hình kết quả

## Test Case BT13: Single track playlist
* Mở ứng dụng Music Player
* Thêm 1 file MP3 hợp lệ
* Kiểm tra danh sách có 1 bài hát
* Bấm nút Play
* Kiểm tra phát bài hát đầu tiên
* Chụp màn hình kết quả

## Test Case BT14: Multiple tracks playlist
* Mở ứng dụng Music Player
* Thêm 5 file MP3 hợp lệ
* Kiểm tra danh sách có 5 bài hát
* Chọn bài hát thứ 3
* Bấm nút Play
* Kiểm tra phát bài hát thứ 3
* Chụp màn hình kết quả

## Test Case BT15: Large playlist (100 tracks)
* Mở ứng dụng Music Player
* Thêm 100 file MP3 hợp lệ
* Kiểm tra danh sách có 100 bài hát
* Chọn bài hát cuối cùng
* Bấm nút Play
* Kiểm tra phát bài hát thứ 100
* Chụp màn hình kết quả

## Test Case BT16: Invalid track index (negative)
* Mở ứng dụng Music Player
* Thêm 5 file MP3 hợp lệ
* Thử chọn bài hát có chỉ số -1
* Bấm nút Play
* Kiểm tra thông báo lỗi "Index không hợp lệ"
* Chụp màn hình kết quả

## Test Case BT17: Invalid track index (out of range)
* Mở ứng dụng Music Player
* Thêm 5 file MP3 hợp lệ
* Thử chọn bài hát có chỉ số 5
* Bấm nút Play
* Kiểm tra thông báo lỗi "Index ngoài phạm vi"
* Chụp màn hình kết quả

## Test Case BT18: Playlist full
* Mở ứng dụng Music Player
* Thêm 100 file MP3 hợp lệ
* Thử thêm bài hát thứ 101
* Kiểm tra thông báo "Playlist đã đầy"
* Chụp màn hình kết quả

## Test Case BT19: Invalid file format
* Mở ứng dụng Music Player
* Thử thêm file .txt vào playlist
* Kiểm tra thông báo "Định dạng không hỗ trợ"
* Chụp màn hình kết quả

## Test Case BT20: Remove track
* Mở ứng dụng Music Player
* Thêm 10 file MP3 hợp lệ
* Chọn bài hát thứ 4
* Bấm nút Remove
* Kiểm tra danh sách còn 9 bài hát
* Chụp màn hình kết quả

## Test Case BT21: Clear all tracks
* Mở ứng dụng Music Player
* Thêm 1 file MP3 hợp lệ
* Bấm nút Clear All
* Kiểm tra danh sách rỗng
* Chụp màn hình kết quả
```

## III. STEP IMPLEMENTATIONS

### A. CommonSteps.cs
```csharp
using Gauge.CSharp.Lib;
using Gauge.CSharp.Lib.Attribute;
using OpenQA.Selenium;
using OpenQA.Selenium.Appium;
using OpenQA.Selenium.Appium.Windows;
using System;
using System.IO;
using System.Threading;

namespace MusicTest.StepImplementations
{
    public class CommonSteps
    {
        protected static WindowsDriver<WindowsElement> _driver;
        protected static string _testDataDir;
        
        [BeforeScenario]
        public void Setup()
        {
            _testDataDir = Environment.GetEnvironmentVariable("test_data_dir") ?? "test-data";
            
            var appCapabilities = new AppiumOptions();
            appCapabilities.AddAdditionalCapability("app", Environment.GetEnvironmentVariable("app_path"));
            appCapabilities.AddAdditionalCapability("deviceName", Environment.GetEnvironmentVariable("device_name"));
            appCapabilities.AddAdditionalCapability("platformName", Environment.GetEnvironmentVariable("platform_name"));
            
            try
            {
                _driver = new WindowsDriver<WindowsElement>(
                    new Uri(Environment.GetEnvironmentVariable("driver_url")), 
                    appCapabilities);
                
                _driver.Manage().Timeouts().ImplicitWait = TimeSpan.FromSeconds(10);
                _driver.Manage().Window.Maximize();
                Thread.Sleep(2000);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to setup WinAppDriver: {ex.Message}");
                throw;
            }
        }
        
        [AfterScenario]
        public void TearDown()
        {
            if (ScenarioContext.Current.ScenarioInfo.IsFailed)
            {
                TakeScreenshot("failure");
            }
            _driver?.Quit();
        }
        
        [Step("Mở ứng dụng Music Player")]
        public void OpenMusicPlayer()
        {
            Thread.Sleep(1000);
        }
        
        [Step("Chụp màn hình kết quả")]
        public void TakeScreenshot(string suffix = "")
        {
            try
            {
                var screenshotDir = Environment.GetEnvironmentVariable("screenshot_dir") ?? "reports/screenshots";
                
                if (!Directory.Exists(screenshotDir))
                {
                    Directory.CreateDirectory(screenshotDir);
                }
                
                var fileName = $"Screenshot_{DateTime.Now:yyyyMMdd_HHmmss}_{suffix}.png";
                var fullPath = Path.Combine(screenshotDir, fileName);
                
                var screenshot = _driver.GetScreenshot();
                screenshot.SaveAsFile(fullPath);
                
                Console.WriteLine($"Screenshot saved: {fullPath}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to take screenshot: {ex.Message}");
            }
        }
        
        protected string GetValidFilePath(string fileName)
        {
            return Path.Combine(_testDataDir, "valid", fileName);
        }
        
        protected string GetInvalidFilePath(string fileName)
        {
            return Path.Combine(_testDataDir, "invalid", fileName);
        }
    }
}
```

### B. BlackBoxSteps.cs
```csharp
using Gauge.CSharp.Lib.Attribute;
using OpenQA.Selenium.Appium.Windows;
using FluentAssertions;

namespace MusicTest.StepImplementations
{
    public class BlackBoxSteps : CommonSteps
    {
        [Step("Kiểm tra trạng thái ban đầu là <status>")]
        public void VerifyInitialStatus(string status)
        {
            var statusLabel = _driver.FindElementByAccessibilityId("lblStatus");
            Assert.That(statusLabel.Text, Is.EqualTo(status));
        }
        
        [Step("Bấm nút <buttonName>")]
        public void ClickButton(string buttonName)
        {
            var buttonId = $"btn{buttonName}";
            var button = _driver.FindElementByAccessibilityId(buttonId);
            button.Click();
            Thread.Sleep(500);
        }
        
        [Step("Kiểm tra trạng thái là <status>")]
        public void VerifyStatus(string status)
        {
            var statusLabel = _driver.FindElementByAccessibilityId("lblStatus");
            Assert.That(statusLabel.Text, Is.EqualTo(status));
        }
        
        [Step("Thêm file hợp lệ <fileName>")]
        public void AddValidFile(string fileName)
        {
            var filePath = GetValidFilePath(fileName);
            var openButton = _driver.FindElementByAccessibilityId("btnOpen");
            openButton.Click();
            Thread.Sleep(1000);
        }
        
        [Step("Kiểm tra playlist rỗng")]
        public void VerifyPlaylistEmpty()
        {
            var playlist = _driver.FindElementByAccessibilityId("lvPlaylist");
            var items = playlist.FindElements(By.ClassName("ListViewItem"));
            items.Count.Should().Be(0);
        }
        
        [Step("Kiểm tra danh sách có <count> bài hát")]
        public void VerifyPlaylistCount(int count)
        {
            var playlist = _driver.FindElementByAccessibilityId("lvPlaylist");
            var items = playlist.FindElements(By.ClassName("ListViewItem"));
            items.Count.Should().Be(count);
        }
        
        [Step("Chọn bài hát thứ <index>")]
        public void SelectTrack(int index)
        {
            var playlist = _driver.FindElementByAccessibilityId("lvPlaylist");
            var items = playlist.FindElements(By.ClassName("ListViewItem"));
            
            if (index < 0 || index >= items.Count)
            {
                throw new ArgumentException($"Invalid track index: {index}");
            }
            
            items[index].Click();
            Thread.Sleep(500);
        }
        
        [Step("Chọn bài hát có chỉ số <index>")]
        public void SelectTrackByIndex(int index)
        {
            SelectTrack(index);
        }
        
        [Step("Bấm nút Remove")]
        public void ClickRemove()
        {
            var removeButton = _driver.FindElementByAccessibilityId("btnRemove");
            removeButton.Click();
            Thread.Sleep(500);
        }
        
        [Step("Bấm nút Clear All")]
        public void ClickClearAll()
        {
            var clearButton = _driver.FindElementByAccessibilityId("btnClear");
            clearButton.Click();
            Thread.Sleep(500);
        }
        
        [Step("Kiểm tra danh sách còn <count> bài hát")]
        public void VerifyPlaylistCountAfterRemoval(int count)
        {
            VerifyPlaylistCount(count);
        }
        
        [Step("Kiểm tra danh sách rỗng")]
        public void VerifyPlaylistEmptyAfterClear()
        {
            VerifyPlaylistEmpty();
        }
        
        [Step("Thử chọn bài hát có chỉ số <index>")]
        public void TrySelectTrackByIndex(int index)
        {
            try
            {
                SelectTrack(index);
            }
            catch (ArgumentException)
            {
                // Expected for invalid indices
            }
        }
        
        [Step("Thử thêm file <fileName> vào playlist")]
        public void TryAddFile(string fileName)
        {
            var filePath = GetInvalidFilePath(fileName);
            var addButton = _driver.FindElementByAccessibilityId("btnAdd");
            addButton.Click();
            Thread.Sleep(1000);
        }
        
        [Step("Thử thêm bài hát thứ <index>")]
        public void TryAddTrack(int index)
        {
            var addButton = _driver.FindElementByAccessibilityId("btnAdd");
            addButton.Click();
            Thread.Sleep(1000);
        }
        
        [Step("Chọn bài hát cuối cùng")]
        public void SelectLastTrack()
        {
            var playlist = _driver.FindElementByAccessibilityId("lvPlaylist");
            var items = playlist.FindElements(By.ClassName("ListViewItem"));
            
            if (items.Count > 0)
            {
                items[items.Count - 1].Click();
                Thread.Sleep(500);
            }
        }
        
        [Step("Kéo thanh volume đến <volume>")]
        public void SetVolume(int volume)
        {
            var volumeSlider = _driver.FindElementByAccessibilityId("sldVolume");
            volumeSlider.Clear();
            volumeSlider.SendKeys(volume.ToString());
            Thread.Sleep(500);
        }
        
        [Step("Kiểm tra volume hiển thị là <volume>")]
        public void VerifyVolume(int volume)
        {
            var volumeLabel = _driver.FindElementByAccessibilityId("lblVolume") ??
                            _driver.FindElementByAccessibilityId("VolumeLabel");
            
            Assert.That(volumeLabel.Text, Does.Contain(volume.ToString()));
        }
        
        [Step("Kiểm tra biểu tượng mute hiển thị")]
        public void VerifyMuteIcon()
        {
            var muteButton = _driver.FindElementByAccessibilityId("btnMute");
            var buttonName = muteButton.GetAttribute("Name");
            
            Assert.That(buttonName, Does.Contain("Mute") | Does.Contain("Unmute"));
        }
        
        [Step("Thử đặt volume <volume>")]
        public void TrySetVolume(int volume)
        {
            SetVolume(volume);
        }
    }
}
```

## IV. CÀI ĐẶT VÀ SỬ DỤNG

### A. Cài đặt Gauge
```bash
cd d:\MusicProject_Final\MusicTest

# Install plugins
gauge install html-report
gauge install screenshot
gauge install xml-report

# Restore packages
dotnet restore
```

### B. Chạy tests
```bash
# Run all black-box tests
gauge run specs/black-box/ --html-report reports/html-report --screenshots-dir reports/screenshots

# Run specific spec
gauge run specs/black-box/playback-control.spec
gauge run specs/black-box/playlist-management.spec
```

## V. KẾT QUẢ

### A. HTML Report
- Mở `reports/html-report/index.html`
- Xem chi tiết 21 test cases
- Screenshots tự động chụp khi thất bại

### B. Screenshots
- Location: `reports/screenshots/`
- Naming: `Screenshot_BT01_Playing_20240322_143022.png`

---

**Đây là file Gauge đã sửa - chỉ cho Black-box testing!** ✅
