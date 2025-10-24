using LibraryAPI2.Domain;
using LibraryAPI2.Application.Exceptions;
using LibraryAPI2.Application.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace LibraryAPI2.Infrastructure;

public class LibraryRepository : ILibraryRepository
{
    private readonly LibraryContext _dbContext;

    public LibraryRepository(LibraryContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> AddAuthor(Author entry)
    {
        _dbContext.Authors.Add(entry);
        await _dbContext.SaveChangesAsync();
        return entry.Id;
    }

    public async Task DeleteAuthor(int id)
    {
        var author = await _dbContext.Authors.FindAsync(id) ?? throw new EntityDoesNotExistException(nameof(Author), id);
        _dbContext.Authors.Remove(author);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Author>> GetAllAuthors()
    {
        var authors = await _dbContext.Authors
            .Include(a => a.Books)
            .AsNoTracking()
            .ToListAsync();
        return authors;
    }

    public async Task<Author> GetAuthor(int id)
    {
        var author = await _dbContext.Authors
            .Include(a => a.Books)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
        return author ?? throw new EntityDoesNotExistException(nameof(Author), id);
    }

    public async Task UpdateAuthor(Author entry)
    {
        var author = await _dbContext.Authors.FindAsync(entry.Id) ?? throw new EntityDoesNotExistException(nameof(Author), entry.Id);

        author.Name = entry.Name;
        author.DateOfBirth = entry.DateOfBirth;

        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<int> AddBook(Book entry)
    {
        await MakeSureAuthorExistsOrThrow(entry.AuthorId);
        
        _dbContext.Books.Add(entry);
        await _dbContext.SaveChangesAsync();
        await _dbContext.Entry(entry)
            .Reference(b => b.Author)
            .LoadAsync();
        return entry.Id;
    }

    public async Task DeleteBook(int id)
    {
        var book = await _dbContext.Books.FindAsync(id) ?? throw new EntityDoesNotExistException(nameof(Book), id);
        _dbContext.Books.Remove(book);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<Book>> GetAllBooks()
    {
        var books = await _dbContext.Books
            .Include(b => b.Author)
            .AsNoTracking()
            .ToListAsync();
        return books;
    }

    public async Task<Book> GetBook(int id)
    {
        var book = await _dbContext.Books
            .Include(b => b.Author)
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id);
        return book ?? throw new EntityDoesNotExistException(nameof(Book), id);;
    }

    public async Task UpdateBook(Book entry)
    {
        var book = await _dbContext.Books.FindAsync(entry.Id) ?? throw new EntityDoesNotExistException(nameof(Book), entry.Id);
        
        await MakeSureAuthorExistsOrThrow(entry.AuthorId);

        book.Title = entry.Title;
        book.PublishedYear = entry.PublishedYear;
        book.AuthorId = entry.AuthorId;

        await _dbContext.SaveChangesAsync();
    }

    public async Task MakeSureAuthorExistsOrThrow(int authorId)
    {
        if (!await _dbContext.Authors.AnyAsync(a => a.Id == authorId)) throw new EntityDoesNotExistException(nameof(Author), authorId);
    }
}