using System.Text.RegularExpressions;

namespace DemoApi.Users;

public static class EmailValidator
{
    private static readonly Regex EmailFormat = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

    public static bool IsValid(string? email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return true;
        }

        return EmailFormat.IsMatch(email);
    }
}
