﻿namespace LibraryManagementAPI.Models;

public class Author
{
    public int Id { get; set; }
    public string AuthorName { get; set; } = string.Empty;
    public string Biography { get; set; } = string.Empty;
    public ICollection<Book> Books { get; set; } = new List<Book>();
}