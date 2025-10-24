using FluentValidation;
using LibraryAPI2.Application.DTO.Author;
using LibraryAPI2.Application.Interfaces;
using LibraryAPI2.Application.Validators.Author;
using LibraryAPI2.Domain;

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

    public async Task<IEnumerable<ReturnAuthorDto>> GetAllAuthors()
    {
        var authors = await _repository.GetAllAuthors();
        return authors.Select(a => new ReturnAuthorDto(a));;
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
}