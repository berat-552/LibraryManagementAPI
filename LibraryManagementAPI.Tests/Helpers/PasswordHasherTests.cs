using LibraryManagementAPI.Helpers;

namespace Tests.Helpers;

public class PasswordHasherTests
{
    [Fact]
    public void HashPassword_ShouldReturnHashedPassword()
    {
        string password = "securePassword123";
        string hashedPassword = PasswordHasher.HashPassword(password);

        Assert.False(string.IsNullOrEmpty(hashedPassword));
        Assert.NotEqual(password, hashedPassword); // Ensure the hashed password is different from the original
    }

    [Fact]
    public void VerifyPassword_ShouldReturnTrue_ForValidPassword()
    {
        string password = "securePassword123";
        string hashedPassword = PasswordHasher.HashPassword(password);

        bool isValid = PasswordHasher.VerifyPassword(password, hashedPassword);

        Assert.True(isValid);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnFalse_ForInvalidPassword()
    {
        string password = "securePassword123";
        string wrongPassword = "wrongPassword123";
        string hashedPassword = PasswordHasher.HashPassword(password);

        bool isValid = PasswordHasher.VerifyPassword(wrongPassword, hashedPassword);

        Assert.False(isValid);
    }
}
