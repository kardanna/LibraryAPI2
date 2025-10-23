using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Domain;
using LibraryAPI.Application.DTO.Book;
using LibraryAPI.Application.Validators.Book;
using LibraryAPI.Data;
using LibraryAPI.Exceptions;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly ILibraryRepository _repository;

        public BooksController(ILibraryRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ReturnBookDto>> GetBooks()
        {
            var books = _repository.GetAllBooks().Select(b => new ReturnBookDto(b)).ToList();
            return books;
        }

        [HttpGet("{id:int:min(1)}")]
        public ActionResult<ReturnBookDto> GetBook(int id)
        {
            Book book;
            try
            {
                book = _repository.GetBook(id);
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { errorMessage = e.Message });
            }

            return new ReturnBookDto(book);
        }

        [HttpPut("{id:int:min(1)}")]
        public IActionResult PutBook(int id, CreateBookDto bookDto)
        {
            var validator = new CreateBookDtoValidator();
            var validationResult = validator.Validate(bookDto);
            if(!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new
                {
                    errorCode = e.ErrorCode,
                    errorMessage = e.ErrorMessage
                });
                return BadRequest(new { errors });
            }            

            var book = new Book()
            {
                Id = id,
                Title = bookDto.Title,
                PublishedYear = bookDto.PublishedYear,
                AuthorId = bookDto.AuthorId
            };

            try
            {
                _repository.UpdateBook(book);
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { errorMessage = e.Message });
            }

            return NoContent();
        }

        [HttpPost]
        public ActionResult<ReturnBookDto> PostBook(CreateBookDto bookDto)
        {
            var validator = new CreateBookDtoValidator();
            var validationResult = validator.Validate(bookDto);
            if(!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new
                {
                    errorCode = e.ErrorCode,
                    errorMessage = e.ErrorMessage
                });
                return BadRequest(new { errors });
            }
            
            var book = new Book()
            {
                Title = bookDto.Title,
                PublishedYear = bookDto.PublishedYear,
                AuthorId = bookDto.AuthorId
            };

            try
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
            }

            var returnBook = new ReturnBookDto(book);
            return CreatedAtAction("GetBook", new { id = book.Id }, returnBook);
        }

        [HttpDelete("{id:int:min(1)}")]
        public IActionResult DeleteBook(int id)
        {
            try
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
            }

            return NoContent();
        }
    }
}