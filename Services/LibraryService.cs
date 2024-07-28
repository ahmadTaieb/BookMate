using BookMate.DataAccess.Data;
using BookMate.Entities;
using BookMate.Entities.Enums;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
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
           
            _db.Libraries.Add(library);
            await _db.SaveChangesAsync();

        }


        public async Task AddBookToLibrary(string userId, Guid bookId, string? status)
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
            Library? library = _db.Libraries.FirstOrDefault(x => x.UserId == userId);

            if (library == null)
            {
                throw new InvalidOperationException("Library not found for the user.");
            }

            ReadingStatus? readingStatus;
            if (status == "Reading")
                readingStatus = ReadingStatus.Reading;
            else if(status=="ToRead")
                readingStatus = ReadingStatus.ToRead;
            else if(status=="Read")
            {
                readingStatus= ReadingStatus.Read;
            }
            else
            {
                throw new InvalidOperationException("Reading status not correct.");
            }


            // Check if the book already exists in the library
            BookLibrary? existingBookLibrary = _db.BookLibraries
                .FirstOrDefault(x => x.LibraryId == library.LibraryId && x.BookId == bookId);

            if (existingBookLibrary != null)
            {
                // Update the status if the book is already in the library
                existingBookLibrary.ReadingStatus = readingStatus;
            }
            else
            {
                // Add the book to the library if it doesn't exist
                BookLibrary bookLibrary = new BookLibrary
                {
                    BookId = bookId,
                    LibraryId = library.LibraryId,
                    ReadingStatus = readingStatus // Include the status in the new entry
                };

                _db.BookLibraries.Add(bookLibrary);
            }

            // Save changes to the database
            await _db.SaveChangesAsync();
        }


        public async Task<List<BookResponse?>>?  GetBooksByStatus(string userId, string status)
        {
            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (!Enum.TryParse<ReadingStatus>(status, true, out var readingStatus))
            {
                throw new ArgumentException($"Invalid status value: {status}", nameof(status));
            }

            Console.WriteLine($"Processing userId: {userId} with status: {status}");

            var booksWithStatus =  _db.Books
                .Include(book => book.Categories)
                .Include(book => book.BookLibrary)
                .ThenInclude(bookLibrary => bookLibrary.Library)
                .Where(book => book.BookLibrary.Any(bl => bl.Library.UserId == userId && bl.ReadingStatus == readingStatus))
                .Select(book => new
                {
                    Book = book,
                    ReadingStatus = book.BookLibrary
                        .Where(bl => bl.Library.UserId == userId && bl.ReadingStatus == readingStatus)
                        .Select(bl => bl.ReadingStatus)
                        .FirstOrDefault()
                })
                .AsEnumerable() // Switch to client-side evaluation for the custom projection.
                .Select(bookWithStatus => bookWithStatus.Book.ToBookResponseMobile(bookWithStatus.ReadingStatus))
                .ToList(); // Use ToListAsync for async operation

          

            return booksWithStatus;
        }

        public async Task RemoveBookFromLibrary(string userId,Guid bookId)
        {


            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (bookId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(bookId));
            }

            Library? library = _db.Libraries.FirstOrDefault(x => x.UserId == userId);

            if (library == null)
            {
                throw new InvalidOperationException("Library not found for the user.");
            }

            BookLibrary? book = _db.BookLibraries
                .FirstOrDefault(x => x.LibraryId == library.LibraryId && x.BookId == bookId);

            if (book != null)
            {
               
                _db.BookLibraries.Remove(book);
                await _db.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("This Book is not in your Library");

            }



            }
    }
}
