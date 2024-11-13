using Microsoft.EntityFrameworkCore;
using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Data;

public class LibraryContext(DbContextOptions<LibraryContext> options) : DbContext(options)
{
    public DbSet<Author> Authors { get; set; }
    public DbSet<Book> Books { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>()
            .HasMany(a => a.Books)
            .WithOne()
            .HasForeignKey(b => b.AuthorId);
    }
}
