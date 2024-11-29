using LibraryManagementAPI.Data;
using LibraryManagementAPI.Interfaces;
using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AuthorsController(LibraryContext context) : ControllerBase, IAuthorsController
{
    private readonly LibraryContext _context = context;

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
    {
        var authors = await _context.Authors.ToListAsync();
        if (authors == null || authors.Count == 0)
        {
            return NotFound();
        }

        return Ok(authors);
    }

    [HttpGet("quantity")]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Author>>> GetAuthorsByQuantity(int? quantity)
    {
        if (quantity <= 0)
        {
            return BadRequest();
        }

        var authors = await _context.Authors.ToListAsync();

        if (!quantity.HasValue)
        {
            return Ok(authors);
        }

        if (quantity.Value > authors.Count)
        {
            var partialQuantityAuthors = authors.Take(quantity.Value).ToList();
            return StatusCode(StatusCodes.Status206PartialContent, partialQuantityAuthors);
        }

        var quantityWanted = authors.Take(quantity.Value).ToList();
        return Ok(quantityWanted);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Author>> GetAuthorById(int id)
    {
        var author = await _context.Authors.FindAsync(id);

        if (author == null)
        {
            return NotFound();
        }

        return Ok(author);
    }

    [HttpPost]
    [Authorize]
    public async Task<ActionResult<Author>> CreateAuthor([FromBody] Author author)
    {
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAuthorById), new { id = author.Id }, author);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateAuthor(int id, [FromBody] Author author)
    {
        if (id != author.Id || !ModelState.IsValid)
        {
            return BadRequest();
        }

        // Detach the existing entity if it's already being tracked
        var existingAuthor = await _context.Authors.FindAsync(id);
        if (existingAuthor != null)
        {
            _context.Entry(existingAuthor).State = EntityState.Detached;
        }

        _context.Entry(author).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return Ok(author);
    }

    [HttpDelete("{id}")]
    [Authorize]
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
