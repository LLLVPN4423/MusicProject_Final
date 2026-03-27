using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Win32;
using MusicApp.Models;
using MusicApp.Services;
using TagLib;
using TagLibFile = TagLib.File;

namespace MusicApp;

public partial class MainWindow : Window
{
    private readonly MediaPlayer _mediaPlayer = new();
    private readonly DispatcherTimer? _positionTimer;
    private bool _isDraggingProgress;
    private bool _isPlaying;
    private bool _isMediaLoaded;
    private int _currentTrackIndex = -1;
    private bool _isShuffle;
    private bool _isRepeat;
    private bool _isMuted;
    private double _previousVolume = 0.5;
    private bool _enableShortcuts = true;

    public ObservableCollection<TrackInfo> Playlist { get; } = new();
    public ObservableCollection<TrackInfo> Favorites { get; } = new();
    public ObservableCollection<TrackInfo> History { get; } = new();
    public ObservableCollection<TrackInfo> SearchResults { get; private set; } = new();

    private string _currentSearchTerm = "";

    public MainWindow()
    {
        try
        {
            InitializeComponent();
            DataContext = this;

            KeyDown += MainWindowOnKeyDown;
            Focusable = true;

            // Apply initial theme
            ApplyCurrentTheme();

            // Subscribe to theme changes
            ThemeManager.ThemeChanged += OnThemeChanged;

            // Load Favorites and History
            var loadedFavorites = DataManager.LoadFavorites();
            foreach (var fav in loadedFavorites)
                Favorites.Add(fav);

            var loadedHistory = DataManager.LoadHistory();
            foreach (var hist in loadedHistory)
                History.Add(hist);

            _mediaPlayer.MediaOpened += MediaPlayerOnMediaOpened;
            _mediaPlayer.MediaEnded += MediaPlayerOnMediaEnded;

            _positionTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(500)
            };
            if (_positionTimer != null)
                _positionTimer.Tick += PositionTimerOnTick;

            // Initialize button states
            _isPlaying = false;
            _isMediaLoaded = false;
            btnPause.Content = "▶️"; // Icon play ban đầu
            lblStatus.Text = "Sẵn sàng";
            lblMessage.Text = "";

            // Apply system settings (volume + shortcuts)
            var systemSettings = SystemSettingsManager.Load();
            ApplySystemSettings(systemSettings);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"MainWindow initialization error: {ex.Message}");
            System.Windows.MessageBox.Show($"Lỗi khởi tạo ứng dụng: {ex.Message}", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }

    public void ApplySystemSettings(SystemSettings settings)
    {
        if (settings == null) return;

        _enableShortcuts = settings.EnableShortcuts;

        // Volume
        var volume = Math.Max(0, Math.Min(1, settings.DefaultVolume));
        _previousVolume = volume;

        if (_mediaPlayer != null)
            _mediaPlayer.Volume = volume;

        if (sldVolume != null)
            sldVolume.Value = volume;

        // Update mute icon/button based on volume
        if (volume <= 0)
        {
            _isMuted = true;
            if (btnMute != null) btnMute.Content = "🔇";
        }
        else
        {
            _isMuted = false;
            if (btnMute != null) btnMute.Content = volume < 0.5 ? "🔉" : "🔊";
        }
    }

    private void MainWindowOnKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
    {
        if (!_enableShortcuts)
            return;

        // Ignore shortcuts while typing in text fields.
        if (Keyboard.FocusedElement is System.Windows.Controls.TextBox ||
            Keyboard.FocusedElement is System.Windows.Controls.PasswordBox)
            return;

        if (e.Key == Key.Space)
        {
            e.Handled = true;
            BtnPause_Click(this, new RoutedEventArgs());
            return;
        }

        if (e.Key == Key.Left)
        {
            e.Handled = true;
            BtnPrev_Click(this, new RoutedEventArgs());
            return;
        }

        if (e.Key == Key.Right)
        {
            e.Handled = true;
            BtnNext_Click(this, new RoutedEventArgs());
            return;
        }

        if (e.Key == Key.M)
        {
            e.Handled = true;
            BtnMute_Click(this, new RoutedEventArgs());
        }
    }

    private void OnThemeChanged(Theme newTheme)
    {
        ApplyCurrentTheme();
    }

