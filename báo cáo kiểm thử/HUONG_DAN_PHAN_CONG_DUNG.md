# HƯỚNG DẪN PHÂN CÔNG CÔNG CỤ ĐÚNG
# Cho đồ án Kiểm thử phần mềm

## I. PHÂN CÔNG ĐÚNG THEO YÊU CẦU

### A. Yêu cầu đồ án:
- **Chương 2:** Black-box testing → **Gauge**
- **Chương 3:** White-box testing → **NUnit** 
- **Chương 4:** Unit testing → **NUnit**
- **Chương 5:** Demo tự động → **Cả 2**

### B. Phân công chính xác:

| Chương | Loại test | Công cụ | Vai trò |
|---------|-----------|----------|----------|
| Chương 2 | **Black-box** | **Gauge** | BDD specs, UI automation |
| Chương 3 | **White-box** | **NUnit** | Branch/condition coverage |
| Chương 4 | **Unit testing** | **NUnit** | Business logic testing |
| Chương 5 | **Demo** | **Cả 2** | Comprehensive demo |

---

## II. SỬA LẦM TRƯỚC ĐÂY

### A. Sai lầm trong hướng dẫn cũ:
```markdown
# ❌ PHÂN CÔNG SAI
Gauge: Black-box + White-box  # SAI!
NUnit: Chỉ Unit testing      # SAI!
```

### B. Phân công đúng:
```markdown
# ✅ PHÂN CÔNG ĐÚNG
Gauge: Chỉ Black-box (Chương 2)
NUnit: White-box (Chương 3) + Unit testing (Chương 4)
```

---

## III. CẦN SỬA LẠI CÁC FILE HƯỚNG DẪN

### A. File cần sửa:

#### 1. HUONG_DAN_NANG_CAP_GAUGE.md
**❌ Cần bỏ:**
- `specs/white-box/` folder
- White-box specs (media-loading.spec, volume-control.spec)
- White-box step implementations

**✅ Chỉ giữ:**
- `specs/black-box/` folder
- Black-box specs (playback-control.spec, playlist-management.spec)
- Black-box step implementations

#### 2. HUONG_DAN_CAI_DAT_NUNIT.md
**❌ Cần thêm:**
- White-box test methods (branch coverage)
- Code coverage analysis
- Path testing implementations

**✅ Giữ nguyên:**
- Unit test methods
- Test data setup
- Coverage configuration

#### 3. HUONG_DAN_LAY_DU_LIEU_BAO_CAO.md
**❌ Cần sửa:**
- Data extraction mapping
- Report generation logic
- Summary calculations

---

## IV. CẤU TRÚC ĐÚNG

### A. Gauge Project Structure (Chỉ Black-box)
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

### B. NUnit Project Structure (White-box + Unit)
```
MusicTest.NUnit/
├── MusicTest.NUnit.csproj
├── Tests/
│   ├── WhiteBox/
│   │   ├── MediaLoadingTests.cs      # WT01-WT10
│   │   └── VolumeControlTests.cs     # WT11-WT18
│   ├── Unit/
│   │   ├── MediaPlayerServiceTests.cs
│   │   ├── VolumeControlServiceTests.cs
│   │   └── PlaylistServiceTests.cs
│   └── Helpers/
│       ├── TestBase.cs
│       └── TestDataHelper.cs
├── TestData/
└── TestResults/
    ├── Coverage/
    └── Reports/
```

---

## V. TEST CASES PHÂN CÔNG ĐÚNG

### A. Gauge - Black-box Testing (21 test cases)

#### 1. Playback Control (BT01-BT11)
```gherkin
# specs/black-box/playback-control.spec

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

# ... (BT03-BT11)
```

#### 2. Playlist Management (BT12-BT21)
```gherkin
# specs/black-box/playlist-management.spec

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

# ... (BT14-BT21)
```

### B. NUnit - White-box Testing (18 test cases)

