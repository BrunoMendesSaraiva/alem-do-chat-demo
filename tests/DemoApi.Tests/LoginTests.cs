using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Json;
using DemoApi.Auth;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DemoApi.Tests;

public class LoginTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public LoginTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_WithValidCredentials_ReturnsToken()
    {
        var response = await _client.PostAsJsonAsync("/login", new LoginRequest("ana@example.com", "Senha@123"));

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        var body = await response.Content.ReadFromJsonAsync<LoginResponse>();
        Assert.False(string.IsNullOrEmpty(body!.AccessToken));
    }

    [Fact]
    public async Task Login_WithWrongPassword_ReturnsUnauthorized()
    {
        var response = await _client.PostAsJsonAsync("/login", new LoginRequest("ana@example.com", "senha-errada"));

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_WithUnknownEmail_ReturnsUnauthorized()
    {
        var response = await _client.PostAsJsonAsync("/login", new LoginRequest("ninguem@example.com", "Senha@123"));

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task Login_ReturnsTokenThatExpiresIn30Minutes()
    {
        var response = await _client.PostAsJsonAsync("/login", new LoginRequest("bruno@example.com", "Senha@123"));
        var body = await response.Content.ReadFromJsonAsync<LoginResponse>();

        var token = new JwtSecurityTokenHandler().ReadJwtToken(body!.AccessToken);

        Assert.Equal(TimeSpan.FromMinutes(30), token.ValidTo - token.ValidFrom);
    }
}
