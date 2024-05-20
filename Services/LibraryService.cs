using BookMate.DataAccess.Data;
using BookMate.Entities;
using ServiceContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class LibraryService :ILibraryService
    {

        private readonly ApplicationDbContext _db;

        public LibraryService(ApplicationDbContext db)
        {
            _db = db;
        }
        

        public async Task CreateLibrary(string userId)
        {
           
            if(userId == null)
            {
                throw new ArgumentNullException("userId");
            }

            Library library = new Library
            {
                UserId = userId
            };
           
            _db.Librarys.Add(library);
            await _db.SaveChangesAsync();

        }


        public async Task AddBookToLibrary(string userId, Guid bookId)
        {
            if (userId == null) {
                throw new ArgumentNullException("userId");
            }

            if (bookId == Guid.Empty)
            {
                throw new ArgumentNullException("bookId");
            }


            Library? library = _db.Librarys.FirstOrDefault(x => x.UserId == userId);

            BookLibrary bookLibrary = new BookLibrary
            {
                BookId = bookId,
                LibraryId = library.LibraryId

             };
            
           _db.BookLibraries.Add(bookLibrary);
            await _db.SaveChangesAsync();

            


        }


    }
}
