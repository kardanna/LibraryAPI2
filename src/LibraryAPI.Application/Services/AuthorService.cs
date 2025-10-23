using LibraryAPI.Application.DTO.Author;
using LibraryAPI.Application.Interfaces;
using LibraryAPI.Application.Validators.Author;
using LibraryAPI.Domain;

namespace LibraryAPI.Application.Services;

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
        var validationResult = validator.Validate(entry);
        if(!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => new
            {
                errorCode = e.ErrorCode,
                errorMessage = e.ErrorMessage
            });
            //return BadRequest(new { errors });
        }

        var author = new Author()
        {
            Name = entry.Name,
            DateOfBirth = entry.DateOfBirth
        };

        _repository.AddAuthor(author);

        /*try
        {
            author.Id = _repository.AddAuthor(author);
        }
        catch (RepositoryUpdateException e)
        {
            return StatusCode(500, new { errorMessage = e.Message });
        }*/

        return new ReturnAuthorDto(author);        
        
        //return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, returnAuthor);
    }

    public void DeleteAuthor(int id)
    {
        _repository.DeleteAuthor(id);
        
        /*try
        {
            _repository.DeleteAuthor(id);
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { errorMessage = e.Message });
        }
        catch (RepositoryUpdateException e)
        {
            return StatusCode(500, new { errorMessage = e.Message });
        }*/

        //return NoContent();
    }

    public IEnumerable<ReturnAuthorDto> GetAllAuthors()
    {
        var authors = _repository.GetAllAuthors().Select(a => new ReturnAuthorDto(a));
        return authors;
    }

    public ReturnAuthorDto GetAuthor(int id)
    {
        /*ReturnAuthorDto author;
        try
        {
            author = _repository.GetAuthor(id);
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { errorMessage = e.Message });
        }*/

        return new ReturnAuthorDto(_repository.GetAuthor(id));
    }

    public void UpdateAuthor(int id, CreateAuthorDto entry)
    {
        var validator = new CreateAuthorDtoValidator();
        var validationResult = validator.Validate(entry);
        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(e => new
            {
                errorCode = e.ErrorCode,
                errorMessage = e.ErrorMessage
            });
            //return BadRequest(new { errors });
        }

        var author = new Author()
        {
            Id = id,
            Name = entry.Name,
            DateOfBirth = entry.DateOfBirth
        };

        _repository.UpdateAuthor(author);

        /*try
        {
            _repository.UpdateAuthor(author);
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { errorMessage = e.Message });
        }*/

        //return NoContent();
    }
}