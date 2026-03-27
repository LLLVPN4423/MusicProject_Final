using System.Windows;
using MusicApp.Services;

namespace MusicApp;

public partial class LoginWindow : Window
{
    public LoginWindow(string? presetUsername = null)
    {
        InitializeComponent();
        if (!string.IsNullOrWhiteSpace(presetUsername) && txtUsername != null)
            txtUsername.Text = presetUsername;
    }

    private void BtnLogin_Click(object sender, RoutedEventArgs e)
    {
        txtError.Text = "";
        var (ok, message) = AuthManager.Login(txtUsername.Text, txtPassword.Password);
        if (!ok)
        {
            txtError.Text = message;
            return;
        }

        System.Windows.MessageBox.Show(message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        DialogResult = true;
        Close();
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}

