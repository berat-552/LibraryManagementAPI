using LibraryManagementAPI.Models;
using LibraryManagementAPI.Responses;
using LibraryManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class BooksController(BookService bookService) : ControllerBase
{
    private readonly BookService _bookService = bookService;

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Book>>> GetBooks()
    {
        var books = await _bookService.GetAllBooks();
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

        var (books, partialData) = await _bookService.GetQuantityOfBooks(quantity);

        if (!partialData) return Ok(books);
        var response = new PartialResponse<Book>
        {
            PartialData = true,
            Items = books
        };

        return StatusCode(StatusCodes.Status200OK, response);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Book>> GetBookById(int id)
    {
        var book = await _bookService.GetBookById(id);

        if (book == null) return NotFound();

        return Ok(book);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Book>> CreateBook([FromBody] Book book)
    {
        var createdBook = await _bookService.CreateNewBook(book);

        if (createdBook == null)
        {
            return Conflict(new { message = $"A book with the ISBN: {book.Isbn} already exists." });
        }

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

        var updatedBook = await _bookService.UpdateExistingBook(id, book);
        if (updatedBook == null)
        {
            return BadRequest();
        }

        return Ok(updatedBook);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteBook(int id)
    {
        var book = await _bookService.DeleteExistingBook(id);
        if (book == null)
        {
            return NotFound();
        }

        return NoContent();
    }
}