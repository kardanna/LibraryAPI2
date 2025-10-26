using System.Linq.Expressions;
using LibraryAPI2.Domain;

namespace LibraryAPI2.Application.Interfaces;

public interface ILibraryRepository
{
    public Task<IEnumerable<Book>> GetAllBooks(Expression<Func<Book, bool>>? filteringPredicate, Func<IQueryable<Book>, IOrderedQueryable<Book>>? orderingDelegate);
    public Task<Book> GetBook(int id);
    public Task<int> AddBook(Book entry);
    public Task DeleteBook(int id);
    public Task UpdateBook(Book entry);
    public Task<IEnumerable<Author>> GetAllAuthors(Expression<Func<Author, bool>>? filteringPredicate, Func<IQueryable<Author>, IOrderedQueryable<Author>>? orderingDelegate);
    public Task<Author> GetAuthor(int id);
    public Task<int> AddAuthor(Author entry);
    public Task DeleteAuthor(int id);
    public Task UpdateAuthor(Author entry);
}