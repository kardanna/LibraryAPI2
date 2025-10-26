using System.Linq.Expressions;
using FluentValidation;
using LibraryAPI2.Application.Common;
using LibraryAPI2.Application.DTO.Author;
using LibraryAPI2.Application.Interfaces;
using LibraryAPI2.Application.Validators.Author;
using LibraryAPI2.Domain;
using LinqKit;

namespace LibraryAPI2.Application.Services;

public class AuthorService : IAuthorService
{
    private readonly ILibraryRepository _repository;

    public AuthorService(ILibraryRepository repository)
    {
        _repository = repository;
    }

    public async Task<ReturnAuthorDto> AddAuthor(CreateAuthorDto entry)
    {
        var validator = new CreateAuthorDtoValidator();
        await validator.ValidateAndThrowAsync(entry);

        var author = new Author()
        {
            Name = entry.Name,
            DateOfBirth = entry.DateOfBirth
        };

        await _repository.AddAuthor(author);

        return new ReturnAuthorDto(author);
    }

    public async Task DeleteAuthor(int id)
    {
        await _repository.DeleteAuthor(id);
    }

    public async Task<IEnumerable<ReturnAuthorDto>> GetAllAuthors(AuthorQueryParameters? query)
    {
        var orderingDelegate = ConstructOrderingDelegate(query);
        var filteringPredicate = ConstructFilteringPredicate(query);

        var authors = await _repository.GetAllAuthors(filteringPredicate, orderingDelegate);
        return authors.Select(a => new ReturnAuthorDto(a));
    }

    public async Task<ReturnAuthorDto> GetAuthor(int id)
    {
        return new ReturnAuthorDto(await _repository.GetAuthor(id));
    }

    public async Task UpdateAuthor(int id, CreateAuthorDto entry)
    {
        var validator = new CreateAuthorDtoValidator();
        await validator.ValidateAndThrowAsync(entry);

        var author = new Author()
        {
            Id = id,
            Name = entry.Name,
            DateOfBirth = entry.DateOfBirth
        };

        await _repository.UpdateAuthor(author);
    }

    private static Func<IQueryable<Author>, IOrderedQueryable<Author>>? ConstructOrderingDelegate(AuthorQueryParameters? query)
    {
        if (query?.OrderBy == null) return null;
        query.OrderDirection ??= OrderDirection.Asc;

        var IsAscending = query.OrderDirection == OrderDirection.Asc;
        Func<IQueryable<Author>, IOrderedQueryable<Author>>? orderingDelegate;

        orderingDelegate = query.OrderBy switch
        {
            AuthorOrderBy.Name => IsAscending ? a => a.OrderBy(a => a.Name) : a => a.OrderByDescending(a => a.Name),
            AuthorOrderBy.DateOfBirth => IsAscending ? a => a.OrderBy(a => a.DateOfBirth) : a => a.OrderByDescending(a => a.DateOfBirth),
            _ => null
        };

        return orderingDelegate;
    }

    private static Expression<Func<Author, bool>>? ConstructFilteringPredicate(AuthorQueryParameters? query)
    {
        if (query == null) return null;

        var filteringPredicate = PredicateBuilder.New<Author>(true);

        if (!string.IsNullOrWhiteSpace(query.NameContains))
        {
            filteringPredicate = filteringPredicate.And(a => a.Name.ToLower().Contains(query.NameContains.ToLower()));
        }

        if (!string.IsNullOrWhiteSpace(query.NameStartsWith))
        {
            filteringPredicate = filteringPredicate.And(a => a.Name.ToLower().StartsWith(query.NameStartsWith.ToLower()));
        }
        
        return filteringPredicate.IsStarted ? filteringPredicate : null;
    }
}