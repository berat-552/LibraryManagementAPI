using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace Tests.Models;

public class LibraryMemberTests
{
    private static LibraryMember CreateLibraryMember(string username, string email, string password) => new()
    {
        Username = username,
        Email = email,
        Password = password
    };

    [Fact]
    public void LibraryMember_ShouldHaveDefaultValues()
    {
        var member = new LibraryMember();

        Assert.Equal(string.Empty, member.Username);
        Assert.Equal(string.Empty, member.Email);
        Assert.Equal(string.Empty, member.Password);
        Assert.Equal(0, member.Id);
    }

    [Fact]
    public void SeedLibraryMembers_ShouldReturnListOfLibraryMembers()
    {
        var count = 6;
        var members = SeedData.SeedLibraryMembers();

        Assert.NotNull(members);
        Assert.Equal(count, members.Count);
        Assert.IsType<List<LibraryMember>>(members);
    }

    [Theory]
    [InlineData("john_doe", "john.doe@example.com", "securePassword123!$#")]
    public void LibraryMember_WithValidData_ShouldPassValidation(string username, string email, string password)
    {
        var member = CreateLibraryMember(username, email, password);

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(member, new ValidationContext(member), validationResults, true);

        Assert.True(isValid);
        Assert.Empty(validationResults);
    }

    [Theory]
    [InlineData("", "john.doe@example.com", "securePassword123")] // Empty username
    [InlineData("john_doe", "", "securePassword123")] // Empty email
    [InlineData("john_doe", "john.doe@example.com", "")] // Empty password
    [InlineData("john_doe", "invalid-email", "securePassword123")] // Invalid email
    [InlineData("john_doe", "john.doe@example.com", "short")] // Password too short
    [InlineData("john_doe", "john.doe@example.com", "NoSpecialChar1")] // No special character
    [InlineData("john_doe", "john.doe@example.com", "nouppercase1!")] // No uppercase letter
    [InlineData("john_doe", "john.doe@example.com", "NOLOWERCASE1!")] // No lowercase letter
    [InlineData("john_doe", "john.doe@example.com", "NoNumber!")] // No number
    public void LibraryMember_WithInvalidData_ShouldFailValidation(string username, string email, string password)
    {
        var member = CreateLibraryMember(username, email, password);

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(member, new ValidationContext(member), validationResults, true);

        Assert.False(isValid);
        Assert.NotEmpty(validationResults);
    }
}
