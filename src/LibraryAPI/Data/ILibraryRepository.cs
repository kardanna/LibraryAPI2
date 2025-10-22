using LibraryAPI.Models;

namespace LibraryAPI.Data;

public interface ILibraryRepository
{
    public IEnumerable<Book> GetAllBooks();
    public Book GetBook(int id);
    public int AddBook(Book entry);
    public void DeleteBook(int id);
    public void UpdateBook(Book entry);
    public IEnumerable<Author> GetAllAuthors();
    public Author GetAuthor(int id);
    public int AddAuthor(Author entry);
    public void DeleteAuthor(int id);
    public void UpdateAuthor(Author entry);
}