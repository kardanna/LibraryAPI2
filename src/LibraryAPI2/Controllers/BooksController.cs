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
        public ActionResult<IEnumerable<ReturnBookDto>> GetBooks()
        {
            return _bookService.GetAllBooks().ToList();
        }

        [HttpGet("{id:int:min(1)}")]
        public ActionResult<ReturnBookDto> GetBook(int id)
        {
            return _bookService.GetBook(id);
        }

        [HttpPut("{id:int:min(1)}")]
        public IActionResult PutBook(int id, CreateBookDto bookDto)
        {
            _bookService.UpdateBook(id, bookDto);
            return NoContent();
        }

        [HttpPost]
        public ActionResult<ReturnBookDto> PostBook(CreateBookDto bookDto)
        {
            var createdBook = _bookService.AddBook(bookDto);
            return CreatedAtAction(nameof(GetBook), new { id = createdBook.Id }, createdBook);
        }

        [HttpDelete("{id:int:min(1)}")]
        public IActionResult DeleteBook(int id)
        {
            _bookService.DeleteBook(id);
            return NoContent();
        }
    }
}