#### 1. Media Loading (WT01-WT10)
```csharp
// Tests/WhiteBox/MediaLoadingTests.cs
[TestFixture]
public class MediaLoadingTests : TestBase
{
    private MediaPlayerService _mediaPlayerService;

    [SetUp]
    public void Setup()
    {
        _mediaPlayerService = new MediaPlayerService(MockLogger.Object);
    }

    // WT01: Null file path - Branch 1.1
    [Test]
    public void LoadMedia_NullFilePath_CoversBranch1_1_ReturnsFalse()
    {
        // Arrange
        string filePath = null;

        // Act
        var result = _mediaPlayerService.LoadMedia(filePath);

        // Assert
        result.Should().BeFalse();
        // Verify branch 1.1 was covered
    }

    // WT02: Empty file path - Branch 1.1
    [Test]
    public void LoadMedia_EmptyFilePath_CoversBranch1_1_ReturnsFalse()
    {
        // Arrange
        string filePath = "";

        // Act
        var result = _mediaPlayerService.LoadMedia(filePath);

        // Assert
        result.Should().BeFalse();
        // Verify branch 1.1 was covered
    }

    // WT03: Non-existent file - Branch 1.2
    [Test]
    public void LoadMedia_NonExistentFile_CoversBranch1_2_ReturnsFalse()
    {
        // Arrange
        string filePath = "nonexistent.mp3";

        // Act
        var result = _mediaPlayerService.LoadMedia(filePath);

        // Assert
        result.Should().BeFalse();
        // Verify branch 1.2 was covered
    }

    // WT04: Valid MP3 file - Branch 3.2.1
    [Test]
    public void LoadMedia_ValidMP3_CoversBranch3_2_1_ReturnsTrue()
    {
        // Arrange
        string filePath = GetValidFilePath("test.mp3");

        // Act
        var result = _mediaPlayerService.LoadMedia(filePath);

        // Assert
        result.Should().BeTrue();
        // Verify branch 3.2.1 was covered
    }

    // ... (WT05-WT10)
}
```

#### 2. Volume Control (WT11-WT18)
```csharp
// Tests/WhiteBox/VolumeControlTests.cs
[TestFixture]
public class VolumeControlTests : TestBase
{
    private VolumeControlService _volumeService;

    [SetUp]
    public void Setup()
    {
        _volumeService = new VolumeControlService(MockLogger.Object);
    }

    // WT11: Volume below minimum - Branch 1.1
    [Test]
    public void SetVolume_BelowMinimum_CoversBranch1_1_ReturnsFalse()
    {
        // Arrange
        int volume = -1;

        // Act
        var result = _volumeService.SetVolume(volume);

        // Assert
        result.Should().BeFalse();
        // Verify branch 1.1 was covered
    }

    // WT16: Volume >0 when muted - Branch 2.1.1
    [Test]
    public void SetVolume_PositiveWhenMuted_CoversBranch2_1_1_ReturnsTrue()
    {
        // Arrange
        _volumeService.Mute();
        int volume = 50;

        // Act
        var result = _volumeService.SetVolume(volume);

        // Assert
        result.Should().BeTrue();
        _volumeService.IsMuted.Should().BeFalse();
        // Verify branch 2.1.1 was covered
    }

    // WT17: Volume 0 when muted - Branch 2.1.2
    [Test]
    public void SetVolume_ZeroWhenMuted_CoversBranch2_1_2_ReturnsTrue()
    {
        // Arrange
        _volumeService.Mute();
        int volume = 0;

        // Act
        var result = _volumeService.SetVolume(volume);

        // Assert
        result.Should().BeTrue();
        _volumeService.IsMuted.Should().BeTrue();
        // Verify branch 2.1.2 was covered
    }

    // ... (WT12-WT15, WT18)
}
```

### C. NUnit - Unit Testing (18 test cases)

