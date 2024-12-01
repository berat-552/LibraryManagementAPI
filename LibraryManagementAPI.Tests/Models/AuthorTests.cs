using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace Tests.Models;

public class AuthorTests
{
    [Fact]
    public void Author_ShouldHaveDefaultValues()
    {
        var author = new Author();

        Assert.Equal(string.Empty, author.AuthorName);
        Assert.Equal(string.Empty, author.Biography);
        Assert.Equal([], author.Books);
    }

    [Fact]
    public void SeedAuthors_ShouldReturnListOfAuthors()
    {
        var count = 9;
        var authors = SeedData.SeedAuthors();

        Assert.NotNull(authors);
        Assert.Equal(count, authors.Count);
        Assert.IsType<List<Author>>(authors);
    }

    [Fact]
    public void Author_ShouldPassValidation()
    {
        var author = SeedData.SeedAuthors().First();

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(author, new ValidationContext(author), validationResults, true);

        Assert.True(isValid);
        Assert.Empty(validationResults);
    }

    [Fact]
    public void Author_ShouldFailValidation()
    {
        var author = new Author();

        var validationResults = new List<ValidationResult>();
        var isValid = Validator.TryValidateObject(author, new ValidationContext(author), validationResults, true);

        Assert.False(isValid);
        Assert.NotEmpty(validationResults);
    }
}