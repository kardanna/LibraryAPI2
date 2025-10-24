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

    public async Task<ReturnBookDto> AddBook(CreateBookDto entry)
    {
        var validator = new CreateBookDtoValidator();
        await validator.ValidateAndThrowAsync(entry);

        var book = new Book()
        {
            Title = entry.Title,
            PublishedYear = entry.PublishedYear,
            AuthorId = entry.AuthorId
        };
        
        await _repository.AddBook(book);

        return new ReturnBookDto(book);
    }

    public async Task DeleteBook(int id)
    {
        await _repository.DeleteBook(id);
    }

    public async Task<IEnumerable<ReturnBookDto>> GetAllBooks()
    {
        var books = await _repository.GetAllBooks();
        return books.Select(b => new ReturnBookDto(b));;
    }

    public async Task<ReturnBookDto> GetBook(int id)
    {
        return new ReturnBookDto(await _repository.GetBook(id));
    }

    public async Task UpdateBook(int id, CreateBookDto entry)
    {
        var validator = new CreateBookDtoValidator();
        await validator.ValidateAndThrowAsync(entry);

        var book = new Book()
        {
            Id = id,
            Title = entry.Title,
            PublishedYear = entry.PublishedYear,
            AuthorId = entry.AuthorId
        };
        
        await _repository.UpdateBook(book);
    }
}