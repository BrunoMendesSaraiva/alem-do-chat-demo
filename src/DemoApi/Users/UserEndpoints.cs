using DemoApi.Auth;

namespace DemoApi.Users;

public static class UserEndpoints
{
    public const int MinPasswordLength = 8;

    public static void MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("/users", Register);
    }

    private static IResult Register(RegisterUserRequest request, InMemoryUserRepository repository)
    {
        if (!EmailValidator.IsValid(request.Email))
        {
            return Results.BadRequest(new { error = "Invalid email format." });
        }

        if (request.Password is null || request.Password.Length < MinPasswordLength)
        {
            return Results.BadRequest(new { error = $"Password must have at least {MinPasswordLength} characters." });
        }

        if (repository.EmailExists(request.Email))
        {
            return Results.Conflict(new { error = "Email already registered." });
        }

        var user = repository.Add(request.Name, request.Email, PasswordHasher.Hash(request.Password));

        return Results.Created($"/users/{user.Id}", UserResponse.From(user));
    }
}

public record RegisterUserRequest(string Name, string Email, string Password);

public record UserResponse(int Id, string Name, string Email)
{
    public static UserResponse From(User user) => new(user.Id, user.Name, user.Email);
}
