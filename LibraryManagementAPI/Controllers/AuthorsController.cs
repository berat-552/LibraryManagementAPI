using LibraryManagementAPI.Models;
using LibraryManagementAPI.Responses;
using LibraryManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public sealed class AuthorsController(AuthorService authorService) : ControllerBase
{
    private readonly AuthorService _authorService = authorService;

    [HttpGet]
    [Authorize]
    public async Task<ActionResult<IEnumerable<Author>>> GetAuthors()
    {
        var authors = await _authorService.GetAllAuthors();
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

        var (authors, partialData) = await _authorService.GetQuantityOfAuthors(quantity);

        if (partialData)
        {
            var response = new PartialResponse<Author>
            {
                PartialData = true,
                Items = authors
            };

            return StatusCode(StatusCodes.Status200OK, response);
        }

        return Ok(authors);
    }

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<Author>> GetAuthorById(int id)
    {
        var author = await _authorService.GetAuthorById(id);

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
        if (author == null || !ModelState.IsValid)
        {
            return BadRequest();
        }
        var createdAuthor = await _authorService.CreateNewAuthor(author);

        if (createdAuthor == null)
        {
            return BadRequest();
        }

        return CreatedAtAction(nameof(GetAuthorById), new { id = createdAuthor.Id }, createdAuthor);
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> UpdateAuthor(int id, [FromBody] Author author)
    {
        if (id != author.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var updatedAuthor = await _authorService.UpdateExistingAuthor(id, author);
        if (updatedAuthor == null)
        {
            return BadRequest();
        }

        return Ok(updatedAuthor);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteAuthor(int id)
    {
        var author = await _authorService.DeleteExistingAuthor(id);
        if (author == null)
        {
            return NotFound();
        }

        return NoContent();
    }
}