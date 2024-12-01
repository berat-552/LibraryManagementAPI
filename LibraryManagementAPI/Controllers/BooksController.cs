using LibraryManagementAPI.Data;
using LibraryManagementAPI.Interfaces;
using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BooksController(LibraryContext context) : ControllerBase, IBooksController
{
    private readonly LibraryContext _context = context;

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
    {
        var books = await _context.Books.ToListAsync();
        if (books == null || books.Count == 0)
        {
            return NotFound();
        }

        return Ok(books);
    }

    [HttpGet("quantity")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooksByQuantity(int? quantity)
    {
        if (quantity <= 0)
        {
            return BadRequest();
        }

        var books = await _context.Books.ToListAsync();

        if (!quantity.HasValue)
        {
            return Ok(books);
        }

        if (quantity.Value > books.Count)
        {
            var partialQuantityBooks = books.Take(quantity.Value).ToList();
            return StatusCode(StatusCodes.Status206PartialContent, partialQuantityBooks);
        }

        var quantityWanted = books.Take(quantity.Value).ToList();
        return Ok(quantityWanted);
    }

    [HttpGet("{id}")]
    [Authorize]
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
    [Authorize]
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
    [Authorize]
    public async Task<IActionResult> UpdateBook(int id, [FromBody] Book book)
    {
        if (id != book.Id || !ModelState.IsValid)
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
    [Authorize]
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