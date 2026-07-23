using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using rest_with_asp_net10_ericles.Data.DTO.V1;
using rest_with_asp_net10_ericles.Services.Interfaces;

namespace rest_with_asp_net10_ericles.Controllers
{
    [Route("api/[controller]/v2")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly ILogger<BookController> _logger;

        public BookController(IBookService bookService, ILogger<BookController> logger)
        {
            _bookService = bookService;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(BookDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Get()
        {
            _logger.LogInformation("Fetching all books");
            return StatusCode(StatusCodes.Status200OK, _bookService.FindAll());
        }

        [HttpGet("{Id}")]
        [ProducesResponseType(200, Type = typeof(BookDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Get(int Id)
        {
            _logger.LogInformation("Fetching book by ID: {Id}", Id);
            var book = _bookService.FindById(Id);
            if (book == null)
            {
                _logger.LogWarning("Book with ID: {Id} not found", Id);
                return NotFound();
            }
            return StatusCode(StatusCodes.Status200OK, book);
        }

        [HttpPost]
        [ProducesResponseType(200, Type = typeof(BookDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Post([FromBody] BookDTO book)
        {
            _logger.LogInformation("Creating new book: {title} {author}", book.Title, book.Author);
            var createdBook = _bookService.Create(book);
            if (createdBook == null)
            {
                _logger.LogError("Failed to create book: {title} {author}", book.Title, book.Author);
                return BadRequest("Book could not be created.");
            }
            return StatusCode(StatusCodes.Status201Created, createdBook);
        }

        [HttpPut]
        [ProducesResponseType(200, Type = typeof(BookDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Put([FromBody] BookDTO book)
        {
            _logger.LogInformation("Updating book with ID: {Id}", book.Id);
            var updatedBook = _bookService.Update(book);
            if (updatedBook == null)
            {
                _logger.LogWarning("Failed to update Book with Id {Id}", book.Id);
                return BadRequest("Book could not be updated.");
            }
            _logger.LogDebug("Book with ID: {Id} updated successfully", book.Id);
            return StatusCode(StatusCodes.Status200OK, updatedBook);
        }

        [HttpDelete]
        [ProducesResponseType(204, Type = typeof(BookDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Delete(int Id)
        {
            _logger.LogInformation("Deleting book with ID: {Id}", Id);
            var deleted = _bookService.Delete(Id);
            if (!deleted)
            {
                _logger.LogWarning("Book with ID: {Id} not found for deletion", Id);
                return NotFound();
            }
            _logger.LogDebug("Book with ID: {Id} deleted successfully", Id);
            return StatusCode(StatusCodes.Status204NoContent);
        }
    }
}
