using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Models;
using LibraryAPI.DTO.Author;
using LibraryAPI.DTO.Author.Validators;
using LibraryAPI.Data;
using LibraryAPI.Exceptions;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly ILibraryRepository _repository;

        public AuthorsController(ILibraryRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ReturnAuthorDto>> GetAuthors()
        {
            var authors = _repository.GetAllAuthors().Select(a => new ReturnAuthorDto(a)).ToList();
            return authors;
        }

        [HttpGet("{id:int:min(1)}")]
        public ActionResult<ReturnAuthorDto> GetAuthor(int id)
        {
            Author author;
            try
            {
                author = _repository.GetAuthor(id);
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { errorMessage = e.Message });
            }

            return new ReturnAuthorDto(author);
        }

        [HttpPut("{id:int:min(1)}")]
        public IActionResult PutAuthor(int id, CreateAuthorDto authorDto)
        {
            var validator = new CreateAuthorDtoValidator();
            var validationResult = validator.Validate(authorDto);
            if(!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new
                {
                    errorCode = e.ErrorCode,
                    errorMessage = e.ErrorMessage
                });
                return BadRequest(new { errors });
            }
            
            var author = new Author()
            {
                Id = id,
                Name = authorDto.Name,
                DateOfBirth = authorDto.DateOfBirth
            };

            try
            {
                _repository.UpdateAuthor(author);
            }
            catch (ArgumentException e)
            {
                return BadRequest(new { errorMessage = e.Message });
            }

            return NoContent();
        }

        [HttpPost]
        public ActionResult<ReturnAuthorDto> PostAuthor(CreateAuthorDto authorDto)
        {
            var validator = new CreateAuthorDtoValidator();
            var validationResult = validator.Validate(authorDto);
            if(!validationResult.IsValid)
            {
                var errors = validationResult.Errors.Select(e => new
                {
                    errorCode = e.ErrorCode,
                    errorMessage = e.ErrorMessage
                });
                return BadRequest(new { errors });
            }

            var author = new Author()
            {
                Name = authorDto.Name,
                DateOfBirth = authorDto.DateOfBirth
            };

            try
            {
                author.Id = _repository.AddAuthor(author);
            }
            catch (RepositoryUpdateException e)
            {
                return StatusCode(500, new { errorMessage = e.Message });
            }

            var returnAuthor = new ReturnAuthorDto(author);
            return CreatedAtAction(nameof(GetAuthor), new { id = author.Id }, returnAuthor);
        }

        [HttpDelete("{id:int:min(1)}")]
        public IActionResult DeleteAuthor(int id)
        {
            try
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
            }

            return NoContent();
        }
    }
}