    private void ApplyCurrentTheme()
    {
        var theme = ThemeManager.CurrentTheme;
        
        // Update main background
        Background = new SolidColorBrush(ThemeManager.GetColor("Background"));
        
        // Update sidebar background
        if (FindName("sidebarBorder") is Border sidebarBorder)
            sidebarBorder.Background = new SolidColorBrush(ThemeManager.GetColor("Sidebar"));
        
        // Update main surface
        if (FindName("mainSurface") is Grid mainSurface)
            mainSurface.Background = new SolidColorBrush(ThemeManager.GetColor("Surface"));
        
        // Update playlist panel
        if (FindName("playlistPanel") is Border playlistPanel)
            playlistPanel.Background = new SolidColorBrush(ThemeManager.GetColor("Card"));
        
        // Update text colors
        UpdateTextColors();
    }

    private void UpdateTextColors()
    {
        var primaryColor = new SolidColorBrush(ThemeManager.GetColor("TextPrimary"));
        var secondaryColor = new SolidColorBrush(ThemeManager.GetColor("TextSecondary"));

        // Update logo text
        if (FindName("logoText") is TextBlock logoText)
            logoText.Foreground = primaryColor;

        // Update track info
        if (FindName("lblTrackTitle") is TextBlock lblTrackTitle)
            lblTrackTitle.Foreground = primaryColor;
        
        if (FindName("lblArtist") is TextBlock lblArtist)
            lblArtist.Foreground = primaryColor;
        
        if (FindName("lblStatus") is TextBlock lblStatus)
            lblStatus.Foreground = primaryColor;

        // Update labels
        UpdateLabel("lblTrackTitleLabel", secondaryColor);
        UpdateLabel("lblArtistLabel", secondaryColor);
        UpdateLabel("lblStatusLabel", secondaryColor);
    }

    private void UpdateLabel(string name, SolidColorBrush color)
    {
        if (FindName(name) is TextBlock label)
            label.Foreground = color;
    }

    private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            System.Diagnostics.Debug.WriteLine("=== BtnOpenFile_Click START ===");
            
