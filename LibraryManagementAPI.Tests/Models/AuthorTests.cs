using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Tests.Models;

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
    public void GetTestAuthors_ShouldReturnListOfAuthors()
    {
        var count = 6;
        var authors = Author.GetTestAuthors();

        Assert.NotNull(authors);
        Assert.Equal(count, authors.Count);
        Assert.IsType<List<Author>>(authors);
    }
}
