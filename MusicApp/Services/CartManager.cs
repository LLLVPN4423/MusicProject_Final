using System.Collections.ObjectModel;
using System.IO;
using System.Text.Json;
using MusicApp.Models;

namespace MusicApp.Services;

public static class CartManager
{
    private static readonly string CartPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "MusicApp",
        "cart.json");

    public static ObservableCollection<CartItem> LoadCart()
    {
        try
        {
            if (!File.Exists(CartPath)) return new ObservableCollection<CartItem>();
            var json = File.ReadAllText(CartPath);
            var data = JsonSerializer.Deserialize<List<CartItem>>(json) ?? new List<CartItem>();
            return new ObservableCollection<CartItem>(data);
        }
        catch
        {
            return new ObservableCollection<CartItem>();
        }
    }

    public static void SaveCart(ObservableCollection<CartItem> cart)
    {
        var directory = Path.GetDirectoryName(CartPath);
        if (!Directory.Exists(directory)) Directory.CreateDirectory(directory!);

        var json = JsonSerializer.Serialize(cart.ToList(), new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(CartPath, json);
    }

    public static IReadOnlyList<ServicePackage> DefaultPackages { get; } = new List<ServicePackage>
    {
        new() { Id = "premium_month", Name = "Gói Premium 1 tháng", Price = 49000, Description = "Nghe nhạc không quảng cáo, chất lượng cao" },
        new() { Id = "premium_year", Name = "Gói Premium 12 tháng", Price = 499000, Description = "Tiết kiệm hơn, trải nghiệm Premium trọn năm" },
    };
}