            var dialog = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Audio/Video Files|*.mp3;*.wav;*.wma;*.aac;*.flac;*.ogg;*.m4a;*.mp4|All Files|*.*",
                Multiselect = true,
                Title = "Chọn bài hát"
            };

            System.Diagnostics.Debug.WriteLine($"Dialog created: {dialog.Title}");

            if (dialog.ShowDialog() != true)
            {
                System.Diagnostics.Debug.WriteLine("User cancelled file dialog");
                SetMessage("Không có bài hát nào được thêm");
                return;
            }

            System.Diagnostics.Debug.WriteLine($"User selected {dialog.FileNames.Length} files");

            foreach (var filePath in dialog.FileNames)
            {
                try
                {
                    System.Diagnostics.Debug.WriteLine($"Processing file: {filePath}");
                    
                    using var tagFile = TagLibFile.Create(filePath);
                    var duration = tagFile.Properties.Duration;
                    var artworkData = ExtractArtwork(tagFile.Tag.Pictures);

                    var isVideo = IsVideoFile(filePath);
                    var track = new TrackInfo
                    {
                        Title = isVideo
                            ? Path.GetFileNameWithoutExtension(filePath)
                            : (string.IsNullOrWhiteSpace(tagFile.Tag.Title)
                                ? Path.GetFileNameWithoutExtension(filePath)
                                : tagFile.Tag.Title),
                        Artist = tagFile.Tag.Performers.Length > 0
                            ? string.Join(", ", tagFile.Tag.Performers)
                            : "Chưa rõ ca sĩ",
                        Duration = duration,
                        DurationText = duration == TimeSpan.Zero ? "--:--" : FormatTimeSpan(duration),
                        FilePath = filePath,
                        ArtworkData = artworkData,
                        IsVideo = isVideo
                    };

                    System.Diagnostics.Debug.WriteLine($"Created track: {track.Title} by {track.Artist}");
                    Playlist.Add(track);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"ERROR loading file {filePath}: {ex.Message}");
                    System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
                    // Continue with other files instead of showing error for each file
                }
            }

            System.Diagnostics.Debug.WriteLine($"Total tracks in playlist: {Playlist.Count}");
            RefreshSequenceNumbers();

            if (Playlist.Count == 0)
            {
                System.Diagnostics.Debug.WriteLine("No valid tracks loaded");
                SetMessage("Chưa có bài hát hợp lệ");
                return;
            }

            if (_currentTrackIndex == -1)
            {
                System.Diagnostics.Debug.WriteLine("Auto-playing first track");
                PlayTrack(0);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine($"Not auto-playing, current index: {_currentTrackIndex}");
                SetMessage($"Đã thêm {dialog.FileNames.Length} bài hát");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"FATAL ERROR in BtnOpenFile_Click: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Stack trace: {ex.StackTrace}");
            System.Windows.MessageBox.Show(
                $"Lỗi mở file: {ex.Message}",
                "Lỗi",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            SetMessage("Lỗi mở file");
        }
    }

    private void BtnOpenFolder_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new System.Windows.Forms.FolderBrowserDialog();
        dialog.Description = "Chọn thư mục chứa nhạc";
        
        if (dialog.ShowDialog() != System.Windows.Forms.DialogResult.OK)
        {
            SetMessage("Không có thư mục nào được chọn");
            return;
        }

        var audioFiles = Directory.GetFiles(dialog.SelectedPath, "*.*", SearchOption.AllDirectories)
            .Where(file => IsAudioFile(file));

        int addedCount = 0;
        foreach (var filePath in audioFiles)
        {
            try
            {
                using var tagFile = TagLibFile.Create(filePath);
                var duration = tagFile.Properties.Duration;
                var artworkData = ExtractArtwork(tagFile.Tag.Pictures);

                var isVideo = IsVideoFile(filePath);
                var track = new TrackInfo
                {
                    Title = isVideo
                        ? Path.GetFileNameWithoutExtension(filePath)
                        : (string.IsNullOrWhiteSpace(tagFile.Tag.Title)
                            ? Path.GetFileNameWithoutExtension(filePath)
                            : tagFile.Tag.Title),
                    Artist = tagFile.Tag.Performers.Length > 0
                        ? string.Join(", ", tagFile.Tag.Performers)
                        : "Chưa rõ ca sĩ",
                    Duration = duration,
                    DurationText = duration == TimeSpan.Zero ? "--:--" : FormatTimeSpan(duration),
                    FilePath = filePath,
                    ArtworkData = artworkData,
                    IsVideo = isVideo
                };

                Playlist.Add(track);
                addedCount++;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show(
                    $"Không thể đọc file \"{Path.GetFileName(filePath)}\".\nChi tiết: {ex.Message}",
                    "Lỗi đọc file",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        RefreshSequenceNumbers();

        if (addedCount == 0)
        {
            SetMessage("Không có file audio hợp lệ trong thư mục");
            return;
        }

        if (_currentTrackIndex == -1)
        {
            PlayTrack(0);
        }
        else
        {
            SetMessage($"Đã thêm {addedCount} bài hát từ thư mục");
        }
    }

    private void MenuButton_Click(object sender, RoutedEventArgs e)
    {
        menuItemsPanel.Visibility =
            menuItemsPanel.Visibility == Visibility.Visible
            ? Visibility.Collapsed
            : Visibility.Visible;
    }
    
    private void BtnPrev_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (Playlist?.Count > 0)
            {
                int nextIndex;
                if (_isShuffle)
                {
                    var random = new Random();
                    nextIndex = random.Next(0, Playlist.Count);
                }
                else
                {
                    nextIndex = (_currentTrackIndex - 1 + Playlist.Count) % Playlist.Count;
                }
                PlayTrack(nextIndex);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Prev button error: {ex.Message}");
            SetMessage("Lỗi chuyển bài");
        }
    }

    private void BtnNext_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (Playlist?.Count > 0)
            {
                int nextIndex;
                if (_isShuffle)
                {
                    var random = new Random();
                    nextIndex = random.Next(0, Playlist.Count);
                }
                else
                {
                    nextIndex = (_currentTrackIndex + 1) % Playlist.Count;
                }
                PlayTrack(nextIndex);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Next button error: {ex.Message}");
            SetMessage("Lỗi chuyển bài");
        }
    }

    private void BtnPause_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            if (Playlist?.Count > 0)
            {
                if (_isPlaying)
                {
                    // Đang phát -> Dừng
                    _mediaPlayer?.Pause();
                    _isPlaying = false;
                    btnPause.Content = "▶️"; // Icon play
                    lblStatus.Text = "Đã tạm dừng";
                }
                else
                {
                    // Đang dừng -> Phát
                    if (_currentTrackIndex >= 0 && _currentTrackIndex < Playlist.Count)
                    {
                        // If MediaPlayer has no media opened yet (or got reset), re-open via PlayTrack.
                        if (!_isMediaLoaded)
                        {
                            PlayTrack(_currentTrackIndex);
                        }
                        else
                        {
                            _mediaPlayer?.Play();
                            _positionTimer?.Start();
                            _isPlaying = true;
                            btnPause.Content = "⏸️"; // Icon pause
                            lblStatus.Text = "Đang phát";
                        }
                    }
                    else
                    {
                        SetMessage("Không có bài hát để phát");
                    }
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Pause button error: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"Pause button stack: {ex.StackTrace}");
            _isPlaying = false;
            btnPause.Content = "▶️";
            SetMessage("Lỗi phát/dừng");
        }
    }

    private void LvPlaylist_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (lvPlaylist.SelectedIndex >= 0)
        {
            PlayTrack(lvPlaylist.SelectedIndex);
        }
    }

    private void Progress_DragStarted(object sender, DragStartedEventArgs e)
    {
        _isDraggingProgress = true;
    }

    private void Progress_DragCompleted(object sender, DragCompletedEventArgs e)
    {
        _isDraggingProgress = false;
        SeekToSlider();
    }

    private void Progress_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
    {
        if (!_isDraggingProgress)
        {
            return;
        }

        _isDraggingProgress = false;
        SeekToSlider();
    }

    private void PlayTrack(int index)
    {
        try
        {
            if (index < 0 || index >= Playlist.Count)
            {
                System.Diagnostics.Debug.WriteLine($"Invalid track index: {index}");
                return;
            }

            var track = Playlist[index];
            _currentTrackIndex = index;

            // Add to history
            track.LastPlayed = DateTime.Now;
            AddToHistory(track);

            try
            {
                _mediaPlayer?.Open(new Uri(track.FilePath));
                _mediaPlayer?.Play();
                _positionTimer?.Start();
                _isPlaying = true;
                _isMediaLoaded = true;
                btnPause.Content = "⏸️"; // Icon pause khi đang phát

                lvPlaylist.SelectedIndex = index;
                lvPlaylist.ScrollIntoView(track);

                lblTrackTitle.Text = string.IsNullOrWhiteSpace(track.Title) ? "Chưa rõ tên bài hát" : track.Title;
                lblArtist.Text = string.IsNullOrWhiteSpace(track.Artist) ? "Chưa rõ ca sĩ" : track.Artist;
                lblStatus.Text = "Đang phát";
                sldProgress.Value = 0;

                DisplayTrackVisuals(track);
                SyncFavoriteButton(track);

                var durationText = track.Duration == TimeSpan.Zero
                    ? "00:00"
                    : FormatTimeSpan(track.Duration);
                lblTimeDisplay.Text = $"00:00 / {durationText}";
            }
            catch (Exception mediaEx)
            {
                System.Diagnostics.Debug.WriteLine($"Media error: {mediaEx.Message}");
                System.Diagnostics.Debug.WriteLine($"Media stack: {mediaEx.StackTrace}");
                _isPlaying = false;
                _isMediaLoaded = false;
                btnPause.Content = "▶️";
                System.Windows.MessageBox.Show(
                    $"Không thể phát bài hát.\nChi tiết: {mediaEx.Message}",
                    "Lỗi phát nhạc",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
                SetMessage("Lỗi phát nhạc");
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"PlayTrack error: {ex.Message}");
            System.Diagnostics.Debug.WriteLine($"PlayTrack stack: {ex.StackTrace}");
            _isPlaying = false;
            _isMediaLoaded = false;
            btnPause.Content = "▶️";
            SetMessage("Lỗi phát nhạc");
        }
    }

    private void MediaPlayerOnMediaOpened(object? sender, EventArgs e)
    {
        if (!_mediaPlayer.NaturalDuration.HasTimeSpan)
        {
            return;
        }

        _isMediaLoaded = true;

        var total = _mediaPlayer.NaturalDuration.TimeSpan;
        sldProgress.Maximum = total.TotalSeconds;
        UpdateTimeDisplay(TimeSpan.Zero, total);
    }

    private void MediaPlayerOnMediaEnded(object? sender, EventArgs e)
    {
        try
        {
            if (Playlist?.Count > 0)
            {
                if (_isRepeat)
                {
                    PlayTrack(_currentTrackIndex);
                }
                else
                {
                    BtnNext_Click(this, new RoutedEventArgs());
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"MediaEnded error: {ex.Message}");
            SetMessage("Lỗi khi kết thúc bài hát");
        }
    }

    private void PositionTimerOnTick(object? sender, EventArgs e)
    {
        if (!_mediaPlayer.NaturalDuration.HasTimeSpan || _isDraggingProgress)
        {
            return;
        }

        var duration = _mediaPlayer.NaturalDuration.TimeSpan;
        var position = _mediaPlayer.Position;

        sldProgress.Maximum = duration.TotalSeconds;
        sldProgress.Value = Math.Min(position.TotalSeconds, duration.TotalSeconds);
        UpdateTimeDisplay(position, duration);
    }

    private void SeekToSlider()
    {
        if (!_mediaPlayer.NaturalDuration.HasTimeSpan)
        {
            return;
        }

        var duration = _mediaPlayer.NaturalDuration.TimeSpan;
        var seconds = Math.Max(0, Math.Min(sldProgress.Value, duration.TotalSeconds));
        _mediaPlayer.Position = TimeSpan.FromSeconds(seconds);
        UpdateTimeDisplay(_mediaPlayer.Position, duration);
    }

    private void RefreshSequenceNumbers()
    {
        for (var i = 0; i < Playlist.Count; i++)
        {
            Playlist[i].Sequence = i + 1;
        }
    }

    private static string FormatTimeSpan(TimeSpan value)
    {
        if (value <= TimeSpan.Zero || double.IsNaN(value.TotalSeconds))
        {
            return "00:00";
        }

        return $"{(int)value.TotalMinutes:D2}:{value.Seconds:D2}";
    }

    private void UpdateTimeDisplay(TimeSpan position, TimeSpan duration)
    {
        lblTimeDisplay.Text = $"{FormatTimeSpan(position)} / {FormatTimeSpan(duration)}";
    }

    private static bool IsVideoFile(string path) =>
        string.Equals(Path.GetExtension(path), ".mp4", StringComparison.OrdinalIgnoreCase);

    private static byte[]? ExtractArtwork(IPicture[] pictures)
    {
        if (pictures == null || pictures.Length == 0)
        {
            return null;
        }

        return pictures[0].Data?.Data;
    }

    private void DisplayTrackVisuals(TrackInfo track)
    {
        videoPreview.Stop();
        videoPreview.Source = null;
        videoPreview.Visibility = Visibility.Collapsed;

        imgPreview.Source = null;
        imgPreview.Visibility = Visibility.Collapsed;

        lblImagePlaceholder.Visibility = Visibility.Visible;

        if (track.IsVideo && System.IO.File.Exists(track.FilePath))
        {
            videoPreview.Source = new Uri(track.FilePath);
            videoPreview.Visibility = Visibility.Visible;
            videoPreview.Position = TimeSpan.Zero;
            videoPreview.Play();

            lblImagePlaceholder.Visibility = Visibility.Collapsed;
            return;
        }

        if (track.ArtworkData is { Length: > 0 })
        {
            try
            {
                using var ms = new MemoryStream(track.ArtworkData);
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = ms;
                bitmap.EndInit();
                bitmap.Freeze();

                imgPreview.Source = bitmap;
                imgPreview.Visibility = Visibility.Visible;
                lblImagePlaceholder.Visibility = Visibility.Collapsed;
            }
            catch
            {
                // ignore invalid artwork
            }
        }
    }

    private void BtnShuffle_Click(object sender, RoutedEventArgs e)
    {
        _isShuffle = !_isShuffle;
        btnShuffle.Background = _isShuffle ? new SolidColorBrush(System.Windows.Media.Color.FromRgb(192, 124, 199)) : new SolidColorBrush(System.Windows.Media.Color.FromRgb(246, 245, 242));
        SetMessage(_isShuffle ? "Bật phát ngẫu nhiên" : "Tắt phát ngẫu nhiên");
    }

    private void BtnRepeat_Click(object sender, RoutedEventArgs e)
    {
        _isRepeat = !_isRepeat;
        btnRepeat.Background = _isRepeat ? new SolidColorBrush(System.Windows.Media.Color.FromRgb(192, 124, 199)) : new SolidColorBrush(System.Windows.Media.Color.FromRgb(246, 245, 242));
        SetMessage(_isRepeat ? "Bật lặp lại" : "Tắt lặp lại");
    }

    private void BtnMute_Click(object sender, RoutedEventArgs e)
    {
        try
        {
            _isMuted = !_isMuted;
            
            if (_isMuted)
            {
                _previousVolume = _mediaPlayer?.Volume ?? 0.5;
                if (_mediaPlayer != null)
                    _mediaPlayer.Volume = 0;
                if (sldVolume != null)
                    sldVolume.Value = 0;
                btnMute.Content = "🔇";
            }
            else
            {
                if (_mediaPlayer != null)
                    _mediaPlayer.Volume = _previousVolume;
                if (sldVolume != null)
                    sldVolume.Value = _previousVolume;
                btnMute.Content = "🔊";
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Mute button error: {ex.Message}");
            SetMessage("Lỗi tắt/mở tiếng");
        }
    }

    private void SldVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        try
        {
            if (_mediaPlayer != null)
            {
                _mediaPlayer.Volume = e.NewValue;
                _previousVolume = e.NewValue;
                
                // Update mute button icon based on volume
                if (e.NewValue == 0)
                {
                    btnMute.Content = "🔇";
                }
                else if (e.NewValue < 0.5)
                {
                    btnMute.Content = "🔉";
                }
                else
                {
                    btnMute.Content = "🔊";
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Volume change error: {ex.Message}");
            SetMessage("Lỗi điều chỉnh âm lượng");
        }
    }

    private void BtnPlaylist_Click(object sender, RoutedEventArgs e)
    {
        ShowPlaylistView();
    }

    private void BtnFavorites_Click(object sender, RoutedEventArgs e)
    {
        ShowFavoritesView();
    }

    private void BtnHistory_Click(object sender, RoutedEventArgs e)
    {
        ShowHistoryView();
    }

    private void BtnPlaylistTab_Click(object sender, RoutedEventArgs e)
    {
        ShowPlaylistView();
    }

    private void BtnFavoritesTab_Click(object sender, RoutedEventArgs e)
    {
        ShowFavoritesView();
    }

    private void BtnHistoryTab_Click(object sender, RoutedEventArgs e)
    {
        ShowHistoryView();
    }

    private void ShowPlaylistView()
    {
        playlistView.Visibility = Visibility.Visible;
        favoritesView.Visibility = Visibility.Collapsed;
        historyView.Visibility = Visibility.Collapsed;

        btnPlaylistTab.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(192, 124, 199));
        btnFavoritesTab.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(141, 136, 134));
        btnHistoryTab.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(141, 136, 134));

        UpdateCurrentView();
        SetMessage(string.IsNullOrWhiteSpace(_currentSearchTerm)
            ? "Danh sách phát"
            : $"Tìm kiếm: {_currentSearchTerm} ({SearchResults.Count} kết quả)");
    }

    private void ShowFavoritesView()
    {
        playlistView.Visibility = Visibility.Collapsed;
        favoritesView.Visibility = Visibility.Visible;
        historyView.Visibility = Visibility.Collapsed;

        btnPlaylistTab.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(141, 136, 134));
        btnFavoritesTab.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(192, 124, 199));
        btnHistoryTab.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(141, 136, 134));

        UpdateCurrentView();
        RefreshFavoritesSequence();
        SetMessage(string.IsNullOrWhiteSpace(_currentSearchTerm)
            ? "Danh sách yêu thích"
            : $"Tìm kiếm: {_currentSearchTerm} ({SearchResults.Count} kết quả)");
    }

    private void ShowHistoryView()
    {
        playlistView.Visibility = Visibility.Collapsed;
        favoritesView.Visibility = Visibility.Collapsed;
        historyView.Visibility = Visibility.Visible;

        btnPlaylistTab.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(141, 136, 134));
        btnFavoritesTab.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(141, 136, 134));
        btnHistoryTab.Background = new SolidColorBrush(System.Windows.Media.Color.FromRgb(192, 124, 199));

        UpdateCurrentView();
        RefreshHistorySequence();
        SetMessage(string.IsNullOrWhiteSpace(_currentSearchTerm)
            ? "Lịch sử nghe nhạc"
            : $"Tìm kiếm: {_currentSearchTerm} ({SearchResults.Count} kết quả)");
    }

    private void RefreshFavoritesSequence()
    {
        for (var i = 0; i < Favorites.Count; i++)
        {
            Favorites[i].Sequence = i + 1;
        }
    }

    private void RefreshHistorySequence()
    {
        for (var i = 0; i < History.Count; i++)
        {
            History[i].Sequence = i + 1;
        }
    }

    private void AddToHistory(TrackInfo track)
    {
        // Remove existing entry if present
        var existing = History.FirstOrDefault(h => h.FilePath == track.FilePath);
        if (existing != null)
        {
            History.Remove(existing);
        }

        // Add to beginning
        var historyTrack = new TrackInfo
        {
            Title = string.IsNullOrWhiteSpace(track.Title) ? "Chưa rõ tên bài hát" : track.Title,
            Artist = string.IsNullOrWhiteSpace(track.Artist) ? "Chưa rõ ca sĩ" : track.Artist,
            FilePath = track.FilePath,
            Duration = track.Duration,
            DurationText = track.DurationText,
            IsVideo = track.IsVideo,
            ArtworkData = track.ArtworkData,
            LastPlayed = DateTime.Now
        };

        History.Insert(0, historyTrack);

        // Keep only last 100 items
        while (History.Count > 100)
        {
            History.RemoveAt(History.Count - 1);
        }

        DataManager.SaveHistory(History);
    }

    private void LvFavorites_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (lvFavorites.SelectedIndex >= 0)
        {
            var track = Favorites[lvFavorites.SelectedIndex];
            
            // Add to current playlist if not exists
            if (!Playlist.Any(p => p.FilePath == track.FilePath))
            {
                var newTrack = new TrackInfo
                {
                    Title = track.Title,
                    Artist = track.Artist,
                    FilePath = track.FilePath,
                    Duration = track.Duration,
                    DurationText = track.DurationText,
                    IsVideo = track.IsVideo,
                    ArtworkData = track.ArtworkData,
                    IsFavorite = true
                };
                Playlist.Add(newTrack);
                RefreshSequenceNumbers();
            }

            var index = Playlist.ToList().FindIndex(p => p.FilePath == track.FilePath);
            if (index >= 0)
            {
                PlayTrack(index);
            }
        }
    }

    private void LvHistory_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (lvHistory.SelectedIndex >= 0)
        {
            var track = History[lvHistory.SelectedIndex];
            
            // Add to current playlist if not exists
            if (!Playlist.Any(p => p.FilePath == track.FilePath))
            {
                var newTrack = new TrackInfo
                {
                    Title = track.Title,
                    Artist = track.Artist,
                    FilePath = track.FilePath,
                    Duration = track.Duration,
                    DurationText = track.DurationText,
                    IsVideo = track.IsVideo,
                    ArtworkData = track.ArtworkData,
                    IsFavorite = Favorites.Any(f => f.FilePath == track.FilePath)
                };
                Playlist.Add(newTrack);
                RefreshSequenceNumbers();
            }

            var index = Playlist.ToList().FindIndex(p => p.FilePath == track.FilePath);
            if (index >= 0)
            {
                PlayTrack(index);
            }
        }
    }

    private void TxtSearch_TextChanged(object sender, TextChangedEventArgs e)
    {
        try
        {
            _currentSearchTerm = txtSearch?.Text ?? "";
            UpdateCurrentView();
            
            SetMessage(string.IsNullOrWhiteSpace(_currentSearchTerm)
                ? (playlistView.Visibility == Visibility.Visible
                    ? "Danh sách phát"
                    : favoritesView.Visibility == Visibility.Visible
                        ? "Danh sách yêu thích"
                        : "Lịch sử nghe nhạc")
                : $"Tìm kiếm: {_currentSearchTerm} ({SearchResults?.Count ?? 0} kết quả)");
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Search error: {ex.Message}");
            SetMessage("Lỗi tìm kiếm");
        }
    }

    private void BtnClearSearch_Click(object sender, RoutedEventArgs e)
    {
        txtSearch.Text = "";
        _currentSearchTerm = "";
        UpdateCurrentView();
        SetMessage("Đã xóa tìm kiếm");
    }

    private void UpdateCurrentView()
    {
        try
        {
            if (playlistView.Visibility == Visibility.Visible)
            {
                var source = string.IsNullOrWhiteSpace(_currentSearchTerm) 
                    ? Playlist 
                    : SearchService.SearchTracks(Playlist, _currentSearchTerm);
                
                SearchResults.Clear();
                foreach (var item in source)
                {
                    SearchResults.Add(item);
                }
            }
            else if (favoritesView.Visibility == Visibility.Visible)
            {
                var source = string.IsNullOrWhiteSpace(_currentSearchTerm) 
                    ? Favorites 
                    : SearchService.SearchTracks(Favorites, _currentSearchTerm);
                
                SearchResults.Clear();
                foreach (var item in source)
                {
                    SearchResults.Add(item);
                }
            }
            else if (historyView.Visibility == Visibility.Visible)
            {
                var source = string.IsNullOrWhiteSpace(_currentSearchTerm) 
                    ? History 
                    : SearchService.SearchTracks(History, _currentSearchTerm);
                
                SearchResults.Clear();
                foreach (var item in source)
                {
                    SearchResults.Add(item);
                }
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"UpdateCurrentView error: {ex.Message}");
            SetMessage("Lỗi cập nhật danh sách");
        }
    }

    private void BtnTheme_Click(object sender, RoutedEventArgs e)
    {
        ThemeManager.SwitchTheme();
        SetMessage(ThemeManager.CurrentTheme == Theme.Light ? "Giao diện Sáng" : "Giao diện Tối");
    }

    private void BtnSettings_Click(object sender, RoutedEventArgs e)
    {
        var win = new SettingsWindow();
        win.Owner = this;
        win.ShowDialog();
    }

    private void BtnAbout_Click(object sender, RoutedEventArgs e)
    {
        System.Windows.MessageBox.Show(
            "Nhóm Pho\n\nLê Minh Phúc - Tống Minh Hiếu - Lý Thành Lợi\n\nHọc Phần: Công cụ và môi trường phát triển",
            "Thông tin",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    private static bool IsAudioFile(string path)
    {
        var extension = Path.GetExtension(path).ToLowerInvariant();
        return extension is ".mp3" or ".wav" or ".wma" or ".aac" or ".flac" or ".ogg" or ".m4a";
    }

    private void SetMessage(string message)
    {
        if (lblMessage == null) return;
        lblMessage.Text = message ?? "";
    }

    private void SyncFavoriteButton(TrackInfo? track)
    {
        if (btnToggleFavorite == null) return;
        if (track == null)
        {
            btnToggleFavorite.Content = "🤍";
            return;
        }

        var isFav = Favorites.Any(f => f.FilePath == track.FilePath) || track.IsFavorite;
        track.IsFavorite = isFav;
        btnToggleFavorite.Content = isFav ? "❤️" : "🤍";
    }

    private void BtnToggleFavorite_Click(object sender, RoutedEventArgs e)
    {
        if (_currentTrackIndex < 0 || _currentTrackIndex >= Playlist.Count)
        {
            SetMessage("Chưa có bài hát để yêu thích");
            return;
        }

        ToggleFavorite(Playlist[_currentTrackIndex]);
    }

    private void BtnRowToggleFavorite_Click(object sender, RoutedEventArgs e)
    {
        if (sender is System.Windows.Controls.Button { DataContext: TrackInfo track })
        {
            ToggleFavorite(track);
        }
    }

    private void ToggleFavorite(TrackInfo track)
    {
        var existing = Favorites.FirstOrDefault(f => f.FilePath == track.FilePath);
        if (existing != null)
        {
            Favorites.Remove(existing);
            SetMessage("Đã xóa khỏi Yêu thích");
        }
        else
        {
            Favorites.Insert(0, new TrackInfo
            {
                Title = string.IsNullOrWhiteSpace(track.Title) ? "Chưa rõ tên bài hát" : track.Title,
                Artist = string.IsNullOrWhiteSpace(track.Artist) ? "Chưa rõ ca sĩ" : track.Artist,
                FilePath = track.FilePath,
                Duration = track.Duration,
                DurationText = track.DurationText,
                IsVideo = track.IsVideo,
                ArtworkData = track.ArtworkData,
                IsFavorite = true
            });
            SetMessage("Đã thêm vào Yêu thích");
        }

        // Sync IsFavorite across collections
        var isFavNow = Favorites.Any(f => f.FilePath == track.FilePath);
        foreach (var p in Playlist.Where(p => p.FilePath == track.FilePath))
            p.IsFavorite = isFavNow;
        foreach (var h in History.Where(h => h.FilePath == track.FilePath))
            h.IsFavorite = isFavNow;

        RefreshFavoritesSequence();
        DataManager.SaveFavorites(Favorites);
        SyncFavoriteButton(_currentTrackIndex >= 0 && _currentTrackIndex < Playlist.Count ? Playlist[_currentTrackIndex] : null);
        UpdateCurrentView();
    }

    private void BtnRowDelete_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not System.Windows.Controls.Button { DataContext: TrackInfo track })
            return;

        var index = Playlist.ToList().FindIndex(p => p.FilePath == track.FilePath);
        if (index < 0) return;

        DeleteFromPlaylist(index);
    }

    private void DeleteFromPlaylist(int index)
    {
        if (index < 0 || index >= Playlist.Count) return;

        var wasCurrent = index == _currentTrackIndex;
        Playlist.RemoveAt(index);
        RefreshSequenceNumbers();
        UpdateCurrentView();

        if (Playlist.Count == 0)
        {
            _mediaPlayer.Stop();
            _positionTimer?.Stop();
            _isPlaying = false;
            _isMediaLoaded = false;
            _currentTrackIndex = -1;
            btnPause.Content = "▶️";
            lblTrackTitle.Text = "Chưa có bài hát";
            lblArtist.Text = "Thêm bài hát để bắt đầu";
            lblStatus.Text = "Sẵn sàng";
            SyncFavoriteButton(null);
            SetMessage("Danh sách phát đang trống");
            sldProgress.Value = 0;
            lblTimeDisplay.Text = "00:00 / 00:00";
            return;
        }

        if (!wasCurrent)
        {
            if (index < _currentTrackIndex) _currentTrackIndex--;
            SetMessage("Đã xóa khỏi danh sách phát");
            return;
        }

        // If deleted current track: continue with next (same index), or last if index out of range.
        var nextIndex = Math.Min(index, Playlist.Count - 1);
        PlayTrack(nextIndex);
        SetMessage("Đã xóa bài đang phát và chuyển bài");
    }
}
