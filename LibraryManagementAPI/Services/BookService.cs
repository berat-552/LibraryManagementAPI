using LibraryManagementAPI.Data;
using LibraryManagementAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryManagementAPI.Services;

public class BookService(LibraryContext context)
{
    private readonly LibraryContext _context = context;

    public async Task<List<Book>> GetAllBooks() => await _context.Books.ToListAsync();

    public async Task<(List<Book> Books, bool PartialData)> GetQuantityOfBooks(int? quantity)
    {
        var books = await _context.Books.ToListAsync();

        if (!quantity.HasValue)
        {
            return (books, false);
        }

        if (quantity.Value > books.Count)
        {
            var partialQuantityBooks = books.Take(quantity.Value).ToList();
            return (partialQuantityBooks, true);
        }

        var quantityWanted = books.Take(quantity.Value).ToList();
        return (quantityWanted, false);
    }

    public async Task<Book> GetBookById(int id)
    {
        var book = await _context.Books.FindAsync(id);
        return book!;
    }

    public async Task<Book?> CreateNewBook(Book book)
    {
        var existingBook = await _context.Books
            .FirstOrDefaultAsync(b => b.ISBN == book.ISBN);

        if (existingBook != null)
        {
            return null;
        }

        _context.Books.Add(book);
        await _context.SaveChangesAsync();
        return book;
    }

    public async Task<Book?> UpdateExistingBook(int id, Book book)
    {
        // Detach the existing entity if it's already being tracked
        var existingBook = await _context.Books.FindAsync(id);

        if (existingBook == null)
        {
            return null;
        }

        _context.Entry(existingBook).State = EntityState.Detached;
        _context.Entry(book).State = EntityState.Modified;
        await _context.SaveChangesAsync();

        return book;
    }

    public async Task<Book?> DeleteExistingBook(int id)
    {
        var book = await _context.Books.FindAsync(id);
        if (book == null)
        {
            return null;
        }

        _context.Books.Remove(book);
        await _context.SaveChangesAsync();
        return book;
    }
}
