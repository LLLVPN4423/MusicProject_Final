# HƯỚNG DẪN THIẾT KẾ TEST CASE THỰC TẾ
# Cho ứng dụng Music Player hiện tại

## I. PHÂN TÍCH ỨNG DỤNG HIỆN TẠI

### A. Các chức năng chính có trong Music Player:

#### 1. **Media Loading** (Chức năng quan trọng nhất)
- **File → Open**: Mở file nhạc
- **Drag & Drop**: Kéo thả file vào playlist
- **File formats hỗ trợ**: mp3, wav, flac, m4a
- **Validation**: Kiểm tra file tồn tại, format hợp lệ

#### 2. **Playback Control** (Chức năng cốt lõi)
- **Play/Pause Button**: Chuyển trạng thái phát/dừng
- **Stop Button**: Dừng hoàn toàn
- **Next/Previous**: Chuyển bài
- **Progress Bar**: Seek và hiển thị tiến độ
- **Volume Control**: Tăng/giảm âm lượng, mute

#### 3. **Playlist Management** (Chức năng quản lý)
- **Add Files**: Thêm file vào playlist
- **Remove Selected**: Xóa bài hát được chọn
- **Clear All**: Xóa toàn bộ playlist
- **Shuffle**: Phát ngẫu nhiên
- **Repeat**: Lặp lại
- **Search**: Tìm kiếm bài hát

#### 4. **Favorites & History** (Chức năng bổ sung)
- **Add to Favorites**: Thêm vào danh sách yêu thích
- **Remove from Favorites**: Xóa khỏi yêu thích
- **View History**: Xem lịch sử phát
- **Clear History**: Xóa lịch sử

#### 5. **UI Controls** (Chức năng giao diện)
- **Tab Navigation**: Chuyển tab Playlist/Favorites/History
- **Search Box**: Tìm kiếm
- **Status Display**: Hiển thị trạng thái
- **Track Info**: Hiển thị thông tin bài hát

---

## II. CHƯƠNG 2: BLACK-BOX TESTING (21 TEST CASES)

### A. Chức năng 1: PLAYBACK CONTROL (11 test cases)

#### 1. Phân vùng tương đương

| Input Variable | Valid Partitions | Invalid Partitions | Test Cases |
|---------------|------------------|-------------------|------------|
| Media State | Playing, Paused, Stopped | Corrupted, Loading | BT01-BT06 |
| File Status | Valid file, No file | Corrupted file, Invalid format | BT07 |
| Volume Level | 0-100 | <0, >100 | BT08-BT11 |

#### 2. Phân tích giá trị biên

| Variable | Min | Min+ | Nominal | Max- | Max | Invalid |
|----------|------|------|---------|------|-----|---------|
| Volume | 0 | 1 | 50 | 99 | 100 | -1, 101 |
| Progress | 0 | 1 | middle | duration-1 | duration | -1, duration+1 |

#### 3. Bảng quyết định

| Test ID | Initial State | Action | Expected Result | Volume | File Status | Priority |
|---------|---------------|--------|----------------|--------|-------------|----------|
| BT01 | Stopped | Click Play | Playing | Any | Valid file | High |
| BT02 | Playing | Click Play | Playing | Any | Valid file | High |
| BT03 | Paused | Click Play | Playing | Any | Valid file | High |
| BT04 | Stopped | Click Pause | Stopped | Any | Valid file | Medium |
| BT05 | Playing | Click Pause | Paused | Any | Valid file | High |
| BT06 | Paused | Click Pause | Paused | Any | Valid file | Medium |
| BT07 | Any | Click Play | Error message | Any | No file | High |
| BT08 | Playing | Volume=0 | Muted | 0 | Valid file | Medium |
| BT09 | Playing | Volume=100 | Max volume | 100 | Valid file | Medium |
| BT10 | Any | Volume=-1 | Error message | -1 | Valid file | High |
| BT11 | Any | Volume=101 | Error message | 101 | Valid file | High |

#### 4. Test Cases chi tiết cho Playback Control

**BT01: Stopped → Play**
- **Preconditions:** Ứng dụng mở, không có file nhạc
- **Steps:**
  1. Mở file mp3 hợp lệ
  2. Kiểm tra trạng thái là "Stopped"
  3. Click nút Play