#### 1. Service Layer Tests
```csharp
// Tests/Unit/MediaPlayerServiceTests.cs
[TestFixture]
public class MediaPlayerServiceTests : TestBase
{
    private MediaPlayerService _mediaPlayerService;

    [SetUp]
    public void Setup()
    {
        _mediaPlayerService = new MediaPlayerService(MockLogger.Object);
    }

    [Test]
    public void LoadMedia_ValidFile_ReturnsTrue()
    {
        // Arrange
        var filePath = GetValidFilePath("test.mp3");

        // Act
        var result = _mediaPlayerService.LoadMedia(filePath);

        // Assert
        result.Should().BeTrue();
    }

    [Test]
    public void LoadMedia_InvalidFile_ReturnsFalse()
    {
        // Arrange
        var filePath = GetInvalidFilePath("test.txt");

        // Act
        var result = _mediaPlayerService.LoadMedia(filePath);

        // Assert
        result.Should().BeFalse();
    }

    // ... more unit tests
}
```

---

## VI. CẦN CẬP NHẬT CÁC FILE

### A. Sửa HUONG_DAN_NANG_CAP_GAUGE.md
**Bỏ phần white-box:**
```markdown
# ❌ BỎ PHẦN NÀY
## III. BƯỚC 2: TẠO SPEC FILES

### B. White-box Specs
#### specs/white-box/media-loading.spec
#### specs/white-box/volume-control.spec
```

**Chỉ giữ black-box:**
```markdown
# ✅ GIỮ LẠI PHẦN NÀY
## III. BƯỚC 2: TẠO SPEC FILES

### A. Black-box Specs
#### specs/black-box/playback-control.spec
#### specs/black-box/playlist-management.spec
```

### B. Cập nhật HUONG_DAN_CAI_DAT_NUNIT.md
**Thêm white-box testing:**
```markdown
## IV. VIẾT WHITE-BOX TESTS

### A. Test cho Media Loading (WT01-WT10)
### B. Test cho Volume Control (WT11-WT18)

### C. Branch Coverage Analysis
### D. Condition Coverage Analysis
```

### C. Cập nhật HUONG_DAN_LAY_DU_LIEU_BAO_CAO.md
**Sửa mapping:**
```python
# ❌ SAI:
black_box_tests = gauge_results  # Đúng
white_box_tests = gauge_results  # SAI!

# ✅ ĐÚNG:
black_box_tests = gauge_results
white_box_tests = nunit_results
unit_tests = nunit_results
```

---

## VII. KẾ HOẠCH THỰC HIỆN ĐÚNG

### A. Tuần 1: Setup và Testing
1. **Gauge Setup:**
   - Chỉ cài đặt black-box specs
   - 21 test cases (BT01-BT21)
   - UI automation focus

2. **NUnit Setup:**
   - White-box tests (WT01-WT18)
   - Unit tests (18 tests)
   - Code coverage focus

### B. Tuần 2: Execution và Data Collection
1. **Gauge Execution:**
   - Run black-box tests
   - Collect screenshots
   - Generate HTML report

2. **NUnit Execution:**
   - Run white-box + unit tests
   - Collect coverage data
   - Generate coverage report

### C. Tuần 3: Report Completion
1. **Data Integration:**
   - Combine results from both tools
   - Generate comprehensive report
   - Fill report document

---

## VIII. SUMMARY PHÂN CÔNG ĐÚNG

### A. Final Assignment:
| Công cụ | Chương | Loại test | Số test cases | Vai trò |
|----------|---------|-----------|----------------|----------|
| **Gauge** | Chương 2 | Black-box | 21 (BT01-BT21) | BDD + UI automation |
| **NUnit** | Chương 3 | White-box | 18 (WT01-WT18) | Branch/condition coverage |
| **NUnit** | Chương 4 | Unit testing | 18 | Business logic |
| **Cả 2** | Chương 5 | Demo | - | Comprehensive demo |

### B. Benefits của phân công đúng:
1. **Gauge:** Tối ưu cho BDD và UI automation
2. **NUnit:** Mạnh nhất cho unit testing và white-box analysis
3. **Clear separation:** Dễ quản lý và maintain
4. **Industry standard:** Phân công theo best practices

---

**KẾT LUẬN:** Bây giờ bạn có phân công công cụ **đúng theo yêu cầu đồ án**! 🎯

- **Gauge:** Chỉ cho black-box testing
- **NUnit:** Cho cả white-box và unit testing
- **Demo:** Sử dụng cả 2 công cụ

**Cần sửa các file hướng dẫn theo phân công đúng này!** ✅
