using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceContracts;
using ServiceContracts.DTO;
using BookMate.Entities;
using BookMate.DataAccess;
using Microsoft.IdentityModel.Tokens;
using BookMate.DataAccess.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Humanizer;
using BookMate.DataAccess.Migrations;

namespace Services
{
    public class BooksService : IBooksService
    {

        private readonly ApplicationDbContext _db;



        public BooksService(ApplicationDbContext applicationDbContext)
        {

            _db = applicationDbContext;

          

        }


     
        public List<BookResponse> GetAllBooks()
        {

            return _db.Books
                .Include(book => book.Categories)
                .Select(book => book.ToBookResponse())
                .ToList();
        }
        public async Task<List<BookResponse>> GetBooksByCategory(List<string>? categoriesName)
        {
            // Validate input
            if (categoriesName == null || !categoriesName.Any())
            {
                return new List<BookResponse>();
            }

            // Query the books that have all the specified categories
            var books = await _db.Books
                                 .Include(book => book.Categories)
                                 .Where(book => categoriesName.All(name => book.Categories.Any(category => category.categoryName == name)))
                                 .ToListAsync();

            // Map the books to BookResponse objects
            var bookResponses = books.Select(book => book.ToBookResponse()).ToList();

            return bookResponses;
        }
        public BookResponse? GetBookByBookId(Guid? Id)
        {
            if(Id== null) 
                return null;

            Book? book_response=_db.Books.FirstOrDefault(temp=>temp.Id==Id);

            if(book_response == null)
                return null;

            return book_response.ToBookResponse();

        }
        public BookResponse? GetBookByBookTitle(string? title)
        {
            if (title == null)
                return null;

            Book? book_response = _db.Books.FirstOrDefault(temp => temp.Title == title);

            if (book_response == null)
                return null;

            return book_response.ToBookResponse();

        }


        public BookResponse AddBook(BookAddRequest? bookAddRequest)
        {
         

            if (bookAddRequest == null)
            {
                throw new ArgumentNullException(nameof(bookAddRequest));
            }


            if(bookAddRequest.Title == null)
            {
                throw new ArgumentException(nameof(bookAddRequest.Title));
            }


            if(_db.Books.Count(temp=>temp.Title==bookAddRequest.Title)>0)
            {

                throw new ArgumentException("Given Book Title already exists");
            }


            Book book = bookAddRequest.ToBook(_db.Categories);
            book.Id=Guid.NewGuid();
            book.AverageRating = 0;
            book.RatingsCount = 0;

            _db.Books.Add(book);
            _db.SaveChanges();


            return book.ToBookResponse();
        }
        public async Task EditBookAsync(Guid bookId, BookAddRequest editedBook)
        {
            // Find the book you want to edit
            var book = await _db.Books.Include(b => b.Categories).FirstOrDefaultAsync(b => b.Id == bookId);

            if (book != null)
            {
                // Update book properties
                book.Title = editedBook.Title;
                book.Author = editedBook.Author;
                book.ImageUrl = editedBook.ImageUrl;
                book.PdfUrl = editedBook.PdfUrl;
                book.VoiceUrl = editedBook.VoiceUrl;
                book.Description = editedBook.Description;
                book.NumberOfPage = editedBook.NumberOfPage;
                book.PublishedYear = editedBook.PublishedYear;

                book.Categories = _db.Categories.Where(c => editedBook.CategoryIds.Contains(c.categoryID)).ToList();


                 try
        {
            // Ensure that the book entity is marked as modified
            _db.Entry(book).State = EntityState.Modified;

            // Save changes to the database
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
            else
            {
                // Handle case where the book with the specified ID is not found
                throw new InvalidOperationException($"Book with ID '{bookId}' not found.");
            }
        }

       
    }
}
