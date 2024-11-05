using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthorsController : ControllerBase
{
    private readonly LibraryContext _context;

    public AuthorsController(LibraryContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
    {
        return await _context.Authors.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Author>> CreateAuthor(Author author)
    {
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAuthorById), new { id = author.Id }, author);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Author>> GetAuthorById(int id)
    {
        var author = await _context.Authors.FindAsync(id);

        if (author == null)
        {
            return NotFound();
        }

        return Ok(author);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAuthor(int id, Author author)
    {
        if (id != author.Id)
        {
            return BadRequest();
        }

        _context.Entry(author).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return Ok(author);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        var author = await _context.Authors.FindAsync(id);
        if (author == null)
        {
            return NotFound();
        }

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
