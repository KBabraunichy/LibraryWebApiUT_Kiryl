using LibraryWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApi.Repositories
{
    public class AuthorRepository : IRepository<Author>
    {
        private LibraryContext db;

        public AuthorRepository()
        {
            db = new LibraryContext();
        }

        public async Task<IEnumerable<Author>> GetObjectList()
        {
            return await db.Authors.ToListAsync();
        }

        public async Task<Author> GetObject(int id)
        {
            return await db.Authors.FindAsync(id);
        }

        public async Task<Author> Create(Author author)
        {
            var result = await db.Authors.AddAsync(author);
            await db.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Author> Update(Author author)
        {
            var result = await GetObject(author.Id);

            if (result != null)
            {
                result.FirstName = author.FirstName;
                result.LastName = author.LastName;
                result.SurName = author.SurName;
                result.BirthDate = author.BirthDate;

                await db.SaveChangesAsync();

                return result;
            }

            return null;
        }

        public async Task<Author> Delete(int id)
        {
            var author = await db.Authors
                .FirstOrDefaultAsync(e => e.Id == id);
            if (author != null)
            {
                db.Authors.Remove(author);

                await db.SaveChangesAsync();

                return author;
            }

            return null;
        }

    }
}