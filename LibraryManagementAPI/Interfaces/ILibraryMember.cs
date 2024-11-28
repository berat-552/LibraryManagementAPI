namespace LibraryManagementAPI.Interfaces;

public interface ILibraryMember
{
    int Id { get; set; }
    string Username { get; set; }
    string Password { get; set; }
    string Email { get; set; }
}
