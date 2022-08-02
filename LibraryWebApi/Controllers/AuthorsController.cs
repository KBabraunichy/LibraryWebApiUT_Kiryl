using Microsoft.AspNetCore.Mvc;
using LibraryWebApi.Models;
using LibraryWebApi.Interfaces;
using Microsoft.AspNetCore.Authorization;

namespace LibraryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthorsController : ControllerBase
    {
        private readonly IRepository<Author> _authorRepository;
        private readonly ILoggerException _loggerException;

        public AuthorsController(IRepository<Author> repository, ILoggerException loggerException)
        {
            _authorRepository = repository;
            _loggerException = loggerException;
        }

        // GET: api/Authors
        [HttpGet]
        public async Task<ActionResult> GetAuthors()
        {
            try
            {
                return Ok(await _authorRepository.GetObjectList());
            }
            catch (Exception e)
            {
                _loggerException.ExceptionInfo(e);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error retrieving data from the database");
            }
        }

        // GET: api/Authors/5
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Author>> GetAuthor(int id)
        {
            try
            {
                var result = await _authorRepository.GetObject(id);

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

        // PUT: api/Authors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Author>> UpdateAuthor(int id, Author author)
        {
            try
            {
                if (id != author.Id)
                    return BadRequest("Author ID mismatch");

                var authorToUpdate = await _authorRepository.GetObject(id);

                if (authorToUpdate == null)
                    return NotFound($"Author with Id = {id} not found");

                return Ok(await _authorRepository.Update(author));
            }
            catch (Exception e)
            {
                _loggerException.ExceptionInfo(e);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    $"Error updating data. {e.Message}");
            }
        }

        // POST: api/Authors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Author>> CreateAuthor(Author author)
        {
            try
            {
                if (author == null)
                    return BadRequest();

                var createdAuthor = await _authorRepository.Create(author);

                return CreatedAtAction(nameof(CreateAuthor),
                    new { id = createdAuthor.Id }, createdAuthor);
            }
            catch (Exception e)
            {
                _loggerException.ExceptionInfo(e);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error creating new Author record");
            }
        }

        // DELETE: api/Authors/5
        [HttpDelete("{id:int}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<Author>> DeleteAuthor(int id)
        {
            try
            {
                var authorToDelete = await _authorRepository.GetObject(id);

                if (authorToDelete == null)
                {
                    return NotFound($"Author with Id = {id} not found");
                }

                await _authorRepository.Delete(id);

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