- **Expected:** Trạng thái thay đổi thành "Playing", nhạc bắt đầu phát
- **Screenshot:** Chụp màn hình khi trạng thái là "Playing"

**BT02: Playing → Play**
- **Preconditions:** Đang phát nhạc
- **Steps:**
  1. Kiểm tra trạng thái là "Playing"
  2. Click nút Play
- **Expected:** Trạng thái vẫn là "Playing", nhạc tiếp tục phát
- **Screenshot:** Chụp màn hình trạng thái "Playing"

**BT03: Paused → Play**
- **Preconditions:** Nhạc đang tạm dừng
- **Steps:**
  1. Click nút Pause (để chuyển sang Paused)
  2. Kiểm tra trạng thái là "Paused"
  3. Click nút Play
- **Expected:** Trạng thái thay đổi thành "Playing", nhạc tiếp tục phát
- **Screenshot:** Chụp màn hình khi resume playing

**BT04: Stopped → Pause**
- **Preconditions:** Ứng dụng mở, nhạc đã dừng
- **Steps:**
  1. Kiểm tra trạng thái là "Stopped"
  2. Click nút Pause
- **Expected:** Trạng thái vẫn là "Stopped", không có gì xảy ra
- **Screenshot:** Chụp màn hình trạng thái "Stopped"

**BT05: Playing → Pause**
- **Preconditions:** Đang phát nhạc
- **Steps:**
  1. Kiểm tra trạng thái là "Playing"
  2. Click nút Pause
- **Expected:** Trạng thái thay đổi thành "Paused", nhạc tạm dừng
- **Screenshot:** Chụp màn hình trạng thái "Paused"

**BT06: Paused → Pause**
- **Preconditions:** Nhạc đang tạm dừng
- **Steps:**
  1. Kiểm tra trạng thái là "Paused"
  2. Click nút Pause
- **Expected:** Trạng thái vẫn là "Paused", không có gì xảy ra
- **Screenshot:** Chụp màn hình trạng thái "Paused"

**BT07: Play without file**
- **Preconditions:** Ứng dụng mở, playlist rỗng
- **Steps:**
  1. Kiểm tra playlist rỗng
  2. Click nút Play
- **Expected:** Hiển thị thông báo "Không có bài hát nào để phát"
- **Screenshot:** Chụp màn hình thông báo lỗi

**BT08: Volume = 0 (Mute)**
- **Preconditions:** Đang phát nhạc
- **Steps:**
  1. Kéo thanh volume đến 0
  2. Kiểm tra biểu tượng volume
- **Expected:** Biểu tượng mute hiển thị, không có âm thanh
- **Screenshot:** Chụp màn hình volume=0

**BT09: Volume = 100 (Max)**
- **Preconditions:** Đang phát nhạc
- **Steps:**
  1. Kéo thanh volume đến 100
  2. Kiểm tra volume display
- **Expected:** Volume hiển thị "100", âm thanh tối đa
- **Screenshot:** Chụp màn hình volume=100

**BT10: Volume = -1 (Invalid)**
- **Preconditions:** Đang phát nhạc
- **Steps:**
  1. Thử đặt volume -1 (nếu có thể)
  2. Kiểm tra thông báo
- **Expected:** Hiển thị thông báo lỗi hoặc không thay đổi
- **Screenshot:** Chụp màn hình thông báo lỗi

**BT11: Volume = 101 (Invalid)**
- **Preconditions:** Đang phát nhạc
- **Steps:**
  1. Thử đặt volume 101 (nếu có thể)
  2. Kiểm tra thông báo
- **Expected:** Hiển thị thông báo lỗi hoặc không thay đổi
- **Screenshot:** Chụp màn hình thông báo lỗi

### B. Chức năng 2: PLAYLIST MANAGEMENT (10 test cases)

#### 1. Phân vùng tương đương

