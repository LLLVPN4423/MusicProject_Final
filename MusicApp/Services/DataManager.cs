using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using MusicApp.Models;

namespace MusicApp.Services;

public static class DataManager
{
    private static readonly string FavoritesPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
        "MusicApp", 
        "favorites.json");
    
    private static readonly string HistoryPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), 
        "MusicApp", 
        "history.json");

    public static void SaveFavorites(ObservableCollection<TrackInfo> favorites)
    {
        try
        {
            var directory = Path.GetDirectoryName(FavoritesPath);
            if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var data = favorites.Select(f => new TrackData
            {
                Title = f.Title,
                Artist = f.Artist,
                FilePath = f.FilePath,
                Duration = f.Duration,
                DurationText = f.DurationText,
                IsVideo = f.IsVideo,
                ArtworkData = f.ArtworkData
            }).ToList();

            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(FavoritesPath, json);
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"Lỗi lưu Yêu thích: {ex.Message}", "Lỗi",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
        }
    }

    public static ObservableCollection<TrackInfo> LoadFavorites()
    {
        var favorites = new ObservableCollection<TrackInfo>();
        
        try
        {
            if (!File.Exists(FavoritesPath))
                return favorites;

            var json = File.ReadAllText(FavoritesPath);
            var data = JsonSerializer.Deserialize<List<TrackData>>(json);
            
            if (data != null)
            {
                foreach (var item in data)
                {
                    favorites.Add(new TrackInfo
                    {
                        Title = item.Title,
                        Artist = item.Artist,
                        FilePath = item.FilePath,
                        Duration = item.Duration,
                        DurationText = item.DurationText,
                        IsVideo = item.IsVideo,
                        ArtworkData = item.ArtworkData,
                        IsFavorite = true
                    });
                }
            }
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"Lỗi tải Yêu thích: {ex.Message}", "Lỗi",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
        }

        return favorites;
    }

    public static void SaveHistory(ObservableCollection<TrackInfo> history)
    {
        try
        {
            var directory = Path.GetDirectoryName(HistoryPath);
            if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var data = history.Take(100).Select(h => new TrackData
            {
                Title = h.Title,
                Artist = h.Artist,
                FilePath = h.FilePath,
                Duration = h.Duration,
                DurationText = h.DurationText,
                IsVideo = h.IsVideo,
                ArtworkData = h.ArtworkData,
                LastPlayed = h.LastPlayed
            }).ToList();

            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(HistoryPath, json);
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"Lỗi lưu Lịch sử: {ex.Message}", "Lỗi",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
        }
    }

    public static ObservableCollection<TrackInfo> LoadHistory()
    {
        var history = new ObservableCollection<TrackInfo>();
        
        try
        {
            if (!File.Exists(HistoryPath))
                return history;

            var json = File.ReadAllText(HistoryPath);
            var data = JsonSerializer.Deserialize<List<TrackData>>(json);
            
            if (data != null)
            {
                foreach (var item in data.OrderByDescending(x => x.LastPlayed))
                {
                    history.Add(new TrackInfo
                    {
                        Title = item.Title,
                        Artist = item.Artist,
                        FilePath = item.FilePath,
                        Duration = item.Duration,
                        DurationText = item.DurationText,
                        IsVideo = item.IsVideo,
                        ArtworkData = item.ArtworkData,
                        LastPlayed = item.LastPlayed
                    });
                }
            }
        }
        catch (Exception ex)
        {
            System.Windows.MessageBox.Show($"Lỗi tải Lịch sử: {ex.Message}", "Lỗi",
                System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Warning);
        }

        return history;
    }
}

public class TrackData
{
    public string Title { get; set; } = string.Empty;
    public string Artist { get; set; } = "Unknown Artist";
    public string FilePath { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
    public string DurationText { get; set; } = "--:--";
    public bool IsVideo { get; set; }
    public byte[]? ArtworkData { get; set; }
    public DateTime LastPlayed { get; set; }
}
