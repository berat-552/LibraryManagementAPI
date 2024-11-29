using LibraryManagementAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace Tests.Models;

public class LoginModelTests
{
    private static LoginModel CreateLoginModel(string email, string password)
    {
        return new LoginModel
        {
            Email = email,
            Password = password
        };
    }

    [Fact]
    public void LoginModel_ShouldHaveDefaultValues()
    {
        var loginModel = new LoginModel();

        Assert.Equal(string.Empty, loginModel.Email);
        Assert.Equal(string.Empty, loginModel.Password);
    }

    [Theory]
    [InlineData("john.doe@gmail.com", "password321")]
    public void LoginModel_WithValidData_ShouldPassValidation(string email, string password)
    {
        var loginModel = CreateLoginModel(email, password);

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(loginModel, new ValidationContext(loginModel), validationResults, true);

        Assert.True(isValid);
        Assert.Empty(validationResults);
    }

    [Theory]
    [InlineData("", "securePassword123")] // Empty email
    [InlineData("john.doe@example.com", "")] // Empty password
    [InlineData("invalid-email", "securePassword123")] // Invalid email
    public void LibraryMember_WithInvalidData_ShouldFailValidation(string email, string password)
    {
        var loginModel = CreateLoginModel(email, password);

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(loginModel, new ValidationContext(loginModel), validationResults, true);

        Assert.False(isValid);
        Assert.NotEmpty(validationResults);
    }
}
