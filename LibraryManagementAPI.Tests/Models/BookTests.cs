using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Tests.Models;

public class BookTests
{
    [Fact]
    public void Book_ShouldHaveDefaultValues()
    {
        var book = new Book();

        Assert.Equal(string.Empty, book.BookTitle);
        Assert.Equal(string.Empty, book.ISBN);
        Assert.Equal(string.Empty, book.Genre);
        Assert.Equal(default, book.PublishedDate);
        Assert.Equal(0, book.AuthorId);
    }

    [Fact]
    public void GetTestBooks_ShouldReturnListOfBooks()
    {
        var count = 3;
        var books = Book.GetTestBooks();

        Assert.NotNull(books);
        Assert.Equal(count, books.Count);
        Assert.IsType<List<Book>>(books);
    }
}
