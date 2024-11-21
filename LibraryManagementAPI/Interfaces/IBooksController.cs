using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Interfaces;

public interface IBooksController
{
    Task<ActionResult<IEnumerable<Book>>> GetBooks();
    Task<ActionResult<Book>> GetBookById(int id);
    Task<ActionResult<Book>> CreateBook([FromBody] Book book);
    Task<IActionResult> UpdateBook(int id, [FromBody] Book book);
    Task<IActionResult> DeleteBook(int id);
}
