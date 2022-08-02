using LibraryWebApi.Interfaces;
using LibraryWebApi.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryWebApi.Repositories
{
    public class UserRepository : IAuthRepository<User, UserLogin>
    {
        private LibraryContext db;
        private readonly IConfiguration _config;

        public UserRepository(IConfiguration config)
        {
            db = new LibraryContext();
            _config = config;
        }

        public User Authenticate(UserLogin user)
        {
            var currentUser = db.Users.FirstOrDefault(o => o.Username.ToLower() == user.Username.ToLower().Trim() && o.Password == user.Password);

            if (currentUser != null)
                return currentUser;
            return null;
        }
        public async Task<User> Registration(User user)
        {
            var result = await db.Users.AddAsync(user);
            await db.SaveChangesAsync();
            return result.Entity;
        }

        public bool RegistrationCheck(User user)
        {
            var checkedUser = db.Users.FirstOrDefault(o => o.Username.ToLower().Trim() == user.Username.ToLower().Trim() || 
                                                      o.Email.ToLower().Trim() == user.Email.ToLower().Trim());

            if (checkedUser != null)
                return true;
            return false;

        }


        public string Generate(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Audience"],
              claims,
              expires: DateTime.Now.AddMinutes(15),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
