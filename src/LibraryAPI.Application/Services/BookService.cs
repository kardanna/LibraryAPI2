using FluentValidation;
using LibraryAPI.Application.DTO.Book;
using LibraryAPI.Application.Interfaces;
using LibraryAPI.Application.Validators.Book;
using LibraryAPI.Domain;

namespace LibraryAPI.Application.Services;

public class BookService : IBookService
{
    private readonly ILibraryRepository _repository;

    public BookService(ILibraryRepository repository)
    {
        _repository = repository;
    }

    public ReturnBookDto AddBook(CreateBookDto entry)
    {
        var validator = new CreateBookDtoValidator();
        validator.ValidateAndThrow(entry);

        var book = new Book()
        {
            Title = entry.Title,
            PublishedYear = entry.PublishedYear,
            AuthorId = entry.AuthorId
        };
        
        _repository.AddBook(book);

        return new ReturnBookDto(book);
    }

    public void DeleteBook(int id)
    {
        _repository.DeleteBook(id);
    }

    public IEnumerable<ReturnBookDto> GetAllBooks()
    {
        var books = _repository.GetAllBooks().Select(b => new ReturnBookDto(b));
        return books;
    }

    public ReturnBookDto GetBook(int id)
    {
        return new ReturnBookDto(_repository.GetBook(id));
    }

    public void UpdateBook(int id, CreateBookDto entry)
    {
        var validator = new CreateBookDtoValidator();
        validator.ValidateAndThrow(entry);

        var book = new Book()
        {
            Id = id,
            Title = entry.Title,
            PublishedYear = entry.PublishedYear,
            AuthorId = entry.AuthorId
        };
        
        _repository.UpdateBook(book);
    }
}