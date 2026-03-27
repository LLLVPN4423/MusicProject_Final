using System.Linq;
using System.Windows;
using MusicApp.Models;
using MusicApp.Services;

namespace MusicApp;

public partial class SettingsWindow : Window
{
    private bool _isUpdatingSystemSettings;

    public SettingsWindow()
    {
        InitializeComponent();
        RefreshAccountStatus();
        RefreshProfileSection();
        RefreshSystemSettings();
    }

    private void RefreshAccountStatus()
    {
        if (txtAccountStatus == null) return;

        var user = AuthManager.CurrentUser;
        txtAccountStatus.Text = user == null
            ? "Chưa đăng nhập"
            : user.IsPremium
                ? $"Đã đăng nhập: {user.Username} (Premium)"
                : $"Đã đăng nhập: {user.Username} (Thường)";

        if (btnLogout != null)
            btnLogout.Visibility = user == null ? Visibility.Collapsed : Visibility.Visible;

        if (btnLogin != null)
            btnLogin.Visibility = user == null ? Visibility.Visible : Visibility.Collapsed;

        if (btnRegister != null)
            btnRegister.Visibility = user == null ? Visibility.Visible : Visibility.Collapsed;
    }

    private void RefreshProfileSection()
    {
        if (txtProfileUsername == null) return;

        var user = AuthManager.CurrentUser;
        var hasUser = user != null;

        if (!hasUser)
        {
            txtProfileUsername.Text = "Chưa đăng nhập";
            txtProfilePremium.Text = "";
            txtProfileFavoritesCount.Text = "";
            txtProfileTopArtists.Text = "Chưa có dữ liệu sở thích";
            return;
        }

        txtProfileUsername.Text = user!.Username;
        txtProfilePremium.Text = user.IsPremium ? "Premium" : "Tài khoản thường";

        if (Owner is not MainWindow main)
        {
            txtProfileFavoritesCount.Text = "";
            txtProfileTopArtists.Text = "";
            return;
        }

        var favoriteCount = main.Favorites.Count;
        txtProfileFavoritesCount.Text = $"Số bài yêu thích: {favoriteCount}";

        if (favoriteCount == 0)
        {
            txtProfileTopArtists.Text = "Chưa có sở thích (chưa có bài yêu thích).";
            return;
        }

        var topArtists = main.Favorites
            .Where(t => !string.IsNullOrWhiteSpace(t.Artist))
            .GroupBy(t => t.Artist)
            .OrderByDescending(g => g.Count())
            .Take(3)
            .Select(g => $"{g.Key} ({g.Count()})");

        txtProfileTopArtists.Text = "Top ca sĩ: " + string.Join(", ", topArtists);
    }

    private void BtnLogin_Click(object sender, RoutedEventArgs e)
    {
        var win = new LoginWindow { Owner = this };
        win.ShowDialog();
        RefreshAccountStatus();
        RefreshProfileSection();
    }

    private void BtnRegister_Click(object sender, RoutedEventArgs e)
    {
        var win = new RegisterWindow { Owner = this };
        win.ShowDialog();
        RefreshAccountStatus();
        RefreshProfileSection();
    }

    private void BtnLogout_Click(object sender, RoutedEventArgs e)
    {
        AuthManager.Logout();
        RefreshAccountStatus();
        RefreshProfileSection();
        System.Windows.MessageBox.Show("Đã đăng xuất", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void BtnCart_Click(object sender, RoutedEventArgs e)
    {
        var win = new CartWindow { Owner = this };
        win.ShowDialog();
    }

    private void BtnPayment_Click(object sender, RoutedEventArgs e)
    {
        var win = new PaymentWindow { Owner = this };
        win.ShowDialog();
        RefreshAccountStatus();
        RefreshProfileSection();
    }

    private void RefreshSystemSettings()
    {
        if (sldDefaultVolume == null || chkEnableShortcuts == null || lblDefaultVolume == null)
            return;

        var settings = SystemSettingsManager.Load();

        _isUpdatingSystemSettings = true;
        sldDefaultVolume.Value = settings.DefaultVolume;
        lblDefaultVolume.Text = $"{Math.Round(settings.DefaultVolume * 100)}%";
        chkEnableShortcuts.IsChecked = settings.EnableShortcuts;
        _isUpdatingSystemSettings = false;
    }

    private void SldDefaultVolume_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
    {
        if (_isUpdatingSystemSettings) return;
        SaveAndApplySystemSettings();
    }

    private void ChkEnableShortcuts_Checked(object sender, RoutedEventArgs e)
    {
        if (_isUpdatingSystemSettings) return;
        SaveAndApplySystemSettings();
    }

    private void SaveAndApplySystemSettings()
    {
        if (sldDefaultVolume == null || chkEnableShortcuts == null || lblDefaultVolume == null)
            return;

        var settings = new SystemSettings
        {
            DefaultVolume = sldDefaultVolume.Value,
            EnableShortcuts = chkEnableShortcuts.IsChecked ?? true
        };

        lblDefaultVolume.Text = $"{Math.Round(settings.DefaultVolume * 100)}%";
        SystemSettingsManager.Save(settings);

        if (Owner is MainWindow main)
            main.ApplySystemSettings(settings);
    }

    private void BtnClearHistory_Click(object sender, RoutedEventArgs e)
    {
        if (Owner is MainWindow main)
        {
            main.History.Clear();
            DataManager.SaveHistory(main.History);
        }
        System.Windows.MessageBox.Show("Đã xóa lịch sử nghe", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void BtnClearFavorites_Click(object sender, RoutedEventArgs e)
    {
        if (Owner is MainWindow main)
        {
            main.Favorites.Clear();
            DataManager.SaveFavorites(main.Favorites);
            foreach (var t in main.Playlist) t.IsFavorite = false;
        }
        System.Windows.MessageBox.Show("Đã xóa danh sách yêu thích", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}

