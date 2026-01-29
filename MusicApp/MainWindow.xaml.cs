using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using Microsoft.Win32;
using MusicApp.Models;
using TagLib;
using TagLibFile = TagLib.File;

namespace MusicApp;

public partial class MainWindow : Window
{
    private readonly MediaPlayer _mediaPlayer = new();
    private readonly DispatcherTimer _positionTimer;
    private bool _isDraggingProgress;
    private bool _isPlaying;
    private int _currentTrackIndex = -1;

    public ObservableCollection<TrackInfo> Playlist { get; } = new();

    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;

        _mediaPlayer.MediaOpened += MediaPlayerOnMediaOpened;
        _mediaPlayer.MediaEnded += MediaPlayerOnMediaEnded;

        _positionTimer = new DispatcherTimer
        {
            Interval = TimeSpan.FromMilliseconds(500)
        };
        _positionTimer.Tick += PositionTimerOnTick;
    }

    private void BtnOpenFile_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Filter = "Audio/Video Files|*.mp3;*.wav;*.wma;*.aac;*.flac;*.ogg;*.m4a;*.mp4|All Files|*.*",
            Multiselect = true,
            Title = "Chọn bài hát"
        };

        if (dialog.ShowDialog() != true)
        {
            lblStatus.Text = "Không có bài hát nào được thêm";
            return;
        }

        foreach (var filePath in dialog.FileNames)
        {
            try
            {
                using var tagFile = TagLibFile.Create(filePath);
                var duration = tagFile.Properties.Duration;
                var artworkData = ExtractArtwork(tagFile.Tag.Pictures);

                var track = new TrackInfo
                {
                    Title = string.IsNullOrWhiteSpace(tagFile.Tag.Title)
                        ? Path.GetFileNameWithoutExtension(filePath)
                        : tagFile.Tag.Title,
                    Artist = tagFile.Tag.Performers.Length > 0
                        ? string.Join(", ", tagFile.Tag.Performers)
                        : "Unknown Artist",
                    Duration = duration,
                    DurationText = duration == TimeSpan.Zero ? "--:--" : FormatTimeSpan(duration),
                    FilePath = filePath,
                    ArtworkData = artworkData,
                    IsVideo = IsVideoFile(filePath)
                };

                Playlist.Add(track);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Không thể đọc file \"{Path.GetFileName(filePath)}\".\nChi tiết: {ex.Message}",
                    "Lỗi đọc file",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        RefreshSequenceNumbers();

        if (Playlist.Count == 0)
        {
            lblStatus.Text = "Chưa có bài hát hợp lệ";
            return;
        }

        if (_currentTrackIndex == -1)
        {
            PlayTrack(0);
        }
        else
        {
            lblStatus.Text = $"Đã thêm {dialog.FileNames.Length} bài hát";
        }
    }

    private void MenuButton_Click(object sender, RoutedEventArgs e)
    {
        menuItemsPanel.Visibility = menuItemsPanel.Visibility == Visibility.Visible
            ? Visibility.Collapsed
            : Visibility.Visible;
    }

    private void BtnPrev_Click(object sender, RoutedEventArgs e)
    {
        if (Playlist.Count == 0)
        {
            return;
        }

        var nextIndex = (_currentTrackIndex - 1 + Playlist.Count) % Playlist.Count;
        PlayTrack(nextIndex);
    }

    private void BtnNext_Click(object sender, RoutedEventArgs e)
    {
        if (Playlist.Count == 0)
        {
            return;
        }

        var nextIndex = (_currentTrackIndex + 1) % Playlist.Count;
        PlayTrack(nextIndex);
    }

    private void BtnPause_Click(object sender, RoutedEventArgs e)
    {
        if (Playlist.Count == 0)
        {
            return;
        }

        if (_isPlaying)
        {
            _mediaPlayer.Pause();
            _isPlaying = false;
            btnPause.Content = "Play";
            lblStatus.Text = "Tạm dừng";
        }
        else
        {
            _mediaPlayer.Play();
            _isPlaying = true;
            btnPause.Content = "Pause";
            lblStatus.Text = "Đang phát";
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
        if (index < 0 || index >= Playlist.Count)
        {
            return;
        }

        var track = Playlist[index];
        _currentTrackIndex = index;

        try
        {
            _mediaPlayer.Open(new Uri(track.FilePath));
            _mediaPlayer.Play();
            _positionTimer.Start();
            _isPlaying = true;
            btnPause.Content = "Pause";

            lvPlaylist.SelectedIndex = index;
            lvPlaylist.ScrollIntoView(track);

            lblTrackTitle.Text = track.Title;
            lblArtist.Text = track.Artist;
            lblStatus.Text = "Đang phát";
            sldProgress.Value = 0;

            DisplayTrackVisuals(track);

            var durationText = track.Duration == TimeSpan.Zero
                ? "00:00"
                : FormatTimeSpan(track.Duration);
            lblTimeDisplay.Text = $"00:00 / {durationText}";
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Không thể phát bài hát.\nChi tiết: {ex.Message}",
                "Lỗi phát nhạc",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            lblStatus.Text = "Lỗi phát nhạc";
        }
    }

    private void MediaPlayerOnMediaOpened(object? sender, EventArgs e)
    {
        if (!_mediaPlayer.NaturalDuration.HasTimeSpan)
        {
            return;
        }

        var total = _mediaPlayer.NaturalDuration.TimeSpan;
        sldProgress.Maximum = total.TotalSeconds;
        UpdateTimeDisplay(TimeSpan.Zero, total);
    }

    private void MediaPlayerOnMediaEnded(object? sender, EventArgs e)
    {
        if (Playlist.Count == 0)
        {
            return;
        }

        BtnNext_Click(this, new RoutedEventArgs());
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
}