| Input Variable | Valid Partitions | Invalid Partitions | Test Cases |
|---------------|------------------|-------------------|------------|
| Playlist Size | 0, 1-50, 51-100 | >100 | BT12-BT18 |
| Track Index | 0-(n-1) | <0, ≥n | BT16-BT17 |
| File Type | mp3, wav, flac, m4a | txt, jpg, exe | BT19 |
| Operation | Add, Remove, Clear | Invalid operation | BT20 |

#### 2. Phân tích giá trị biên

| Variable | Min | Min+ | Nominal | Max- | Max | Invalid |
|----------|------|------|---------|------|-----|---------|
| Playlist Size | 0 | 1 | 25 | 99 | 100 | 101 |
| Track Index | 0 | 1 | middle | n-2 | n-1 | n, -1 |

#### 3. Bảng quyết định

| Test ID | Playlist Size | Track Index | Operation | Expected Result | File Type | Priority |
|---------|---------------|-------------|------------|----------------|-----------|----------|
| BT12 | 0 | N/A | Play | Error - Empty playlist | N/A | High |
| BT13 | 1 | 0 | Play | Play track 1 | mp3 | High |
| BT14 | 5 | 2 | Play | Play track 3 | mp3 | Medium |
| BT15 | 100 | 99 | Play | Play track 100 | mp3 | Medium |
| BT16 | 5 | -1 | Play | Error - Invalid index | mp3 | High |
| BT17 | 5 | 5 | Play | Error - Out of range | mp3 | High |
| BT18 | 101 | N/A | Add | Error - Playlist full | mp3 | Medium |
| BT19 | Any | N/A | Add | Error - Invalid format | txt | High |
| BT20 | 10 | 3 | Remove | Success - Track removed | mp3 | Medium |
| BT21 | 1 | 0 | Clear | Success - Empty playlist | mp3 | Medium |

#### 4. Test Cases chi tiết cho Playlist Management

**BT12: Empty playlist play**
- **Preconditions:** Playlist rỗng
- **Steps:**
  1. Kiểm tra playlist không có bài hát
  2. Click nút Play
- **Expected:** Hiển thị thông báo "Playlist rỗng"
- **Screenshot:** Chụp màn hình thông báo lỗi

**BT13: Single track playlist play**
- **Preconditions:** Playlist có 1 bài hát
- **Steps:**
  1. Thêm 1 file mp3 vào playlist
  2. Click nút Play
- **Expected:** Phát bài hát duy nhất
- **Screenshot:** Chụp màn hình phát 1 bài hát

**BT14: Multiple tracks playlist play**
- **Preconditions:** Playlist có 5 bài hát
- **Steps:**
  1. Thêm 5 file mp3 vào playlist
  2. Click bài hát thứ 3 (index 2)
  3. Click nút Play
- **Expected:** Phát bài hát thứ 3
- **Screenshot:** Chụp màn hình phát bài hát được chọn

**BT15: Large playlist play**
- **Preconditions:** Playlist có 100 bài hát
- **Steps:**
  1. Thêm 100 file mp3 vào playlist
  2. Click bài hát cuối cùng (index 99)
  3. Click nút Play
- **Expected:** Phát bài hát thứ 100
- **Screenshot:** Chụp màn hình phát bài hát cuối

**BT16: Invalid track index (negative)**
- **Preconditions:** Playlist có 5 bài hát
- **Steps:**
  1. Thêm 5 file mp3 vào playlist
  2. Thử chọn bài hát có index -1
  3. Click nút Play
- **Expected:** Hiển thị thông báo lỗi "Index không hợp lệ"
- **Screenshot:** Chụp màn hình thông báo lỗi

**BT17: Invalid track index (out of range)**
- **Preconditions:** Playlist có 5 bài hát
- **Steps:**
  1. Thêm 5 file mp3 vào playlist
  2. Thử chọn bài hát có index 5
  3. Click nút Play
- **Expected:** Hiển thị thông báo lỗi "Index ngoài phạm vi"
- **Screenshot:** Chụp màn hình thông báo lỗi

**BT18: Playlist full**
- **Preconditions:** Playlist có 100 bài hát
- **Steps:**
  1. Thêm 100 file mp3 vào playlist
  2. Thử thêm bài hát thứ 101
- **Expected:** Hiển thị thông báo "Playlist đã đầy"
- **Screenshot:** Chụp màn hình thông báo lỗi

