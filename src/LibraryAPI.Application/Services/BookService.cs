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

        var book = new Book()
        {
            Title = entry.Title,
            PublishedYear = entry.PublishedYear,
            AuthorId = entry.AuthorId
        };
        
        _repository.AddBook(book);

        /*try
        {
            book.Id = _repository.AddBook(book);
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { errorMessage = e.Message });
        }
        catch(RepositoryUpdateException e)
        {
            return StatusCode(500, new { errorMessage = e.Message });
        }*/

        return new ReturnBookDto(book);
         
        //return CreatedAtAction("GetBook", new { id = book.Id }, returnBook);
    }

    public void DeleteBook(int id)
    {
        _repository.DeleteBook(id);

        /*try
        {
            _repository.DeleteBook(id);
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

    public IEnumerable<ReturnBookDto> GetAllBooks()
    {
        var books = _repository.GetAllBooks().Select(b => new ReturnBookDto(b));
        return books;
    }

    public ReturnBookDto GetBook(int id)
    {
        //ReturnBookDto book;
        //book = new ReturnBookDto(_repository.GetBook(id));

        /*try
        {
            book = new ReturnBookDto(_repository.GetBook(id));
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { errorMessage = e.Message });
        }*/

        return new ReturnBookDto(_repository.GetBook(id));
    }

    public void UpdateBook(int id, CreateBookDto entry)
    {
        var validator = new CreateBookDtoValidator();
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

        var book = new Book()
        {
            Id = id,
            Title = entry.Title,
            PublishedYear = entry.PublishedYear,
            AuthorId = entry.AuthorId
        };
        
        _repository.UpdateBook(book);

        /*try
        {
            _repository.UpdateBook(book);
        }
        catch (ArgumentException e)
        {
            return BadRequest(new { errorMessage = e.Message });
        }*/

        //return NoContent();
    }
}