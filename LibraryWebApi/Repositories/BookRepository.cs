using LibraryWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace LibraryWebApi.Repositories
{
    public class BookRepository : IRepository<Book>
    {
        private LibraryContext db;

        public BookRepository()
        {
            db = new LibraryContext();
        }

        public async Task<IEnumerable<Book>> GetObjectList()
        {
            return await db.Books.ToListAsync();
        }

        public async Task<Book> GetObject(int id)
        {
            return await db.Books.FindAsync(id);
        }

        public async Task<Book> Create(Book book)
        {
            var result = await db.Books.AddAsync(book);
            await db.SaveChangesAsync();
            return result.Entity;
        }

        public async Task<Book> Update(Book book)
        {
            var result = await GetObject(book.Id);

            if (result != null)
            {
                result.Name = book.Name;
                result.Year = book.Year;
                result.AuthorId = book.AuthorId;

                await db.SaveChangesAsync();

                return result;
            }

            return null;
        }

        public async Task<Book> Delete(int id)
        {
            var book = await db.Books
                .FirstOrDefaultAsync(e => e.Id == id);
            if (book != null)
            {
                db.Books.Remove(book);

                await db.SaveChangesAsync();

                return book;
            }

            return null;
        }

    }

}