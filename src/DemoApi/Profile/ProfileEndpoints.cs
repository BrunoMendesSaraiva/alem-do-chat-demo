using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DemoApi.Users;

namespace DemoApi.Profile;

public static class ProfileEndpoints
{
    public static void MapProfileEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/profile", GetProfile).RequireAuthorization();
        app.MapPut("/profile", UpdateProfile).RequireAuthorization();
    }

    private static IResult GetProfile(ClaimsPrincipal principal, InMemoryUserRepository repository)
    {
        var user = FindCurrentUser(principal, repository);

        return user is null ? Results.NotFound() : Results.Ok(UserResponse.From(user));
    }

    private static IResult UpdateProfile(UpdateProfileRequest request, ClaimsPrincipal principal, InMemoryUserRepository repository)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return Results.BadRequest(new { error = "Name is required." });
        }

        var user = FindCurrentUser(principal, repository);
        if (user is null)
        {
            return Results.NotFound();
        }

        user.Name = request.Name.Trim();

        return Results.Ok(UserResponse.From(user));
    }

    private static User? FindCurrentUser(ClaimsPrincipal principal, InMemoryUserRepository repository)
    {
        var userId = principal.FindFirstValue(JwtRegisteredClaimNames.Sub);

        return int.TryParse(userId, out var id) ? repository.GetById(id) : null;
    }
}

public record UpdateProfileRequest(string Name);
