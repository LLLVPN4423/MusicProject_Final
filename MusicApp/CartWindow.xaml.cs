using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using MusicApp.Models;
using MusicApp.Services;

namespace MusicApp;

public partial class CartWindow : Window
{
    private readonly ObservableCollection<CartItem> _cart;

    public CartWindow()
    {
        InitializeComponent();

        _cart = CartManager.LoadCart();
        lvCart.ItemsSource = _cart;

        lstPackages.ItemsSource = CartManager.DefaultPackages
            .Select(p => $"{p.Name} - {p.Price:N0}đ\n{p.Description}")
            .ToList();

        UpdateTotal();
    }

    private void BtnAddToCart_Click(object sender, RoutedEventArgs e)
    {
        var idx = lstPackages.SelectedIndex;
        if (idx < 0)
        {
            System.Windows.MessageBox.Show("Vui lòng chọn một gói dịch vụ", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var pkg = CartManager.DefaultPackages[idx];
        var existing = _cart.FirstOrDefault(x => x.Package.Id == pkg.Id);
        if (existing != null)
        {
            existing.Quantity += 1;
        }
        else
        {
            _cart.Add(new CartItem { Package = pkg, Quantity = 1 });
        }

        lvCart.Items.Refresh();
        CartManager.SaveCart(_cart);
        UpdateTotal();
    }

    private void BtnRemoveItem_Click(object sender, RoutedEventArgs e)
    {
        if (sender is not System.Windows.Controls.Button { DataContext: CartItem item }) return;
        _cart.Remove(item);
        CartManager.SaveCart(_cart);
        UpdateTotal();
    }

    private void BtnCheckout_Click(object sender, RoutedEventArgs e)
    {
        if (_cart.Count == 0)
        {
            System.Windows.MessageBox.Show("Giỏ hàng đang trống", "Thông báo", MessageBoxButton.OK, MessageBoxImage.Information);
            return;
        }

        var win = new PaymentWindow { Owner = this };
        win.ShowDialog();
        Close();
    }

    private void UpdateTotal()
    {
        var total = _cart.Sum(x => x.LineTotal);
        txtTotal.Text = $"Tổng tiền: {total:N0}đ";
    }

    private void BtnClose_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}

