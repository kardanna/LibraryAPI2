using Microsoft.AspNetCore.Mvc;
using LibraryAPI.Application.DTO.Author;
using LibraryAPI.Application.Services;

namespace LibraryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorsController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ReturnAuthorDto>> GetAuthors()
        {
            return _authorService.GetAllAuthors().ToList();
        }

        [HttpGet("{id:int:min(1)}")]
        public ActionResult<ReturnAuthorDto> GetAuthor(int id)
        {
            return _authorService.GetAuthor(id);
        }

        [HttpPut("{id:int:min(1)}")]
        public IActionResult PutAuthor(int id, CreateAuthorDto authorDto)
        {
            _authorService.UpdateAuthor(id, authorDto);
            return NoContent();
        }

        [HttpPost]
        public ActionResult<ReturnAuthorDto> PostAuthor(CreateAuthorDto authorDto)
        {
            var createdAuthor = _authorService.AddAuthor(authorDto);
            return CreatedAtAction(nameof(GetAuthor), new { id = createdAuthor.Id }, createdAuthor);
        }

        [HttpDelete("{id:int:min(1)}")]
        public IActionResult DeleteAuthor(int id)
        {
            _authorService.DeleteAuthor(id);
            return NoContent();
        }
    }
}