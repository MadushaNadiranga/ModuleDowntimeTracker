using DowntimeTracker.Api.Data;
using DowntimeTracker.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace DowntimeTracker.Api.Services;

public class AuthService
{
    private readonly AppDbContext _db;

    public AuthService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<User?> ValidateUserAsync(string username, string password)
    {
        var user = await _db.Users.FirstOrDefaultAsync(u => u.Username == username && u.IsActive);
        if (user is null) return null;

        var isValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
        return isValid ? user : null;
    }
}