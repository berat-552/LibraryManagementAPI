using LibraryManagementAPI.Data;
using LibraryManagementAPI.Helpers;
using LibraryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Services;

public class LibraryMemberService(AppDbContext context, AuthenticationHandler authenticationHandler)
{
    private readonly AppDbContext _context = context;
    private readonly AuthenticationHandler _authenticationHandler = authenticationHandler;

    public async Task<LibraryMember?> GetLibraryMemberById(int id)
    {
        var libraryMember = await _context.LibraryMembers.FindAsync(id);
        if (libraryMember == null)
        {
            return null;
        }

        return libraryMember;
    }

    public async Task<LibraryMember?> RegisterNewLibraryMember(LibraryMember libraryMember)
    {
        var existingLibraryMember = _context.LibraryMembers.FirstOrDefault(member => member.Email == libraryMember.Email);

        if (existingLibraryMember != null)
        {
            return null;
        }

        libraryMember.Password = PasswordHandler.HashPassword(libraryMember.Password);

        _context.LibraryMembers.Add(libraryMember);
        await _context.SaveChangesAsync();

        return libraryMember;
    }

    public async Task<string?> LoginLibraryMember(LoginModel loginModel)
    {
        var libraryMember = await _context.LibraryMembers.FirstOrDefaultAsync(member => member.Email == loginModel.Email);

        if (libraryMember == null || !PasswordHandler.VerifyPassword(loginModel.Password, libraryMember.Password))
        {
            return null; // Indicate unauthorized
        }

        var token = _authenticationHandler.GenerateJWTToken(libraryMember);
        return token;
    }

    public async Task<LibraryMember?> UpdateExistingLibraryMember(int id, LibraryMember libraryMember)
    {
        var existingMember = await _context.LibraryMembers.FindAsync(id);
        if (existingMember == null)
        {
            return null; // Indicate not found
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

        return existingMember;
    }

    public async Task<LibraryMember?> DeleteExistingLibraryMember(int id)
    {
        var member = await _context.LibraryMembers.FindAsync(id);
        if (member == null)
        {
            return null;
        }

        _context.LibraryMembers.Remove(member);
        await _context.SaveChangesAsync();

        return member;
    }
}
