using System.IO;
using System.Text.Json;
using MusicApp.Models;

namespace MusicApp.Services;

public static class AuthManager
{
    private static readonly string AccountsPath = Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
        "MusicApp",
        "accounts.json");

    public static UserAccount? CurrentUser { get; private set; }

    public static IReadOnlyList<UserAccount> LoadAccounts()
    {
        try
        {
            if (!File.Exists(AccountsPath)) return Array.Empty<UserAccount>();
            var json = File.ReadAllText(AccountsPath);
            return JsonSerializer.Deserialize<List<UserAccount>>(json) ?? new List<UserAccount>();
        }
        catch
        {
            return Array.Empty<UserAccount>();
        }
    }

    private static void SaveAccounts(IReadOnlyList<UserAccount> accounts)
    {
        try
        {
            var directory = Path.GetDirectoryName(AccountsPath);
            if (!string.IsNullOrWhiteSpace(directory) && !Directory.Exists(directory))
                Directory.CreateDirectory(directory);

            var json = JsonSerializer.Serialize(accounts, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(AccountsPath, json);
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"Không thể lưu dữ liệu tài khoản: {ex.Message}", ex);
        }
    }

    public static (bool ok, string message) Register(string username, string password)
    {
        username = (username ?? "").Trim();
        password = (password ?? "").Trim();

        if (username.Length < 3) return (false, "Tên đăng nhập phải có ít nhất 3 ký tự");
        if (password.Length < 4) return (false, "Mật khẩu phải có ít nhất 4 ký tự");

        var accounts = LoadAccounts().ToList();
        if (accounts.Any(a => string.Equals(a.Username, username, StringComparison.OrdinalIgnoreCase)))
            return (false, "Tên đăng nhập đã tồn tại");

        accounts.Add(new UserAccount
        {
            Username = username,
            Password = password,
            IsPremium = false,
            CreatedAt = DateTime.UtcNow
        });

        try
        {
            SaveAccounts(accounts);
            return (true, "Đăng ký thành công");
        }
        catch (Exception ex)
        {
            return (false, ex.Message);
        }
    }

    public static (bool ok, string message) Login(string username, string password)
    {
        username = (username ?? "").Trim();
        password = (password ?? "").Trim();

        var accounts = LoadAccounts();
        var match = accounts.FirstOrDefault(a =>
            string.Equals(a.Username, username, StringComparison.OrdinalIgnoreCase) &&
            a.Password == password);

        if (match == null) return (false, "Sai tên đăng nhập hoặc mật khẩu");

        CurrentUser = match;
        return (true, match.IsPremium ? "Đăng nhập thành công (Premium)" : "Đăng nhập thành công");
    }

    public static void Logout()
    {
        CurrentUser = null;
    }

    public static void SetPremiumForCurrentUser(bool isPremium)
    {
        if (CurrentUser == null) return;

        var accounts = LoadAccounts().ToList();
        var idx = accounts.FindIndex(a => string.Equals(a.Username, CurrentUser.Username, StringComparison.OrdinalIgnoreCase));
        if (idx < 0) return;

        accounts[idx].IsPremium = isPremium;
        SaveAccounts(accounts);
        CurrentUser.IsPremium = isPremium;
    }
}

