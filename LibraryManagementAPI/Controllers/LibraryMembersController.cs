using LibraryManagementAPI.Models;
using LibraryManagementAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryManagementAPI.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class LibraryMembersController(LibraryMemberService libraryMemberService) : ControllerBase
{
    private readonly LibraryMemberService _libraryMemberService = libraryMemberService;


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

        var libraryMember = await _libraryMemberService.GetLibraryMemberById(id);
        if (libraryMember == null)
        {
            return NotFound();
        }

        return Ok(libraryMember);
    }

    [HttpPost("register")]
    public async Task<ActionResult<LibraryMember>> RegisterLibraryMember([FromBody] LibraryMember libraryMember)
    {
        var newLibraryMember = await _libraryMemberService.RegisterNewLibraryMember(libraryMember);

        if (newLibraryMember == null)
        {
            return Conflict();
        }

        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        return CreatedAtAction(nameof(GetLibraryMemberById), new { id = libraryMember.Id }, libraryMember);
    }

    [HttpPost("login")]
    public async Task<ActionResult> LoginLibraryMember([FromBody] LoginModel loginModel)
    {
        var token = await _libraryMemberService.LoginLibraryMember(loginModel);

        if (token == null)
        {
            return Unauthorized();
        }

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

        var updatedMember = await _libraryMemberService.UpdateExistingLibraryMember(id, libraryMember);

        if (updatedMember == null)
        {
            return NotFound();
        }

        return Ok(updatedMember);
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

        var member = await _libraryMemberService.DeleteExistingLibraryMember(id);
        if (member == null)
        {
            return NotFound();
        }

        return NoContent();
    }
}