**BT19: Invalid file format**
- **Preconditions:** Playlist có bài hát
- **Steps:**
  1. Thử thêm file .txt vào playlist
- **Expected:** Hiển thị thông báo "Định dạng không hỗ trợ"
- **Screenshot:** Chụp màn hình thông báo lỗi

**BT20: Remove track**
- **Preconditions:** Playlist có 10 bài hát
- **Steps:**
  1. Thêm 10 file mp3 vào playlist
  2. Chọn bài hát thứ 4 (index 3)
  3. Click nút Remove
- **Expected:** Bài hát được xóa, playlist còn 9 bài hát
- **Screenshot:** Chụp màn hình sau khi xóa

**BT21: Clear all tracks**
- **Preconditions:** Playlist có 1 bài hát
- **Steps:**
  1. Thêm 1 file mp3 vào playlist
  2. Click nút Clear All
- **Expected:** Playlist trở nên rỗng
- **Screenshot:** Chụp màn hình playlist rỗng

---

## III. CHƯƠNG 3: WHITE-BOX TESTING (18 TEST CASES)

### A. Chức năng 1: MEDIA LOADING (10 test cases)

#### 1. Phân tích mã nguồn (MainWindow.xaml.cs)

**Phương thức LoadMedia(string filePath):**
```csharp
private bool LoadMedia(string filePath)
{
    // Branch 1: Input validation
    if (string.IsNullOrEmpty(filePath))
        return false; // Branch 1.1
    
    if (!File.Exists(filePath))
        return false; // Branch 1.2
    
    // Branch 2: Format validation
    var extension = Path.GetExtension(filePath).ToLower();
    var supportedFormats = new[] { ".mp3", ".wav", ".flac", ".m4a" };
    
    if (!supportedFormats.Contains(extension))
        return false; // Branch 2.1
    
    // Branch 3: Loading process
    try
    {
        if (_currentMedia != null)
        {
            _currentMedia.Dispose(); // Branch 3.1.1
        }
        
        _currentMedia = new MediaElement(); // Branch 3.1.2
        _currentMedia.Source = new Uri(filePath);
        
        if (_currentMedia.NaturalDuration.HasTimeSpan)
        {
            _isLoaded = true; // Branch 3.2.1
            return true;
        }
        else
        {
            _isLoaded = false; // Branch 3.2.2
            return false;
        }
    }
    catch (Exception ex)
    {
        return false; // Exception branch
    }
}
```

#### 2. Thiết kế test case phủ nhánh

| Test ID | Test Description | Input | Path Covered | Expected | Priority |
|---------|------------------|-------|--------------|----------|----------|
| WT01 | Null file path | null | Branch 1.1 | false | High |
| WT02 | Empty file path | "" | Branch 1.1 | false | High |
| WT03 | Non-existent file | "nonexistent.mp3" | Branch 1.2 | false | High |
| WT04 | Valid MP3 file | "test.mp3" | Branch 3.2.1 | true | High |
| WT05 | Valid WAV file | "test.wav" | Branch 3.2.1 | true | Medium |
| WT06 | Valid FLAC file | "test.flac" | Branch 3.2.1 | true | Medium |
| WT07 | Valid M4A file | "test.m4a" | Branch 3.2.1 | true | Medium |
| WT08 | Invalid TXT format | "test.txt" | Branch 2.1 | false | High |
| WT09 | Invalid JPG format | "test.jpg" | Branch 2.1 | false | Medium |
| WT10 | Corrupted media file | "corrupted.mp3" | Exception branch | false | Medium |

#### 3. Test Cases chi tiết cho Media Loading

**WT01: Null file path**
- **Preconditions:** Ứng dụng mở
- **Steps:**
  1. Gọi LoadMedia(null)
- **Expected:** Return false, không có exception
- **Coverage:** Branch 1.1
- **Screenshot:** Chụp màn hình trạng thái ban đầu

**WT02: Empty file path**
- **Preconditions:** Ứng dụng mở
- **Steps:**
  1. Gọi LoadMedia("")
- **Expected:** Return false, không có exception
- **Coverage:** Branch 1.1
- **Screenshot:** Chụp màn hình trạng thái ban đầu

