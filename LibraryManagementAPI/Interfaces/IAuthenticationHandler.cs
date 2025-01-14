using LibraryManagementAPI.Models;

namespace LibraryManagementAPI.Interfaces;

public interface IAuthenticationHandler
{
    string GenerateJwtToken(LibraryMember user);
}
