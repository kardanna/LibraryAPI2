using FluentValidation;
using LibraryAPI2.Application.DTO.Book;
using LibraryAPI2.Application.Interfaces;
using LibraryAPI2.Application.Validators.Book;
using LibraryAPI2.Domain;

namespace LibraryAPI2.Application.Services;

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