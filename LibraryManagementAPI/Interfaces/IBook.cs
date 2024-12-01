namespace LibraryManagementAPI.Interfaces;

public interface IBook
{
    int Id { get; set; }
    string BookTitle { get; set; }
    string ISBN { get; set; }
    string Genre { get; set; }
    DateTime PublishedDate { get; set; }
    int AuthorId { get; set; }
}

