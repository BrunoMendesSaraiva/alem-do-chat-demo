using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using DemoApi.Auth;
using DemoApi.Profile;
using DemoApi.Users;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DemoApi.Tests;

public class ProfileTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public ProfileTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task GetProfile_WithoutToken_ReturnsUnauthorized()
    {
        var response = await _client.GetAsync("/profile");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task GetProfile_WithToken_ReturnsLoggedUser()
    {
        await AuthenticateAsync("carla@example.com", "Senha@123");

        var profile = await _client.GetFromJsonAsync<UserResponse>("/profile");

        Assert.Equal("Carla Mendes", profile!.Name);
        Assert.Equal("carla@example.com", profile.Email);
    }

    [Fact]
    public async Task UpdateProfile_ChangesOnlyTheName()
    {
        await _client.PostAsJsonAsync("/users", new RegisterUserRequest("Gabi Torres", "gabi@example.com", "Senha@123"));
        await AuthenticateAsync("gabi@example.com", "Senha@123");

        var response = await _client.PutAsJsonAsync("/profile", new { name = "Gabriela Torres", email = "outro@example.com" });

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var profile = await _client.GetFromJsonAsync<UserResponse>("/profile");
        Assert.Equal("Gabriela Torres", profile!.Name);
        Assert.Equal("gabi@example.com", profile.Email);
    }

    [Fact]
    public async Task UpdateProfile_WithEmptyName_ReturnsBadRequest()
    {
        await AuthenticateAsync("carla@example.com", "Senha@123");

        var response = await _client.PutAsJsonAsync("/profile", new UpdateProfileRequest(" "));

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    private async Task AuthenticateAsync(string email, string password)
    {
        var response = await _client.PostAsJsonAsync("/login", new LoginRequest(email, password));
        var body = await response.Content.ReadFromJsonAsync<LoginResponse>();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", body!.AccessToken);
    }
}
