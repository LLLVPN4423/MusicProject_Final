namespace MusicApp.Models;

public sealed class CartItem
{
    public ServicePackage Package { get; set; } = new();
    public int Quantity { get; set; } = 1;

    public decimal LineTotal => Package.Price * Quantity;
}

