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

    public ReturnAuthorDto AddAuthor(CreateAuthorDto entry)
    {
        var validator = new CreateAuthorDtoValidator();
        validator.ValidateAndThrow(entry);

        var author = new Author()
        {
            Name = entry.Name,
            DateOfBirth = entry.DateOfBirth
        };

        _repository.AddAuthor(author);

        return new ReturnAuthorDto(author);
    }

    public void DeleteAuthor(int id)
    {
        _repository.DeleteAuthor(id);
    }

    public IEnumerable<ReturnAuthorDto> GetAllAuthors()
    {
        var authors = _repository.GetAllAuthors().Select(a => new ReturnAuthorDto(a));
        return authors;
    }

    public ReturnAuthorDto GetAuthor(int id)
    {
        return new ReturnAuthorDto(_repository.GetAuthor(id));
    }

    public void UpdateAuthor(int id, CreateAuthorDto entry)
    {
        var validator = new CreateAuthorDtoValidator();
        validator.ValidateAndThrow(entry);

        var author = new Author()
        {
            Id = id,
            Name = entry.Name,
            DateOfBirth = entry.DateOfBirth
        };

        _repository.UpdateAuthor(author);
    }
}