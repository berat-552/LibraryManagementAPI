using LibraryManagementAPI.Controllers;
using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace LibraryManagementAPI.Tests;

public class AuthorsControllerTests
{
    private readonly AuthorsController _controller;
    private readonly LibraryContext _context;
    private readonly ITestOutputHelper _output; // Debug purposes

    public AuthorsControllerTests(ITestOutputHelper output)
    {
        // Configure in memory database options
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(databaseName: "TestDatabase")
            .Options;

        // Initialize the database context with the in-memory options
        _context = new LibraryContext(options);

        // Clear the database before seeding it with test data
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        // Seed the in-memory database with test data
        _context.Authors.AddRange(Author.GetTestAuthors());
        _context.SaveChanges();

        // Initialize the controller with the test database context
        _controller = new AuthorsController(_context);
        _output = output;
    }

    [Fact]
    public async Task GetAuthors_ReturnsAllAuthors()
    {
        var totalAuthorsCount = 6;
        var result = await _controller.GetAuthors();
        var authors = Assert.IsType<List<Author>>(result.Value);

        Assert.NotEmpty(authors);
        Assert.Equal(totalAuthorsCount, authors.Count);
    }

    [Fact]
    public async Task GetAuthorsByQuantity_ReturnsWantedQuantityOfAuthor()
    {
        var quantity = 4;
        var result = await _controller.GetAuthorsByQuantity(quantity);

        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var authors = Assert.IsType<List<Author>>(okResult.Value);

        Assert.NotEmpty(authors);
        Assert.Equal(quantity, authors.Count);
    }

    [Fact]
    public async Task GetAuthorById_ReturnsAuthor()
    {
        var foundAuthor = await _controller.GetAuthorById(1);

        Assert.NotNull(foundAuthor);
        Assert.NotEmpty(_context.Authors);
    }

    [Fact]
    public async Task UpdateAuthor_ReturnsOkResult_WhenAuthorIsUpdated()
    {
        var authorToUpdate = Author.GetTestAuthors().First();
        var result = await _controller.UpdateAuthor(1, authorToUpdate);

        var okResult = Assert.IsType<OkObjectResult>(result);
        var updatedAuthor = Assert.IsType<Author>(okResult.Value);

        Assert.Equal(authorToUpdate.Id, updatedAuthor.Id);
        Assert.Equal(authorToUpdate.AuthorName, updatedAuthor.AuthorName);
    }

    [Fact]
    public async Task DeleteAuthor_ReturnsNoContentResult_WhenAuthorIsDeleted()
    {
        var authorToDelete = Author.GetTestAuthors().First();

        var result = await _controller.DeleteAuthor(authorToDelete.Id);
        var noContentResult = Assert.IsType<NoContentResult>(result);
        var deletedAuthor = await _context.Authors.FindAsync(authorToDelete.Id);

        Assert.NotNull(noContentResult);
        Assert.Null(deletedAuthor);
    }
}