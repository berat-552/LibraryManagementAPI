using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Interfaces;

public interface ILibraryMembersController
{
    Task<ActionResult<LibraryMember>> GetLibraryMemberById(int id);
    Task<ActionResult<LibraryMember>> RegisterLibraryMember([FromBody] LibraryMember libraryMember);
    Task<ActionResult> LoginLibraryMember([FromBody] LoginModel loginModel);
    Task<ActionResult<LibraryMember>> UpdateLibraryMember(int id, [FromBody] LibraryMember libraryMember);
    Task<IActionResult> DeleteLibraryMember(int id);
}
