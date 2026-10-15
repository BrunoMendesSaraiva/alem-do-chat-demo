using DemoApi.Users;

namespace DemoApi.Auth;

public static class LoginEndpoints
{
    public static void MapLoginEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/login", Login);
    }

    private static IResult Login(LoginRequest request, InMemoryUserRepository repository, TokenService tokenService)
    {
        var user = repository.GetByEmail(request.Email);

        if (user is null || request.Password is null || !PasswordHasher.Verify(request.Password, user.PasswordHash))
        {
            return Results.Unauthorized();
        }

        var token = tokenService.CreateToken(user);

        return Results.Ok(new LoginResponse(token, TokenSettings.ExpirationMinutes * 60));
    }
}

public record LoginRequest(string Email, string Password);

public record LoginResponse(string AccessToken, int ExpiresInSeconds);
