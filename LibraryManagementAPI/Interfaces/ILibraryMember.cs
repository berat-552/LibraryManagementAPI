namespace LibraryManagementAPI.Interfaces;

public interface ILibraryMember
{
    Guid Id { get; set; }
    string Username { get; set; }
    string Password { get; set; }
    string Email { get; set; }
}
