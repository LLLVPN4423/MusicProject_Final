using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MusicApp.Models;

public class TrackInfo : INotifyPropertyChanged
{
    private int _sequence;
    private string _title = string.Empty;
    private string _artist = "Chưa rõ ca sĩ";
    private string _filePath = string.Empty;
    private TimeSpan _duration = TimeSpan.Zero;
    private string _durationText = "--:--";
    private byte[]? _artworkData;
    private bool _isVideo;
    private bool _isFavorite;
    private DateTime _lastPlayed;

    public int Sequence
    {
        get => _sequence;
        set => SetField(ref _sequence, value);
    }

    public string Title
    {
        get => _title;
        set => SetField(ref _title, value);
    }

    public string Artist
    {
        get => _artist;
        set => SetField(ref _artist, value);
    }

    public string FilePath
    {
        get => _filePath;
        set => SetField(ref _filePath, value);
    }

    public TimeSpan Duration
    {
        get => _duration;
        set => SetField(ref _duration, value);
    }

    public string DurationText
    {
        get => _durationText;
        set => SetField(ref _durationText, value);
    }

    public byte[]? ArtworkData
    {
        get => _artworkData;
        set => SetField(ref _artworkData, value);
    }

    public bool IsVideo
    {
        get => _isVideo;
        set => SetField(ref _isVideo, value);
    }

    public bool IsFavorite
    {
        get => _isFavorite;
        set => SetField(ref _isFavorite, value);
    }

    public DateTime LastPlayed
    {
        get => _lastPlayed;
        set => SetField(ref _lastPlayed, value);
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

    private void SetField<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
    {
        if (EqualityComparer<T>.Default.Equals(field, value))
        {
            return;
        }

        field = value;
        OnPropertyChanged(propertyName);
    }
}
