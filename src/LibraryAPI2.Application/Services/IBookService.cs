using LibraryAPI2.Application.DTO.Book;

namespace LibraryAPI2.Application.Services;

public interface IBookService
{
    public Task<IEnumerable<ReturnBookDto>> GetAllBooks(BookQueryParameters? query);
    public Task<ReturnBookDto> GetBook(int id);
    public Task<ReturnBookDto> AddBook(CreateBookDto entry);
    public Task DeleteBook(int id);
    public Task UpdateBook(int id, CreateBookDto entry);
}