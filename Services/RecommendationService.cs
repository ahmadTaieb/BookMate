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
    public class RecommendationService : IRecommendationService
    {

        private ApplicationDbContext _db;

        public RecommendationService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddFavoriteCategories(List<string> categories,string userId)
        {
            ApplicationUser? user =_db.ApplicationUsers.FirstOrDefault(u=>u.Id == userId);

            if (user == null)
            {
                throw new Exception("user not found");
            }

            try
            {
                user.FavoriteCategories = categories;
                await _db.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception();
            }
            

        }

        public async Task<List<BookResponse>> RecommendationsBooks(string userId)
        {

            ApplicationUser? user =_db.ApplicationUsers.FirstOrDefault(u=> u.Id == userId);
            if (user == null)
            {
                throw new Exception("user not found");
            }

            List<string?>? categories = user.FavoriteCategories;


            var favoriteId = _db.Favorites
           .Where(fav => fav.UserId == userId)
            .Select(fav => fav.Id)
         .FirstOrDefault();

            // Fetch the user's library book IDs
            var userLibraryBookIds = _db.BookLibraries
                .Where(bl => bl.Library.UserId == userId)
                .Select(bl => bl.BookId)
                .ToList();


            List<BookResponse>? responses= new List<BookResponse>();

            if (categories != null)
            {
                foreach (var category in categories)
                {
                    var books = _db.Books
                        .Include(book => book.Categories)
                        .Include(book => book.BookLibrary)
                            .ThenInclude(bookLibrary => bookLibrary.Library)
                        .Include(book => book.BookFavorite)
                        .Where(book =>
                            !userLibraryBookIds.Contains(book.Id) && // Exclude books in the user's library
                            book.Categories.Any(c => c.categoryName == category)) // Filter books by category
                        .Select(book => new
                        {
                            Book = book,
                            IsFavorite = book.BookFavorite.Any(fav => fav.FavoriteId == favoriteId)
                        })
                        .AsEnumerable()
                        .Select(books => books.Book.ToBookResponseMobile(null, books.IsFavorite))
                        .OrderByDescending(b => b.ReadingCount)
                        .Take(4)
                        .ToList();


                    responses.AddRange(books);
                }


                responses = responses
                    .OrderByDescending(b => b.ReadingCount)
                    .ToList();

            }
            else
            {
                var books = _db.Books
                       .Include(book => book.Categories)
                       .Include(book => book.BookLibrary)
                           .ThenInclude(bookLibrary => bookLibrary.Library)
                       .Include(book => book.BookFavorite)
                       .Where(book =>
                           !userLibraryBookIds.Contains(book.Id)) 
                       .Select(book => new
                       {
                           Book = book,
                           IsFavorite = book.BookFavorite.Any(fav => fav.FavoriteId == favoriteId)
                       })
                       .AsEnumerable()
                       .Select(books => books.Book.ToBookResponseMobile(null, books.IsFavorite))
                       .OrderByDescending(b => b.ReadingCount)
                       .Take(5)
                       .ToList();


                responses.AddRange(books);



            }

            return responses;
        }
    }
}
