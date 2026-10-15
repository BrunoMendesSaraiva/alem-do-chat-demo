namespace DemoApi.Auth;

public static class TokenSettings
{
    public const string Issuer = "alem-do-chat-demo";
    public const string Audience = "alem-do-chat-demo";
    public const string SigningKey = "demo-only-signing-key-change-me-0123456789abcdef";
    public const int ExpirationMinutes = 30;
}
