using BookMate.DataAccess.Data;
using BookMate.Entities;
using BookMate.Entities.Enums;
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


        public async Task AddBookToLibrary(string userId, Guid bookId, ReadingStatus? readingstatus)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (bookId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(bookId));
            }

            // Retrieve the user's library
            Library? library = _db.Librarys.FirstOrDefault(x => x.UserId == userId);

            if (library == null)
            {
                throw new InvalidOperationException("Library not found for the user.");
            }

            // Check if the book already exists in the library
            BookLibrary? existingBookLibrary = _db.BookLibraries
                .FirstOrDefault(x => x.LibraryId == library.LibraryId && x.BookId == bookId);

            if (existingBookLibrary != null)
            {
                // Update the status if the book is already in the library
                existingBookLibrary.ReadingStatus = readingstatus;
            }
            else
            {
                // Add the book to the library if it doesn't exist
                BookLibrary bookLibrary = new BookLibrary
                {
                    BookId = bookId,
                    LibraryId = library.LibraryId,
                    ReadingStatus = readingstatus // Include the status in the new entry
                };

                _db.BookLibraries.Add(bookLibrary);
            }

            // Save changes to the database
            await _db.SaveChangesAsync();
        }



    }
}
