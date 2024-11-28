using LibraryManagementAPI.Data;
using LibraryManagementAPI.Helpers;
using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class LibraryMembersController(LibraryContext context) : ControllerBase
{
    private readonly LibraryContext _context = context;

    [HttpGet("{id}")]
    public async Task<ActionResult<LibraryMember>> GetLibraryMemberById(int id)
    {
        var libraryMember = await _context.LibraryMembers.FindAsync(id);

        if (libraryMember == null)
        {
            return NotFound();
        }

        return Ok(libraryMember);
    }

    [HttpPost]
    public async Task<ActionResult<LibraryMember>> RegisterLibraryMember([FromBody] LibraryMember libraryMember)
    {
        var existingLibraryMember = _context.LibraryMembers.FirstOrDefault(member => member.Email == libraryMember.Email);

        if (existingLibraryMember != null)
        {
            return Conflict();
        }

        libraryMember.Password = PasswordHasher.HashPassword(libraryMember.Password);

        _context.LibraryMembers.Add(libraryMember);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLibraryMemberById), new { id = libraryMember.Id }, libraryMember);
    }

    // login endpoint

    [HttpPut("{id}")]
    public async Task<ActionResult<LibraryMember>> UpdateLibraryMember(int id, [FromBody] LibraryMember libraryMember)
    {
        var existingMember = await _context.LibraryMembers.FindAsync(id);
        if (existingMember == null)
        {
            return NotFound();
        }

        existingMember.Username = libraryMember.Username;
        existingMember.Email = libraryMember.Email;

         
         /* isPasswordChanged will be true if the passwords do not match (indicating that the password has changed)
         * If they are the same, set isPasswordChanged to false.
         * If they are different, set isPasswordChanged to true. */
        var isPasswordChanged = !PasswordHasher.VerifyPassword(libraryMember.Password, existingMember.Password);

        if (isPasswordChanged)
        {
            existingMember.Password = PasswordHasher.HashPassword(libraryMember.Password);
        }

        _context.LibraryMembers.Update(existingMember);
        await _context.SaveChangesAsync();

        return Ok(existingMember);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteLibraryMember(int id)
    {
        var member = await _context.LibraryMembers.FindAsync(id);
        if (member == null)
        {
            return NotFound();
        }

        _context.LibraryMembers.Remove(member);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
