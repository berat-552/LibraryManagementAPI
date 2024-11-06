namespace LibraryManagementAPI.Models;

public class Author
{
    public int Id { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string Biography { get; set; } = string.Empty;
    public ICollection<Book> Books { get; set; } = new List<Book>();

    public static List<Author> GetTestAuthors() => [
                new Author
                {
                    Id = 1,
                    AuthorName = "Jane Austen",
                    Biography = "Jane Austen was an English novelist known primarily for her six major novels, which interpret, critique, and comment upon the British landed gentry at the end of the 18th century. Her most notable works include 'Pride and Prejudice', 'Sense and Sensibility', and 'Emma'."
                },
                new Author
                {
                    Id = 2,
                    AuthorName = "Mark Twain",
                    Biography = "Mark Twain, born Samuel Langhorne Clemens, was an American writer, humorist, entrepreneur, publisher, and lecturer. He is best known for his novels 'The Adventures of Tom Sawyer' and its sequel, 'Adventures of Huckleberry Finn'."
                },
                new Author
                {
                    Id = 3,
                    AuthorName = "Charles Dickens",
                    Biography = "Charles Dickens was an English writer and social critic. He created some of the world's best-known fictional characters and is regarded by many as the greatest novelist of the Victorian era. His notable works include 'A Tale of Two Cities', 'Great Expectations', and 'David Copperfield'."
                },
                new Author
                {
                    Id = 4,
                    AuthorName = "Jane Austen",
                    Biography = "Jane Austen was an English novelist known primarily for her six major novels, which interpret, critique, and comment upon the British landed gentry at the end of the 18th century. Her notable works include 'Pride and Prejudice', 'Sense and Sensibility', and 'Emma'."
                },
                new Author
                {
                    Id = 5,
                    AuthorName = "Mark Twain",
                    Biography = "Mark Twain, born Samuel Langhorne Clemens, was an American writer, humorist, entrepreneur, publisher, and lecturer. He is best known for his novels 'The Adventures of Tom Sawyer' and its sequel, 'Adventures of Huckleberry Finn', the latter often called 'The Great American Novel'."
                },
                new Author
                {
                    Id = 6,
                    AuthorName = "George Orwell",
                    Biography = "George Orwell, born Eric Arthur Blair, was an English novelist, essayist, journalist, and critic. His work is marked by lucid prose, social criticism, opposition to totalitarianism, and outspoken support of democratic socialism. His notable works include '1984' and 'Animal Farm'."
                }
            ];
}