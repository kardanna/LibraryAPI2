using Microsoft.AspNetCore.Mvc;
using LibraryAPI2.Application.DTO.Book;
using LibraryAPI2.Application.Services;

namespace LibraryAPI2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ReturnBookDto>>> GetBooks()
        {
            var books = await _bookService.GetAllBooks();
            return books.ToList();
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<ActionResult<ReturnBookDto>> GetBook(int id)
        {
            return await _bookService.GetBook(id);
        }

        [HttpPut("{id:int:min(1)}")]
        public async Task<IActionResult> PutBook(int id, CreateBookDto bookDto)
        {
            await _bookService.UpdateBook(id, bookDto);
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<ReturnBookDto>> PostBook(CreateBookDto bookDto)
        {
            var createdBook = await _bookService.AddBook(bookDto);
            return CreatedAtAction(nameof(GetBook), new { id = createdBook.Id }, createdBook);
        }

        [HttpDelete("{id:int:min(1)}")]
        public async Task<IActionResult> DeleteBook(int id)
        {
            await _bookService.DeleteBook(id);
            return NoContent();
        }
    }
}