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
            if (string.IsNullOrWhiteSpace(userId))
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
                throw new InvalidOperationException("Favorites not found for the user.");
            }

            // Ensure the book doesn't already exist in the favorites
            if (_db.BookFavorites.Any(bf => bf.FavoriteId == fav.Id && bf.BookId == bookId))
            {
                throw new InvalidOperationException("Book is already in favorites.");
            }

            BookFavorite bookFavorite = new BookFavorite
            {
                BookId = bookId,
                FavoriteId = fav.Id,
            };

            _db.BookFavorites.Add(bookFavorite);
            await _db.SaveChangesAsync();

        }



        public async Task<List<BookResponse?>> GetFavoriteBooks(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentNullException(nameof(userId));
            }

            var favoriteBooks = await _db.Books
                .Include(book => book.Categories)
                .Include(book => book.BookLibrary)
                .Include(book => book.BookFavorite)
                .Where(book => book.BookFavorite.Any(bf => bf.Favorite.UserId == userId))
                .Select(book => new
                {
                    Book = book,
                    ReadingStatus = book.BookLibrary
                        .Where(bl => bl.Library.UserId == userId)
                        .Select(bl => bl.ReadingStatus)
                        .FirstOrDefault()
                })
                .ToListAsync(); // Use ToListAsync for async operation

            var result = favoriteBooks
                .Select(bookWithStatus => bookWithStatus.Book.ToBookResponseMobile(bookWithStatus.ReadingStatus))
                .ToList();

            return result;
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
                .FirstOrDefault(x => x.FavoriteId == fav.Id && x.BookId == bookId);

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
