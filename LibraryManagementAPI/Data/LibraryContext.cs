using LibraryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Data
{
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

            if (!Database.IsInMemory())
            {
                // Seed data for Authors
                modelBuilder.Entity<Author>().HasData(
                    new Author
                    {
                        Id = 30,
                        AuthorName = "F. Scott Fitzgerald",
                        Biography = "F. Scott Fitzgerald was an American novelist, essayist, screenwriter, and short-story writer. He is widely regarded as one of the greatest American writers of the 20th century. His notable works include 'The Great Gatsby' and 'Tender Is the Night'."
                    },
                    new Author
                    {
                        Id = 29,
                        AuthorName = "George Orwell",
                        Biography = "George Orwell, born Eric Arthur Blair, was an English novelist, essayist, journalist, and critic. His work is marked by lucid prose, social criticism, opposition to totalitarianism, and outspoken support of democratic socialism. His notable works include '1984' and 'Animal Farm'."
                    }
                );

                // Seed data for Books
                modelBuilder.Entity<Book>().HasData(
                    new Book
                    {
                        Id = 20,
                        BookTitle = "The Great Gatsby",
                        ISBN = "9780743273565",
                        Genre = "Fiction",
                        PublishedDate = new DateTime(1925, 4, 10),
                        AuthorId = 3
                    },
                    new Book
                    {
                        Id = 25,
                        BookTitle = "1984",
                        ISBN = "9780451524935",
                        Genre = "Dystopian",
                        PublishedDate = new DateTime(1949, 6, 8),
                        AuthorId = 4
                    }
                );
            }
        }
    }
}
