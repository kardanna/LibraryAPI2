using System.Linq.Expressions;
using FluentValidation;
using LibraryAPI2.Application.DTO.Book;
using LibraryAPI2.Application.Interfaces;
using LibraryAPI2.Application.Validators.Book;
using LibraryAPI2.Domain;
using LibraryAPI2.Application.Common;
using LinqKit;

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

    public async Task<IEnumerable<ReturnBookDto>> GetAllBooks(BookQueryParameters? query)
    {
        var filteringPredicate = ConstructFileringPredicate(query);
        var orderingDelegate = ConstructOrderingDelegate(query);

        var books = await _repository.GetAllBooks(filteringPredicate, orderingDelegate);
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
    
    private static Func<IQueryable<Book>, IOrderedQueryable<Book>>? ConstructOrderingDelegate(BookQueryParameters? query)
    {
        if (query?.OrderBy == null) return null;
        query.OrderDirection ??= OrderDirection.Asc;
        var IsAscending = query.OrderDirection == OrderDirection.Asc;

        Func<IQueryable<Book>, IOrderedQueryable<Book>>? orderingDelegate;

        orderingDelegate = query.OrderBy switch
        {
            BookOrderBy.Title => IsAscending ? b => b.OrderBy(b => b.Title) : b => b.OrderByDescending(b => b.Title),
            BookOrderBy.PublishedYear => IsAscending ? b => b.OrderBy(b => b.PublishedYear) : b => b.OrderByDescending(b => b.PublishedYear),
            BookOrderBy.AuthorName => IsAscending ? b => b.OrderBy(b => b.Author.Name) : b => b.OrderByDescending(b => b.Author.Name),
            _ => null
        };

        return orderingDelegate;
    }

    private static Expression<Func<Book, bool>>? ConstructFileringPredicate(BookQueryParameters? query)
    {
        var filteringPredicate = PredicateBuilder.New<Book>(true);
        
        if (query == null) return null;
        
        if (query.PublishedAfter != null) filteringPredicate = filteringPredicate.And(a => a.PublishedYear > query.PublishedAfter);
        if (query.PublishedBefore != null) filteringPredicate = filteringPredicate.And(a => a.PublishedYear < query.PublishedBefore);

        return filteringPredicate.IsStarted ? filteringPredicate : null;
    }
}