using System.Windows;
using MusicApp.Services;

namespace MusicApp;

public partial class RegisterWindow : Window
{
    public RegisterWindow()
    {
        InitializeComponent();
    }

    private void BtnRegister_Click(object sender, RoutedEventArgs e)
    {
        txtError.Text = "";
        if (txtPassword.Password != txtPassword2.Password)
        {
            txtError.Text = "Mật khẩu nhập lại không khớp";
            return;
        }

        var (ok, message) = AuthManager.Register(txtUsername.Text, txtPassword.Password);
        if (!ok)
        {
            txtError.Text = message;
            return;
        }

        System.Windows.MessageBox.Show(message, "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
        var login = new LoginWindow(txtUsername.Text) { Owner = Owner };
        login.ShowDialog();
        Close();
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}