**WT03: Non-existent file**
- **Preconditions:** Ứng dụng mở
- **Steps:**
  1. Gọi LoadMedia("nonexistent.mp3")
- **Expected:** Return false, không có exception
- **Coverage:** Branch 1.2
- **Screenshot:** Chụp màn hình thông báo lỗi

**WT04: Valid MP3 file**
- **Preconditions:** Có file test.mp3 hợp lệ
- **Steps:**
  1. Tạo file test.mp3
  2. Gọi LoadMedia("test.mp3")
- **Expected:** Return true, media loaded successfully
- **Coverage:** Branch 3.2.1
- **Screenshot:** Chụp màn hình khi file loaded

**WT05: Valid WAV file**
- **Preconditions:** Có file test.wav hợp lệ
- **Steps:**
  1. Tạo file test.wav
  2. Gọi LoadMedia("test.wav")
- **Expected:** Return true, media loaded successfully
- **Coverage:** Branch 3.2.1
- **Screenshot:** Chụp màn hình khi file loaded

**WT06: Valid FLAC file**
- **Preconditions:** Có file test.flac hợp lệ
- **Steps:**
  1. Tạo file test.flac
  2. Gọi LoadMedia("test.flac")
- **Expected:** Return true, media loaded successfully
- **Coverage:** Branch 3.2.1
- **Screenshot:** Chụp màn hình khi file loaded

**WT07: Valid M4A file**
- **Preconditions:** Có file test.m4a hợp lệ
- **Steps:**
  1. Tạo file test.m4a
  2. Gọi LoadMedia("test.m4a")
- **Expected:** Return true, media loaded successfully
- **Coverage:** Branch 3.2.1
- **Screenshot:** Chụp màn hình khi file loaded

**WT08: Invalid TXT format**
- **Preconditions:** Có file test.txt
- **Steps:**
  1. Tạo file test.txt
  2. Gọi LoadMedia("test.txt")
- **Expected:** Return false, không có exception
- **Coverage:** Branch 2.1
- **Screenshot:** Chụp màn hình thông báo lỗi

**WT09: Invalid JPG format**
- **Preconditions:** Có file test.jpg
- **Steps:**
  1. Tạo file test.jpg
  2. Gọi LoadMedia("test.jpg")
- **Expected:** Return false, không có exception
- **Coverage:** Branch 2.1
- **Screenshot:** Chụp màn hình thông báo lỗi

**WT10: Corrupted media file**
- **Preconditions:** Có file corrupted.mp3
- **Steps:**
  1. Tạo file corrupted.mp3 (invalid header)
  2. Gọi LoadMedia("corrupted.mp3")
- **Expected:** Return false, không có exception
- **Coverage:** Exception branch
- **Screenshot:** Chụp màn hình thông báo lỗi

### B. Chức năng 2: VOLUME CONTROL (8 test cases)

#### 1. Phân tích mã nguồn

**Phương thức SetVolume(int volume):**
```csharp
public bool SetVolume(int volume)
{
    // Branch 1: Input validation
    if (volume < 0 || volume > 100)
        return false; // Branch 1.1
    
    // Branch 2: Mute state handling
    if (_isMuted)
    {
        if (volume > 0)
        {
            _isMuted = false; // Branch 2.1.1
            _mediaElement.Volume = volume / 100.0;
        }
        else
        {
            return true; // Branch 2.1.2 (keep muted)
        }
    }
    else
    {
        _mediaElement.Volume = volume / 100.0; // Branch 2.2
    }
    
    _currentVolume = volume;
    return true;
}
```

#### 2. Thiết kế test case phủ nhánh

| Test ID | Test Description | Input | Path Covered | Expected | Priority |
|---------|------------------|-------|--------------|----------|----------|
| WT11 | Volume below minimum | -1 | Branch 1.1 | false | High |
| WT12 | Volume above maximum | 101 | Branch 1.1 | false | High |
| WT13 | Valid volume 0 | 0 | Branch 2.2 | true | Medium |
| WT14 | Valid volume 50 | 50 | Branch 2.2 | true | Medium |
| WT15 | Valid volume 100 | 100 | Branch 2.2 | true | Medium |
| WT16 | Volume >0 when muted | 50 | Branch 2.1.1 | true | High |
| WT17 | Volume 0 when muted | 0 | Branch 2.1.2 | true | High |
| WT18 | Valid volume when not muted | 75 | Branch 2.2 | true | Medium |

