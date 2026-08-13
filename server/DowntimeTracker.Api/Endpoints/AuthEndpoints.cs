using DowntimeTracker.Api.DTOs;
using DowntimeTracker.Api.Services;

namespace DowntimeTracker.Api.Endpoints;

public static class AuthEndpoints
{
    public static void MapAuthEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/auth");

        group.MapPost("/login", async (LoginRequest request, AuthService authService, TokenService tokenService) =>
        {
            var user = await authService.ValidateUserAsync(request.Username, request.Password);
            if (user is null)
                return Results.Unauthorized();

            var token = tokenService.GenerateToken(user);
            return Results.Ok(new LoginResponse
            {
                Token = token,
                Username = user.Username,
                Role = user.Role
            });
        });
    }
}