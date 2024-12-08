﻿using LibraryManagementAPI.Controllers;
using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
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

    private const int InvalidAuthorId = 999;

    [Fact]
    public async Task GetAuthors_NoAuthors_ShouldReturnNotFound()
    {
        // Clear the database to simulate no authors
        _context.Authors.RemoveRange(_context.Authors);
        await _context.SaveChangesAsync();

        var response = await _controller.GetAuthors();
        var notFoundResult = Assert.IsType<NotFoundResult>(response.Result);

        Assert.Null(response.Value);
        Assert.IsNotType<List<Author>>(response.Value);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }

    [Fact]
    public async Task GetAuthorById_InvalidId_ShouldReturnNotFound()
    {
        var response = await _controller.GetAuthorById(InvalidAuthorId);
        var notFoundResult = Assert.IsType<NotFoundResult>(response.Result);

        Assert.Null(response.Value);
        Assert.IsNotType<Author>(response.Value);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }

    [Theory]
    [InlineData(-3)]
    [InlineData(0)]
    [InlineData(-250)]
    public async Task GetAuthorsByQuantity_InvalidQuantity_ReturnsBadRequest(int invalidQuantity)
    {
        var response = await _controller.GetAuthorsByQuantity(invalidQuantity);
        var badRequestResult = Assert.IsType<BadRequestResult>(response.Result);

        Assert.Null(response.Value);
        Assert.IsNotType<List<Author>>(response.Value);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task UpdateAuthor_InvalidFields_ShouldReturnBadRequest()
    {
        var authorToUpdate = SeedData.SeedAuthors().First();
        authorToUpdate.AuthorName = "";

        _controller.ModelState.AddModelError("AuthorName", "authorName is required");
        _controller.ModelState.AddModelError("AuthorName", "authorName must be between 1 and 100 characters");

        var response = await _controller.UpdateAuthor(authorToUpdate.Id, authorToUpdate);
        var badRequestResult = Assert.IsType<BadRequestResult>(response);

        Assert.NotNull(badRequestResult);
        Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
    }

    [Fact]
    public async Task DeleteAuthor_AuthorDoesntExist_ShouldReturnNotFound()
    {
        var response = await _controller.DeleteAuthor(InvalidAuthorId);
        var notFoundResult = Assert.IsType<NotFoundResult>(response);

        Assert.NotNull(notFoundResult);
        Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
    }
}
