using Microsoft.AspNetCore.Mvc;
using LibraryWebApi.Models;
using LibraryWebApi.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace LibraryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class BooksController : ControllerBase
    {
        private readonly IRepository<Book> _bookRepository;
        private readonly IRepository<Author> _authorRepository;
        private readonly ILoggerException _loggerException;

        public BooksController(IRepository<Book> bookRepository, IRepository<Author> authorRepository, ILoggerException loggerException)
        {
            _bookRepository = bookRepository;
            _authorRepository = authorRepository;
            _loggerException = loggerException;
        }

        // GET: api/Books
        [HttpGet]
        public async Task<ActionResult> GetBooks()
        {
            try
            {
                return Ok(await _bookRepository.GetObjectList());
            }
            catch (Exception e)
            {
                _loggerException.ExceptionInfo(e);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        // GET: api/Books/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Book>> GetBook(int id)
        {
            try
            {
                var result = await _bookRepository.GetObject(id);

                if (result == null)
                    return NotFound();

                return Ok(result);
            }
            catch (Exception e)
            {
                _loggerException.ExceptionInfo(e);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        // PUT: api/Books/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Book>> UpdateBook(int id, Book book)
        {
            try
            {
                if (id != book.Id)
                    return BadRequest("Book ID mismatch");

                var author = await _authorRepository.GetObject(book.AuthorId);

                if (author == null)
                    return NotFound("There is no author with provided AuthorId");

                var bookToUpdate = await _bookRepository.GetObject(id);

                if (bookToUpdate == null)
                    return NotFound($"Book with Id = {id} not found");

                return Ok(await _bookRepository.Update(book));
            }
            catch (Exception e)
            {
                _loggerException.ExceptionInfo(e);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error updating data. {e.Message}");
            }
        }

        // POST: api/Books
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Book>> CreateBook(Book book)
        {
            try
            {
                if (book == null)
                    return BadRequest();

                var author = await _authorRepository.GetObject(book.AuthorId);

                if (author == null)
                    return NotFound("There is no author with provided AuthorId");

                var createdBook = await _bookRepository.Create(book);

                return CreatedAtAction(nameof(CreateBook),
                    new { id = createdBook.Id }, createdBook);
            }
            catch (Exception e)
            {
                _loggerException.ExceptionInfo(e);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new Book record");
            }
        }

        // DELETE: api/Books/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Book>> DeleteBook(int id)
        {
            try
            {
                var bookToDelete = await _bookRepository.GetObject(id);

                if (bookToDelete == null)
                {
                    return NotFound($"Book with Id = {id} not found");
                }

                await _bookRepository.Delete(id);

                return NoContent();
            }
            catch (Exception e)
            {
                _loggerException.ExceptionInfo(e);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error deleting data");
            }
        }
    }
}
