using LibraryManagementAPI.Controllers;
using LibraryManagementAPI.Data;
using LibraryManagementAPI.Helpers;
using Microsoft.EntityFrameworkCore;
using Xunit.Abstractions;

namespace Tests.Controllers.LibraryMembersControllerTests;

public class LibraryMembersControllerFailTests
{
    private readonly LibraryMembersController _controller;
    private readonly AuthenticationHandler _authenticationHandler;
    private readonly LibraryContext _context;
    private readonly ITestOutputHelper _output; // Debug purposes

    public LibraryMembersControllerFailTests(AuthenticationHandler authenticationHandler, ITestOutputHelper output)
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
        _context.LibraryMembers.AddRange(SeedData.SeedLibraryMembers());
        _context.SaveChanges();

        // Initialize the controller with the test database context
        _authenticationHandler = authenticationHandler;
        _controller = new LibraryMembersController(_context, _authenticationHandler);
        _output = output;
    }
}
