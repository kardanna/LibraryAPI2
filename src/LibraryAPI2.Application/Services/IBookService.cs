using LibraryAPI2.Application.DTO.Book;

namespace LibraryAPI2.Application.Services;

public interface IBookService
{
    public IEnumerable<ReturnBookDto> GetAllBooks();
    public ReturnBookDto GetBook(int id);
    public ReturnBookDto AddBook(CreateBookDto entry);
    public void DeleteBook(int id);
    public void UpdateBook(int id, CreateBookDto entry);
}