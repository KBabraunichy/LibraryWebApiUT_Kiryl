using LibraryWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApi.Repositories
{
    public class LibraryContext : DbContext
    {
        internal DbSet<Author> Authors { get; set; }
        internal DbSet<Book> Books { get; set; }
        internal DbSet<User> Users { get; set; }
        public LibraryContext()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
            //Database.Migrate();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var builder = new ConfigurationBuilder();
            builder.AddJsonFile("appsettings.json");
            var config = builder.Build();
            string connectionString = config.GetConnectionString("DefaultConnection");

            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
