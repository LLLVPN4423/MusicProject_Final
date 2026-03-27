using System.Linq;
using System.Windows;
using MusicApp.Services;

namespace MusicApp;

public partial class PaymentWindow : Window
{
    public PaymentWindow()
    {
        InitializeComponent();
        cmbMethod.SelectedIndex = 0;

        var cart = CartManager.LoadCart();
        var total = cart.Sum(x => x.LineTotal);
        var user = AuthManager.CurrentUser;

        txtSummary.Text = user == null
            ? $"Bạn chưa đăng nhập.\n\nTổng thanh toán tạm tính: {total:N0}đ\nVui lòng đăng nhập để nâng cấp Premium."
            : user.IsPremium
                ? $"Tài khoản {user.Username} đã là Premium.\n\nTổng thanh toán tạm tính: {total:N0}đ"
                : $"Tài khoản: {user.Username}\n\nTổng thanh toán: {total:N0}đ\nSau khi thanh toán, tài khoản sẽ được nâng cấp Premium.";
    }

    private void BtnPay_Click(object sender, RoutedEventArgs e)
    {
        txtError.Text = "";

        var user = AuthManager.CurrentUser;
        if (user == null)
        {
            txtError.Text = "Bạn cần đăng nhập trước khi thanh toán.";
            return;
        }

        if (user.IsPremium)
        {
            System.Windows.MessageBox.Show("Tài khoản đã là Premium.", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            Close();
            return;
        }

        // Fake payment success
        AuthManager.SetPremiumForCurrentUser(true);

        var cart = CartManager.LoadCart();
        cart.Clear();
        CartManager.SaveCart(cart);

        System.Windows.MessageBox.Show("Thanh toán thành công. Tài khoản đã được nâng cấp Premium.", "Thành công",
            MessageBoxButton.OK, MessageBoxImage.Information);
        Close();
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}

