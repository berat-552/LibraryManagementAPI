namespace LibraryManagementAPI.Models;

public class Book
{
    public int Id { get; set; }
    public string BookTitle { get; set; } = string.Empty;
    public string ISBN { get; set; } = string.Empty; // ISBN - International Standard Book Number
    public string Genre {  get; set; } = string.Empty;
    public DateTime PublishedDate { get; set; }
    public int AuthorId { get; set; }
    public Author Author { get; set; } = new Author();
    public ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
