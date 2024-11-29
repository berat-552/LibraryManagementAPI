using LibraryManagementAPI.Helpers;

namespace Tests.Helpers;

public class PasswordHandlerTests
{
    [Fact]
    public void HashPassword_ShouldReturnHashedPassword()
    {
        string password = "securePassword123";
        string hashedPassword = PasswordHandler.HashPassword(password);

        Assert.False(string.IsNullOrEmpty(hashedPassword));
        Assert.NotEqual(password, hashedPassword); // Ensure the hashed password is different from the original
    }

    [Fact]
    public void VerifyPassword_ShouldReturnTrue_ForValidPassword()
    {
        string password = "securePassword123";
        string hashedPassword = PasswordHandler.HashPassword(password);

        bool isValid = PasswordHandler.VerifyPassword(password, hashedPassword);
        Assert.True(isValid);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnFalse_ForInvalidPassword()
    {
        string password = "securePassword123";
        string wrongPassword = "wrongPassword123";
        string hashedPassword = PasswordHandler.HashPassword(password);

        bool isValid = PasswordHandler.VerifyPassword(wrongPassword, hashedPassword);
        Assert.False(isValid);
    }
}
