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
    public class FavoritesService : IFavoritesService
    {


        private readonly ApplicationDbContext _db;

        public FavoritesService(ApplicationDbContext db)
        {
            _db = db;
        }

       

        public async Task CreateFavorite(string userId)
        {
          

                if (userId == null)
                {
                    throw new ArgumentNullException("userId");
                }

                Favorite fav = new Favorite
                {
                    UserId = userId
                };

                _db.Favorites.Add(fav);

                await _db.SaveChangesAsync();
        }



        public async Task AddBookToFav(string userId, Guid bookId)
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
            Favorite? fav = _db.Favorites.FirstOrDefault(x => x.UserId == userId);

            if (fav == null)
            {
                throw new InvalidOperationException("Favorites not found for the user.");
            }

            // Add the book to the library if it doesn't exist
            BookFavorite bookFavorite = new BookFavorite
            {
                BookId = bookId,
                Favorite_Id = fav.Id,
               
            };

            _db.BookFavorites.Add(bookFavorite);
        

        // Save changes to the database
        await _db.SaveChangesAsync();

    }



        public async Task<List<BookResponse?>?>GetFavoriteBooks(string userId)
        {
          
                if (userId == null)
                {
                    throw new ArgumentNullException(nameof(userId));
                }

              
                List<BookResponse?> favoritBooks = _db.Books
                    .Include(book => book.Categories)
                    .Include(book => book.BookLibrary)
                    .ThenInclude(bookLibrary => bookLibrary.Library)
                    .Where(book => book.BookLibrary.Any(bl => bl.Library.UserId == userId))
                    .Select(book => new
                    {
                        Book = book,
                        ReadingStatus = book.BookLibrary
                            .Where(bl => bl.Library.UserId == userId)
                            .Select(bl => bl.ReadingStatus)
                            .FirstOrDefault()
                    })
                    .AsEnumerable() // Switch to client-side evaluation for the custom projection.
                    .Select(bookWithStatus => bookWithStatus.Book.ToBookResponseMobile(bookWithStatus.ReadingStatus))
                    .ToList(); // Use ToListAsync for async operation



                return   favoritBooks;
            
        }


        public async Task RemoveBookFromFav(string userId, Guid bookId)
        {


            if (userId == null)
            {
                throw new ArgumentNullException(nameof(userId));
            }

            if (bookId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(bookId));
            }

            Favorite? fav = _db.Favorites.FirstOrDefault(x => x.UserId == userId);

            if (fav == null)
            {
                throw new InvalidOperationException("Favorite not found for the user.");
            }

            BookFavorite? book = _db.BookFavorites
                .FirstOrDefault(x => x.Favorite_Id == fav.Id && x.BookId == bookId);

            if (book != null)
            {

                _db.BookFavorites.Remove(book);
                await _db.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("This Book is not in your Favorite");

            }



        }

    }
}
