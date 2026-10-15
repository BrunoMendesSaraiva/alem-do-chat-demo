using DemoApi.Auth;
using DemoApi.Profile;
using DemoApi.Users;
using Microsoft.AspNetCore.Authentication.JwtBearer;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSingleton<InMemoryUserRepository>();
builder.Services.AddSingleton<TokenService>();

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = TokenService.CreateValidationParameters();
    });
builder.Services.AddAuthorization();

var app = builder.Build();

app.UseAuthentication();
app.UseAuthorization();

app.MapUserEndpoints();
app.MapLoginEndpoints();
app.MapProfileEndpoints();

app.Run();

public partial class Program { }
