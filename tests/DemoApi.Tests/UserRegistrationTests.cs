using System.Net;
using System.Net.Http.Json;
using DemoApi.Users;
using Microsoft.AspNetCore.Mvc.Testing;

namespace DemoApi.Tests;

public class UserRegistrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public UserRegistrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_WithValidData_ReturnsCreated()
    {
        var request = new RegisterUserRequest("Diego Rocha", "diego@example.com", "Senha@123");

        var response = await _client.PostAsJsonAsync("/users", request);

        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
        var user = await response.Content.ReadFromJsonAsync<UserResponse>();
        Assert.Equal("diego@example.com", user!.Email);
    }

    [Fact]
    public async Task Register_WithDuplicateEmailInDifferentCase_ReturnsConflict()
    {
        var request = new RegisterUserRequest("Outra Ana", "ANA@Example.com", "Senha@123");

        var response = await _client.PostAsJsonAsync("/users", request);

        Assert.Equal(HttpStatusCode.Conflict, response.StatusCode);
    }

    [Fact]
    public async Task Register_WithShortPassword_ReturnsBadRequest()
    {
        var request = new RegisterUserRequest("Elisa Prado", "elisa@example.com", "1234567");

        var response = await _client.PostAsJsonAsync("/users", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Register_WithInvalidEmailFormat_ReturnsBadRequest()
    {
        var request = new RegisterUserRequest("Fabio Reis", "fabio-at-example.com", "Senha@123");

        var response = await _client.PostAsJsonAsync("/users", request);

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
