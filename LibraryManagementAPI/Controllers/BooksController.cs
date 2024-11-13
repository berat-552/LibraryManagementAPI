using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BooksController(LibraryContext context) : ControllerBase
{
    private readonly LibraryContext _context = context;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
    {
        return await _context.Books.ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Book>> GetBookById(int id)
    {
        var book = await _context.Books.FindAsync(id);

        if (book == null)
        {
            return NotFound();
        }

        return Ok(book);
    }

    [HttpPost]
    public async Task<ActionResult<Book>> CreateBook([FromBody] Book book)
    {
        var existingBook = await _context.Books
       .FirstOrDefaultAsync(b => b.ISBN == book.ISBN);

        if (existingBook != null)
        {
            return Conflict(new { message = $"A book with the ISBN: {book.ISBN} already exists." });
        }

        _context.Books.Add(book);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetBookById), new { id = book.Id }, book);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] Book book)
    {
        if (id != book.Id)
        {
            return BadRequest();
        }

        // Detach the existing entity if it's already being tracked
        var existingBook = await _context.Books.FindAsync(id);
        if (existingBook != null)
        {
            _context.Entry(existingBook).State = EntityState.Detached;
        }

        _context.Entry(book).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return Ok(book);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            return NotFound();
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
