using LibraryManagementAPI.Controllers;
using LibraryManagementAPI.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Tests.Controllers.AuthorsControllerTests;

public class AuthorsControllerFailTests
{
    private readonly AuthorsController _controller;
    private readonly LibraryContext _context;
    private readonly ITestOutputHelper _output; // Debug purposes

    public AuthorsControllerFailTests(ITestOutputHelper output)
    {
        var dbName = Guid.NewGuid().ToString();

        // Configure in memory database options
        var options = new DbContextOptionsBuilder<LibraryContext>()
            .UseInMemoryDatabase(databaseName: dbName)
            .Options;

        // Initialize the database context with the in-memory options
        _context = new LibraryContext(options);

        // Clear the database before seeding it with test data
        _context.Database.EnsureDeleted();
        _context.Database.EnsureCreated();

        // Seed the in-memory database with test data
        _context.Authors.AddRange(SeedData.SeedAuthors());
        _context.SaveChanges();

        // Initialize the controller with the test database context
        _controller = new AuthorsController(_context);
        _output = output;
    }

    [Fact]
    public async Task GetAuthorById_InvalidId_ShouldReturnNotFound()
    {
        var invalidId = 999;
        var result = await _controller.GetAuthorById(invalidId);

        var notFoundResult = Assert.IsType<NotFoundResult>(result.Result);

        Assert.Null(result.Value);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
        // add more checks or modify
    }
}