#### 3. Test Cases chi tiết cho Volume Control

**WT11: Volume below minimum**
- **Preconditions:** Ứng dụng mở, có nhạc đang phát
- **Steps:**
  1. Gọi SetVolume(-1)
- **Expected:** Return false, volume không thay đổi
- **Coverage:** Branch 1.1
- **Screenshot:** Chụp màn hình volume hiện tại

**WT12: Volume above maximum**
- **Preconditions:** Ứng dụng mở, có nhạc đang phát
- **Steps:**
  1. Gọi SetVolume(101)
- **Expected:** Return false, volume không thay đổi
- **Coverage:** Branch 1.1
- **Screenshot:** Chụp màn hình volume hiện tại

**WT13: Valid volume 0**
- **Preconditions:** Ứng dụng mở, có nhạc đang phát
- **Steps:**
  1. Gọi SetVolume(0)
- **Expected:** Return true, volume = 0
- **Coverage:** Branch 2.2
- **Screenshot:** Chụp màn hình volume=0

**WT14: Valid volume 50**
- **Preconditions:** Ứng dụng mở, có nhạc đang phát
- **Steps:**
  1. Gọi SetVolume(50)
- **Expected:** Return true, volume = 50
- **Coverage:** Branch 2.2
- **Screenshot:** Chụp màn hình volume=50

**WT15: Valid volume 100**
- **Preconditions:** Ứng dụng mở, có nhạc đang phát
- **Steps:**
  1. Gọi SetVolume(100)
- **Expected:** Return true, volume = 100
- **Coverage:** Branch 2.2
- **Screenshot:** Chụp màn hình volume=100

**WT16: Volume >0 when muted**
- **Preconditions:** Ứng dụng mở, đang muted
- **Steps:**
  1. Mute volume
  2. Gọi SetVolume(50)
- **Expected:** Return true, unmuted, volume = 50
- **Coverage:** Branch 2.1.1
- **Screenshot:** Chụp màn hình unmuted

**WT17: Volume 0 when muted**
- **Preconditions:** Ứng dụng mở, đang muted
- **Steps:**
  1. Mute volume
  2. Gọi SetVolume(0)
- **Expected:** Return true, still muted
- **Coverage:** Branch 2.1.2
- **Screenshot:** Chụp màn hình still muted

**WT18: Valid volume when not muted**
- **Preconditions:** Ứng dụng mở, không muted
- **Steps:**
  1. Gọi SetVolume(75)
- **Expected:** Return true, volume = 75
- **Coverage:** Branch 2.2
- **Screenshot:** Chụp màn hình volume=75

---

## IV. TỔNG QUAN TEST CASES

### A. Summary Table

| Loại test | Chức năng | Số test cases | ID range | Priority |
|-----------|-----------|---------------|----------|----------|
| Black-box | Playback Control | 11 | BT01-BT11 | 8 High, 3 Medium |
| Black-box | Playlist Management | 10 | BT12-BT21 | 6 High, 4 Medium |
| White-box | Media Loading | 10 | WT01-WT10 | 6 High, 4 Medium |
| White-box | Volume Control | 8 | WT11-WT18 | 4 High, 4 Medium |
| **Tổng cộng** | **4 chức năng** | **39** | **BT01-BT21, WT01-WT18** | **24 High, 15 Medium** |

### B. Coverage Targets

| Chức năng | Statement Coverage | Branch Coverage | Condition Coverage |
|-----------|-------------------|-----------------|-------------------|
| Media Loading | 95% | 100% | 100% |
| Volume Control | 100% | 100% | 100% |
| Playback Control | 90% | 85% | 80% |
| Playlist Management | 85% | 80% | 75% |

### C. Execution Order

