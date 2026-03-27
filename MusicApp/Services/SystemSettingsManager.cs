using System.IO;
using System.Text.Json;
using MusicApp.Models;

namespace MusicApp.Services;

public static class SystemSettingsManager
{
    private static readonly string SettingsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "MusicApp",
        "systemSettings.json");

    public static SystemSettings Load()
    {
        try
        {
            if (!File.Exists(SettingsPath))
                return new SystemSettings();

            var json = File.ReadAllText(SettingsPath);
            return JsonSerializer.Deserialize<SystemSettings>(json) ?? new SystemSettings();
        }
        catch
        {
            return new SystemSettings();
        }
    }

    public static void Save(SystemSettings settings)
    {
        var directory = Path.GetDirectoryName(SettingsPath);
        if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(SettingsPath, json);
    }
}

