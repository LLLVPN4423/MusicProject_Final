namespace MusicApp.Models;

public sealed class UserAccount
{
    public string Username { get; set; } = "";
    public string Password { get; set; } = "";
    public bool IsPremium { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

