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
    public class FeedBackService : IFeedBackService
    {


        private readonly ApplicationDbContext _db;

        public FeedBackService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<List<CategoryPercentResponse?>?> CategoriesPercent()
        {


            int numberOfBooks = await _db.Books.CountAsync();

            List<string> categories = await _db.Categories.Select(c => c.categoryName).ToListAsync();

            List<CategoryPercentResponse?> result = new List<CategoryPercentResponse?>();

            foreach (var category in categories)
            {
                int cnt = await _db.Books.Where(b => b.Categories.Any(bc => bc.categoryName == category)).CountAsync();

                double percent = numberOfBooks == 0 ? 0 : (double)cnt*100 / numberOfBooks;

                var response = new CategoryPercentResponse(category, percent);
               // Console.WriteLine($"Category: {response.CategoryName}, Percent: {response.Percent}");
                result.Add(response);

            }

            return result;
        }

     

        public async Task<List<BookResponse?>?> TopReadBooks()
        {
            List<BookResponse?>? books = await _db.Books
                .OrderByDescending(b => b.ReadingCount)
                .Take(5)
                .ToListAsync()
                .ContinueWith(task => task.Result.Select(b => b.ToBookResponse()).ToList());

            return books;
        }

        public async Task<List<ApplicationUserResponse>> TopReader()
        {
            var readers = await _db.BookLibraries
                .Where(bl => bl.ReadingStatus == ReadingStatus.Read)
                .GroupBy(bl => new { bl.Library.UserId})
                .Select(g => new ApplicationUserResponse
                {
                    Id = g.Key.UserId,
                    Name = _db.ApplicationUsers.FirstOrDefault(u=>u.Id==g.Key.UserId).UserName,
                    Email= _db.ApplicationUsers.FirstOrDefault(u => u.Id == g.Key.UserId).Email,
                    ReadingCount = g.Count()
                })
                .OrderByDescending(r => r.ReadingCount)
                .Take(5)
                .ToListAsync();

            return readers;
        }
    }
}
