using LibraryWebApi.Interfaces;
using LibraryWebApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace LibraryWebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        
        private readonly IAuthRepository<User, UserLogin> _userRepository;

        public LoginController(IAuthRepository<User, UserLogin> userRepository)
        {
            _userRepository = userRepository;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            if (userLogin == null)
                return BadRequest("No data provided");

            var user = _userRepository.Authenticate(userLogin);

            if (user != null)
            {
                var token = _userRepository.Generate(user);
                return Ok(token);
            }

            return NotFound("User not found");
        }

        [AllowAnonymous]
        [HttpGet("GetRole")]
        public IActionResult GetRole()
        {
            var role = User?.FindFirstValue(ClaimTypes.Role);
            if (role != null)
            {
                
                return Ok(role);
            }

            return NotFound("The token has expired or an invalid request has been sent");
        }
    }
}
