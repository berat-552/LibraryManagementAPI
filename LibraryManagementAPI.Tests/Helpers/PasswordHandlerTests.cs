using LibraryManagementAPI.Helpers;

namespace Tests.Helpers;

public class PasswordHandlerTests
{
    private const string Password = "securePassword123";
    
    [Fact]
    public void HashPassword_ShouldReturnHashedPassword()
    {
        var hashedPassword = PasswordHandler.HashPassword(Password);

        Assert.False(string.IsNullOrEmpty(hashedPassword));
        Assert.NotEqual(Password, hashedPassword); // Ensure the hashed password is different from the original
    }

    [Fact]
    public void VerifyPassword_ShouldReturnTrue_ForValidPassword()
    {
        var hashedPassword = PasswordHandler.HashPassword(Password);
        var isValid = PasswordHandler.VerifyPassword(Password, hashedPassword);
        Assert.True(isValid);
    }

    [Fact]
    public void VerifyPassword_ShouldReturnFalse_ForInvalidPassword()
    {
        const string wrongPassword = "wrongPassword123";
        var hashedPassword = PasswordHandler.HashPassword(Password);

        var isValid = PasswordHandler.VerifyPassword(wrongPassword, hashedPassword);
        Assert.False(isValid);
    }
}
