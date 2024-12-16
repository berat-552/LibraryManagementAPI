using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Services;

public class AuthorService(LibraryContext context)
{
    private readonly LibraryContext _context = context;

    public async Task<List<Author>> GetAllAuthors() => await _context.Authors.ToListAsync();

    public async Task<(List<Author> Authors, bool PartialData)> GetQuantityOfAuthors(int? quantity)
    {
        var authors = await _context.Authors.ToListAsync();

        if (!quantity.HasValue)
        {
            return (authors, false);
        }

        if (quantity.Value > authors.Count)
        {
            var partialQuantityAuthors = authors.Take(quantity.Value).ToList();
            return (partialQuantityAuthors, true);
        }

        var quantityWanted = authors.Take(quantity.Value).ToList();
        return (quantityWanted, false);
    }

    public async Task<Author> GetAuthorById(int id)
    {
        var author = await _context.Authors.FindAsync(id);
        return author!;
    }

    public async Task<Author?> CreateNewAuthor(Author author)
    {
        _context.Authors.Add(author);
        await _context.SaveChangesAsync();
        return author;
    }

    public async Task<Author> UpdateExistingAuthor(int id, Author author)
    {
        // Detach the existing entity if it's already being tracked
        var existingAuthor = await _context.Authors.FindAsync(id);
        if (existingAuthor != null)
        {
            _context.Entry(existingAuthor).State = EntityState.Detached;
        }

        _context.Entry(author).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return author;
    }

    public async Task<Author?> DeleteExistingAuthor(int id)
    {
        var author = await _context.Authors.FindAsync(id);

        if (author == null)
        {
            return null;
        }

        _context.Authors.Remove(author);
        await _context.SaveChangesAsync();
        return author;
    }
}
