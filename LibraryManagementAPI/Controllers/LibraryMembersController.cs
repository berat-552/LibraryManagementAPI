using LibraryManagementAPI.Data;
using LibraryManagementAPI.Helpers;
using LibraryManagementAPI.Interfaces;
using LibraryManagementAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibraryManagementAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class LibraryMembersController(LibraryContext context, AuthenticationHandler authenticationHandler) : ControllerBase, ILibraryMembersController
{
    private readonly LibraryContext _context = context;
    private readonly AuthenticationHandler _authenticationHandler = authenticationHandler;

    [HttpGet("{id}")]
    [Authorize]
    public async Task<ActionResult<LibraryMember>> GetLibraryMemberById(int id)
    {
        var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var isNotAuthorized = authenticatedUserId == null || authenticatedUserId != id.ToString();

        if (isNotAuthorized)
        {
            return Forbid();
        }

        var libraryMember = await _context.LibraryMembers.FindAsync(id);
        if (libraryMember == null)
        {
            return NotFound();
        }

        return Ok(libraryMember);
    }

    [HttpPost("register")]
    public async Task<ActionResult<LibraryMember>> RegisterLibraryMember([FromBody] LibraryMember libraryMember)
    {
        var existingLibraryMember = _context.LibraryMembers.FirstOrDefault(member => member.Email == libraryMember.Email);

        if (existingLibraryMember != null)
        {
            return Conflict();
        }

        libraryMember.Password = PasswordHandler.HashPassword(libraryMember.Password);

        _context.LibraryMembers.Add(libraryMember);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetLibraryMemberById), new { id = libraryMember.Id }, libraryMember);
    }

    [HttpPost("login")]
    public async Task<ActionResult> LoginLibraryMember([FromBody] LoginModel loginModel)
    {
        var libraryMember = await _context.LibraryMembers.FirstOrDefaultAsync(member => member.Email == loginModel.Email);

        if (libraryMember == null || !PasswordHandler.VerifyPassword(loginModel.Password, libraryMember.Password))
        {
            return Unauthorized();
        }

        var token = _authenticationHandler.GenerateJWTToken(libraryMember);
        return Ok(new { Token = token });
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<ActionResult<LibraryMember>> UpdateLibraryMember(int id, [FromBody] LibraryMember libraryMember)
    {
        var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var isNotAuthorized = authenticatedUserId == null || authenticatedUserId != id.ToString();

        if (isNotAuthorized)
        {
            return Forbid();
        }

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
        var isPasswordChanged = !PasswordHandler.VerifyPassword(libraryMember.Password, existingMember.Password);

        if (isPasswordChanged)
        {
            existingMember.Password = PasswordHandler.HashPassword(libraryMember.Password);
        }

        _context.LibraryMembers.Update(existingMember);
        await _context.SaveChangesAsync();

        return Ok(existingMember);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> DeleteLibraryMember(int id)
    {
        var authenticatedUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var isNotAuthorized = authenticatedUserId == null || authenticatedUserId != id.ToString();

        if (isNotAuthorized)
        {
            return Forbid();
        }

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