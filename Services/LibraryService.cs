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

        private readonly ApplicationDbContext? _db;

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
            BookLibrary? existingBookLibrary = _db.BookLibraries
                .FirstOrDefault(x => x.LibraryId == library.LibraryId && x.BookId == bookId);



            if (existingBookLibrary!=null)
            {

                if (existingBookLibrary.ReadingStatus.ToString().Equals(status))
                {

                    throw new InvalidOperationException("this book already have same reading status");
                }
            }



            if (status == "Reading")
            {
                readingStatus = ReadingStatus.Reading;

              
            }
            else if (status == "ToRead")
            {
                readingStatus = ReadingStatus.ToRead;
              
            }
            else if (status == "Read")
            {
               

                Book? book = _db.Books.Include(book => book.Categories).FirstOrDefault(temp => temp.Id == bookId);

                book.ReadingCount++;

               

                readingStatus = ReadingStatus.Read;
            }
            else
            {
                throw new InvalidOperationException("Reading status not correct.");
            }


        

            if (existingBookLibrary != null)
            {
                // Update the status if the book is already in the library

                if(existingBookLibrary.ReadingStatus.ToString()=="Read")
                {

                    Book? book = _db.Books.Include(book => book.Categories).FirstOrDefault(temp => temp.Id == bookId);
                    book.ReadingCount--;
                }
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

            var favoriteId = _db.Favorites
            .Where(fav => fav.UserId == userId)
             .Select(fav => fav.Id)
    .       FirstOrDefault();

            var booksWithStatus = _db.Books
                .Include(book => book.Categories)
                .Include(book => book.BookLibrary)
                    .ThenInclude(bookLibrary => bookLibrary.Library)
                .Include(book => book.BookFavorite) 
                .Where(book => book.BookLibrary.Any(bl => bl.Library.UserId == userId && bl.ReadingStatus == readingStatus))
                .Select(book => new
                {
                    Book = book,
                    ReadingStatus = book.BookLibrary
                        .Where(bl => bl.Library.UserId == userId && bl.ReadingStatus == readingStatus)
                        .Select(bl => bl.ReadingStatus)
                        .FirstOrDefault(),
                    IsFavorite = book.BookFavorite.Any(fav => fav.FavoriteId == favoriteId) 
                })
                .AsEnumerable().
                Select(booksWithStatus=> booksWithStatus.Book.ToBookResponseMobile(booksWithStatus.ReadingStatus,booksWithStatus.IsFavorite ))
                .ToList(); 


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

            BookLibrary? bookInLibrary = _db.BookLibraries
                .FirstOrDefault(x => x.LibraryId == library.LibraryId && x.BookId == bookId);

            if (bookInLibrary != null)
            {
                if(bookInLibrary.ReadingStatus.ToString().Equals("Read"))
                {
                    Book? book = _db.Books.Include(book => book.Categories).FirstOrDefault(temp => temp.Id == bookId);

                    book.ReadingCount--;

                    try
                    {

                        _db.Entry(book).State = EntityState.Modified;


                        await _db.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException ex)
                    {
                        // Handle concurrency conflicts
                        // Log or handle the exception as needed
                        throw;
                    }
                    catch (DbUpdateException ex)
                    {
                        // Handle other database update errors
                        // Log or handle the exception as needed
                        throw;
                    }
                }

                _db.BookLibraries.Remove(bookInLibrary);
                await _db.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("This Book is not in your Library");

            }



            }
    }
}
