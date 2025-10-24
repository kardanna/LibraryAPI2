using Microsoft.AspNetCore.Mvc;
using LibraryAPI2.Application.DTO.Author;
using LibraryAPI2.Application.Services;

namespace LibraryAPI2.Controllers
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
        public async Task<ActionResult<IEnumerable<ReturnAuthorDto>>> GetAuthors()
        {
            var authors = await _authorService.GetAllAuthors();
            return authors.ToList();
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<ReturnAuthorDto>> GetAuthor(int id)
        {
            return await _authorService.GetAuthor(id);
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<IActionResult> PutAuthor(int id, CreateAuthorDto authorDto)
        {
            await _authorService.UpdateAuthor(id, authorDto);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ReturnAuthorDto>> PostAuthor(CreateAuthorDto authorDto)
        {
            var createdAuthor = await _authorService.AddAuthor(authorDto);
            return CreatedAtAction(nameof(GetAuthor), new { id = createdAuthor.Id }, createdAuthor);
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            await _authorService.DeleteAuthor(id);
            return NoContent();
        }
    }
}