using LibraryWebApi.Interfaces;
using LibraryWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LibraryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IAuthRepository<User, UserLogin> _userRepository;
        private readonly ILoggerException _loggerException;

        public UserController(IAuthRepository<User, UserLogin> userRepository, ILoggerException loggerException)
        {
            _userRepository = userRepository;
            _loggerException = loggerException;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<User>> RegistrateUser(User user)
        {
            try
            {
                if (user == null)
                    return BadRequest("No data provided");

                var registeredUser = _userRepository.RegistrationCheck(user);

                if (registeredUser)
                    return BadRequest("The user with the specified email address or username already exists");

                var createdUser = await _userRepository.Registration(user);

                return CreatedAtAction(nameof(RegistrateUser),
                    new { id = createdUser.Id }, createdUser);
            }
            catch (Exception e)
            {
                _loggerException.ExceptionInfo(e);

                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Error registering new user");
            }
        }
    }
}
