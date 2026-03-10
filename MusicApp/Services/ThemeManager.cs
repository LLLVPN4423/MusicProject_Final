using System.Windows;
using System.Windows.Media;

namespace MusicApp.Services;

public static class ThemeManager
{
    public static event Action<Theme>? ThemeChanged;

    public static Theme CurrentTheme { get; private set; } = Theme.Light;

    public static void SwitchTheme()
    {
        CurrentTheme = CurrentTheme == Theme.Light ? Theme.Dark : Theme.Light;
        ThemeChanged?.Invoke(CurrentTheme);
    }

    public static void ApplyTheme(Theme theme)
    {
        CurrentTheme = theme;
        ThemeChanged?.Invoke(CurrentTheme);
    }

    public static System.Windows.Media.Color GetColor(string colorKey)
    {
        return colorKey switch
        {
            "Primary" when CurrentTheme == Theme.Light => System.Windows.Media.Color.FromRgb(192, 124, 199),
            "Primary" when CurrentTheme == Theme.Dark => System.Windows.Media.Color.FromRgb(147, 51, 234),
            
            "Background" when CurrentTheme == Theme.Light => System.Windows.Media.Color.FromRgb(111, 107, 105),
            "Background" when CurrentTheme == Theme.Dark => System.Windows.Media.Color.FromRgb(24, 24, 27),
            
            "Surface" when CurrentTheme == Theme.Light => System.Windows.Media.Color.FromRgb(124, 120, 118),
            "Surface" when CurrentTheme == Theme.Dark => System.Windows.Media.Color.FromRgb(39, 39, 42),
            
            "Sidebar" when CurrentTheme == Theme.Light => System.Windows.Media.Color.FromRgb(91, 88, 86),
            "Sidebar" when CurrentTheme == Theme.Dark => System.Windows.Media.Color.FromRgb(32, 32, 35),
            
            "Card" when CurrentTheme == Theme.Light => System.Windows.Media.Color.FromRgb(162, 157, 154),
            "Card" when CurrentTheme == Theme.Dark => System.Windows.Media.Color.FromRgb(64, 64, 67),
            
            "TextPrimary" when CurrentTheme == Theme.Light => System.Windows.Media.Color.FromRgb(242, 239, 234),
            "TextPrimary" when CurrentTheme == Theme.Dark => System.Windows.Media.Color.FromRgb(255, 255, 255),
            
            "TextSecondary" when CurrentTheme == Theme.Light => System.Windows.Media.Color.FromRgb(214, 209, 207),
            "TextSecondary" when CurrentTheme == Theme.Dark => System.Windows.Media.Color.FromRgb(170, 170, 170),
            
            "Button" when CurrentTheme == Theme.Light => System.Windows.Media.Color.FromRgb(141, 136, 134),
            "Button" when CurrentTheme == Theme.Dark => System.Windows.Media.Color.FromRgb(82, 82, 84),
            
            "ButtonHover" when CurrentTheme == Theme.Light => System.Windows.Media.Color.FromRgb(160, 141, 156),
            "ButtonHover" when CurrentTheme == Theme.Dark => System.Windows.Media.Color.FromRgb(107, 107, 109),
            
            "Accent" when CurrentTheme == Theme.Light => System.Windows.Media.Color.FromRgb(246, 245, 242),
            "Accent" when CurrentTheme == Theme.Dark => System.Windows.Media.Color.FromRgb(46, 46, 49),
            
            _ => System.Windows.Media.Colors.Gray
        };
    }
}

public enum Theme
{
    Light,
    Dark
}
