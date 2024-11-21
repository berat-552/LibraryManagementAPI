using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Interfaces;

public interface IAuthorsController
{
    Task<ActionResult<IEnumerable<Author>>> GetAuthors();
    Task<ActionResult<Author>> GetAuthorById(int id);
    Task<ActionResult<Author>> GetAuthorsByQuantity(int? quantity);
    Task<ActionResult<Author>> CreateAuthor([FromBody] Author author);
    Task<IActionResult> UpdateAuthor(int id, [FromBody] Author author);
    Task<IActionResult> DeleteAuthor(int id);
}
