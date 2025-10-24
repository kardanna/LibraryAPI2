using LibraryAPI2.Domain;

namespace LibraryAPI2.Application.Interfaces;

public interface ILibraryRepository
{
    public Task<IEnumerable<Book>> GetAllBooks();
    public Task<Book> GetBook(int id);
    public Task<int> AddBook(Book entry);
    public Task DeleteBook(int id);
    public Task UpdateBook(Book entry);
    public Task<IEnumerable<Author>> GetAllAuthors();
    public Task<Author> GetAuthor(int id);
    public Task<int> AddAuthor(Author entry);
    public Task DeleteAuthor(int id);
    public Task UpdateAuthor(Author entry);
}