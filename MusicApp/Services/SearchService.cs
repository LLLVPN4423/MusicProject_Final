using System.Collections.ObjectModel;
using System.Linq;
using MusicApp.Models;

namespace MusicApp.Services;

public static class SearchService
{
    public static ObservableCollection<TrackInfo> SearchTracks(
        ObservableCollection<TrackInfo> tracks, 
        string searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return tracks;

        var searchLower = searchTerm.ToLowerInvariant();
        var results = tracks.Where(track =>
            track.Title.ToLowerInvariant().Contains(searchLower) ||
            track.Artist.ToLowerInvariant().Contains(searchLower) ||
            track.FilePath.ToLowerInvariant().Contains(searchLower)
        ).ToList();

        return new ObservableCollection<TrackInfo>(results);
    }

    public static ObservableCollection<TrackInfo> FilterByArtist(
        ObservableCollection<TrackInfo> tracks, 
        string artist)
    {
        if (string.IsNullOrWhiteSpace(artist))
            return tracks;

        var results = tracks.Where(track =>
            track.Artist.Equals(artist, StringComparison.OrdinalIgnoreCase)
        ).ToList();

        return new ObservableCollection<TrackInfo>(results);
    }

    public static ObservableCollection<TrackInfo> FilterByFavorites(
        ObservableCollection<TrackInfo> tracks)
    {
        var results = tracks.Where(track => track.IsFavorite).ToList();
        return new ObservableCollection<TrackInfo>(results);
    }
}
