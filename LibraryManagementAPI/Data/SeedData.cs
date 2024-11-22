using LibraryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Data;

public static class SeedData
{
    public static void Seed(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Author>().HasData(SeedAuthors());
        modelBuilder.Entity<Book>().HasData(SeedBooks());
    }

    public static List<Author> SeedAuthors()
    {
        return [
            new Author
            {
                Id = 31,
                AuthorName = "J.K. Rowling",
                Biography = "J.K. Rowling is a British author, best known for writing the Harry Potter fantasy series. The books have gained worldwide attention, won multiple awards, and sold more than 500 million copies."
            },
            new Author
            {
                Id = 32,
                AuthorName = "Ernest Hemingway",
                Biography = "Ernest Hemingway was an American novelist, short-story writer, and journalist. His economical and understated style had a strong influence on 20th-century fiction. His notable works include 'The Old Man and the Sea' and 'A Farewell to Arms'."
            },
            new Author
            {
                Id = 33,
                AuthorName = "Agatha Christie",
                Biography = "Agatha Christie was an English writer known for her 66 detective novels and 14 short story collections, particularly those revolving around her fictional detectives Hercule Poirot and Miss Marple."
            },
            new Author
            {
                Id = 34,
                AuthorName = "J.R.R. Tolkien",
                Biography = "J.R.R. Tolkien was an English writer, poet, philologist, and academic, best known for his high fantasy works 'The Hobbit' and 'The Lord of the Rings'."
            },
            new Author
            {
                Id = 35,
                AuthorName = "F. Scott Fitzgerald",
                Biography = "F. Scott Fitzgerald was an American novelist, essayist, screenwriter, and short-story writer. He is widely regarded as one of the greatest American writers of the 20th century. His notable works include 'The Great Gatsby' and 'Tender Is the Night'."
            },
            new Author
            {
                Id = 36,
                AuthorName = "George Orwell",
                Biography = "George Orwell, born Eric Arthur Blair, was an English novelist, essayist, journalist, and critic. His work is marked by lucid prose, social criticism, opposition to totalitarianism, and outspoken support of democratic socialism. His notable works include '1984' and 'Animal Farm'."
            },
            new Author
            {
                Id = 37,
                AuthorName = "Jane Austen",
                Biography = "Jane Austen was an English novelist known primarily for her six major novels, which interpret, critique, and comment upon the British landed gentry at the end of the 18th century. Her most notable works include 'Pride and Prejudice', 'Sense and Sensibility', and 'Emma'."
            },
            new Author
            {
                Id = 38,
                AuthorName = "Mark Twain",
                Biography = "Mark Twain, born Samuel Langhorne Clemens, was an American writer, humorist, entrepreneur, publisher, and lecturer. He is best known for his novels 'The Adventures of Tom Sawyer' and its sequel, 'Adventures of Huckleberry Finn'."
            },
            new Author
            {
                Id = 39,
                AuthorName = "Charles Dickens",
                Biography = "Charles Dickens was an English writer and social critic. He created some of the world's best-known fictional characters and is regarded by many as the greatest novelist of the Victorian era. His notable works include 'A Tale of Two Cities', 'Great Expectations', and 'David Copperfield'."
            }
        ];
    }

    public static List<Book> SeedBooks()
    {
        return [
            new Book
            {
                Id = 21,
                BookTitle = "Harry Potter and the Philosopher's Stone",
                ISBN = "9780747532699",
                Genre = "Fantasy",
                PublishedDate = new DateTime(1997, 6, 26),
                AuthorId = 31
            },
            new Book
            {
                Id = 22,
                BookTitle = "The Old Man and the Sea",
                ISBN = "9780684830490",
                Genre = "Fiction",
                PublishedDate = new DateTime(1952, 9, 1),
                AuthorId = 32
            },
            new Book
            {
                Id = 23,
                BookTitle = "Murder on the Orient Express",
                ISBN = "9780007119318",
                Genre = "Mystery",
                PublishedDate = new DateTime(1934, 1, 1),
                AuthorId = 33
            },
            new Book
            {
                Id = 24,
                BookTitle = "The Hobbit",
                ISBN = "9780547928227",
                Genre = "Fantasy",
                PublishedDate = new DateTime(1937, 9, 21),
                AuthorId = 34
            },
            new Book
            {
                Id = 25,
                BookTitle = "The Great Gatsby",
                ISBN = "9780743273565",
                Genre = "Fiction",
                PublishedDate = new DateTime(1925, 4, 10),
                AuthorId = 35
            },
            new Book
            {
                Id = 26,
                BookTitle = "1984",
                ISBN = "9780451524935",
                Genre = "Dystopian",
                PublishedDate = new DateTime(1949, 6, 8),
                AuthorId = 36
            },
            new Book
            {
                Id = 27,
                BookTitle = "Pride and Prejudice",
                ISBN = "9781503290563",
                Genre = "Romance",
                PublishedDate = new DateTime(1813, 1, 28),
                AuthorId = 37
            },
            new Book
            {
                Id = 28,
                BookTitle = "The Adventures of Tom Sawyer",
                ISBN = "9780486400778",
                Genre = "Adventure",
                PublishedDate = new DateTime(1876, 12, 1),
                AuthorId = 38
            },
            new Book
            {
                Id = 29,
                BookTitle = "A Tale of Two Cities",
                ISBN = "9781853260391",
                Genre = "Historical Fiction",
                PublishedDate = new DateTime(1859, 4, 30),
                AuthorId = 39
            }
        ];
    }
}