#### Priority 1: High Priority (24 test cases)
1. **Media Loading (WT01-WT04, WT08)** - Core functionality
2. **Playback Control (BT01-BT07, BT10-BT11)** - User interaction
3. **Playlist Management (BT12-BT17, BT19)** - Data management
4. **Volume Control (WT11-WT12, WT16-WT17)** - Critical controls

#### Priority 2: Medium Priority (15 test cases)
1. **Media Loading (WT05-WT07, WT09-WT10)** - Additional formats
2. **Playback Control (BT08-BT09)** - Edge cases
3. **Playlist Management (BT18, BT20-BT21)** - Advanced features
4. **Volume Control (WT13-WT15, WT18)** - Normal operations

---

## V. CHECKLIST PREPARATION

### A. Test Data Preparation

#### 1. Media Files cần chuẩn bị:
```
TestFiles/
├── valid/
│   ├── test.mp3 (1MB, valid MP3)
│   ├── test.wav (1MB, valid WAV)
│   ├── test.flac (1MB, valid FLAC)
│   ├── test.m4a (1MB, valid M4A)
│   ├── large.mp3 (50MB, large file)
│   └── short.mp3 (10KB, short file)
├── invalid/
│   ├── test.txt (text file)
│   ├── test.jpg (image file)
│   ├── test.exe (executable)
│   └── corrupted.mp3 (invalid header)
└── empty/
    └── (no files)
```

#### 2. Test Scenarios cần chuẩn bị:
```
TestScenarios/
├── empty_playlist/ (playlist rỗng)
├── single_track/ (1 bài hát)
├── multiple_tracks/ (5 bài hát)
├── large_playlist/ (100 bài hát)
└── mixed_formats/ (đủ các định dạng)
```

### B. Environment Setup

#### 1. System Requirements:
- Windows 10/11
- .NET 8.0 Runtime
- Visual Studio 2022 (for debugging)
- WinAppDriver (for automation)

#### 2. Test Environment:
- Clean installation of Music Player
- Test files in predictable location
- Screenshots folder: `TestResults/Screenshots/`
- Logs folder: `TestResults/Logs/`

### C. Documentation Templates

#### 1. Test Execution Log:
```
Test Execution Log - [Date]
Environment: [OS Version]
Music Player Version: [Version]
Test Data Location: [Path]

Test Results:
BT01: ✅ Pass - Screenshot_BT01_Playing.png
BT02: ✅ Pass - Screenshot_BT02_Playing.png
...
WT10: ❌ Fail - Screenshot_WT10_Error.png - Error: [Message]

Summary: 39 tests, 37 passed, 2 failed
```

#### 2. Screenshot Naming Convention:
```
Screenshots/
├── BlackBox/
│   ├── BT01_Playing_20240322_143022.png
│   ├── BT02_Playing_20240322_143025.png
│   └── ...
├── WhiteBox/
│   ├── WT01_NullPath_20240322_143100.png
│   ├── WT04_ValidMP3_20240322_143105.png
│   └── ...
└── Errors/
    ├── WT10_CorruptedFile_20240322_143200.png
    └── ...
```

---

## VI. NEXT STEPS

### A. Immediate Actions (Hôm nay)
1. ✅ **Xem lại và xác nhận test cases** với ứng dụng thực tế
2. ✅ **Chuẩn bị test data** (media files)
3. ✅ **Setup environment** cho testing

### B. Short Term (2-3 ngày tới)
1. 📋 **Manual testing** cho 24 high priority test cases
2. 📸 **Take screenshots** cho mỗi test case
3. 📝 **Document results** trong execution log

### C. Medium Term (1 tuần tới)
1. 🤖 **Setup Gauge** cho automation
2. 🔧 **Setup NUnit** cho unit testing
3. 📊 **Generate reports** và fill báo cáo

### D. Long Term (2 tuần tới)
1. 🎬 **Demo preparation** cho presentation
2. 📄 **Final report** completion
3. 🎯 **Project submission**

---

**KẾT LUẬN:** 
- **39 test cases** covering **4 core functions**
- **24 high priority** test cases cho immediate execution
- **Clear step-by-step** instructions cho mỗi test
- **Realistic scenarios** based on actual application

**Bắt đầu ngay với manual testing để xác nhận test cases phù hợp với ứng dụng hiện tại!** 🚀
