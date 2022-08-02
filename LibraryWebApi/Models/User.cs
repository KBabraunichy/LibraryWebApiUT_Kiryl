using Microsoft.EntityFrameworkCore;

namespace LibraryWebApi.Models
{
    [Index(nameof(Username), IsUnique = true), Index(nameof(Email), IsUnique = true)]
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }
    }
}
