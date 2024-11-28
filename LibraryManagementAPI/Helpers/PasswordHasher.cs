using Bcrypt = BCrypt.Net.BCrypt;

namespace LibraryManagementAPI.Helpers;

public static class PasswordHasher
{
    public static string HashPassword(string password) => Bcrypt.HashPassword(password);
    public static bool VerifyPassword(string password, string hashedPassword) => Bcrypt.Verify(password, hashedPassword);
}