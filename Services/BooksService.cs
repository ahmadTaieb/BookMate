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
using BookMate.Entities.Enums;
using Microsoft.CodeAnalysis.CSharp.Syntax;
//using BookMate.DataAccess.Migrations;

namespace Services
{
    public class BooksService : IBooksService
    {

        private readonly ApplicationDbContext _db;



        public BooksService(ApplicationDbContext applicationDbContext)
        {

            _db = applicationDbContext;

          

        }


     
        public List<BookResponse?> GetAllBooks(string? userId)
        {

            if (userId == null)
            {
                return _db.Books
                    .Include(book => book.Categories)
                    .Select(book => book.ToBookResponse())
                    .ToList();
            }
            return _db.Books
       .Include(book => book.Categories)
       .Include(book => book.BookLibrary)
       .ThenInclude(bookLibrary => bookLibrary.Library)
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
       .ToList();


        }


        public async Task<List<BookResponse?>>? GetBooksByCategory(List<string>? categoriesName, string? userId)
        {
            // Validate input
            if (categoriesName == null || !categoriesName.Any())
            {
                return new List<BookResponse>();
            }

            // Query the books that have all the specified categories
            var books = await _db.Books
                           .Include(book => book.Categories)
                           .Include(book => book.BookLibrary)
                           .ThenInclude(bookLibrary => bookLibrary.Library)
                           .Where(book => categoriesName.All(name => book.Categories.Any(category => category.categoryName == name)))
                           .ToListAsync();


            List<BookResponse?> bookResponses;
            // Map the books to BookResponse objects
            if (userId == null) {

                bookResponses = books.Select(book => book.ToBookResponse()).ToList();

            }
            else
            {
                  bookResponses = books.Select(book =>
                {
                    var readingStatus = book.BookLibrary
                                            .Where(bl => bl.Library.UserId == userId)
                                            .Select(bl => (ReadingStatus?)bl.ReadingStatus)
                                            .FirstOrDefault();

                    return book.ToBookResponseMobile(readingStatus);
                }).ToList();
            


            }

            return bookResponses;
        }
        public BookResponse? GetBookByBookId(Guid? Id)
        {
            if(Id== null) 
                return null;

            Book? book_response=_db.Books.Include(book => book.Categories).FirstOrDefault(temp=>temp.Id==Id);

            if(book_response == null)
                return null;

            BookResponse? bookResponse = book_response.ToBookResponse();
            return bookResponse;

        }
        public BookResponse? GetBookByBookTitle(string? title)
        {
            if (title == null)
                return null;

            Book? book_response = _db.Books.Include(book => book.Categories).FirstOrDefault(temp => temp.Title == title);

            if (book_response == null)
                return null;

            return book_response.ToBookResponse();

        }


        public async Task AddBook(BookAddRequest? bookAddRequest)
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
            book.ImageUrl = await GetImageUrl(file: bookAddRequest.ImageFile);
            book.PdfUrl=await GetPdfUrl(file: bookAddRequest.PdfFile);
            book.VoiceUrl=await GetVoiceUrl(file: bookAddRequest.VoiceFile);
            book.Id=Guid.NewGuid();
            book.AverageRating = 0;
            book.RatingsCount = 0;
            book.ReadingCount = 0;
            _db.Books.Add(book);
            _db.SaveChanges();


            
        }
        public async Task EditBookAsync(string? bookTitle, BookAddRequest? editedBook)
        {
            // Find the book you want to edit
            var book = await _db.Books.Include(b => b.Categories).FirstOrDefaultAsync(b => b.Title == bookTitle);

            if (book != null)
            {
                // Update book properties
                book.Title = editedBook.Title;
                book.Author = editedBook.Author;
                book.ImageUrl = await GetImageUrl(file: editedBook.ImageFile);
                book.PdfUrl = await GetPdfUrl(file : editedBook.PdfFile); 
                book.VoiceUrl = await GetVoiceUrl(file: editedBook.VoiceFile);
                book.Description = editedBook.Description;
                book.NumberOfPages = editedBook.NumberOfPages;
                book.PublishedYear = editedBook.PublishedYear;

                var categoryNames = editedBook.Categories.Select(c => c.CategoryName).ToList();
                book.Categories = _db.Categories.Where(c => categoryNames.Contains(c.categoryName)).ToList();


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
                throw new InvalidOperationException($"Book with title '{bookTitle}' not found.");
            }
        }


        public async Task DeleteBook(string title)
        {
            var book = _db.Books.FirstOrDefault(b => b.Title == title);

            if (book == null)
            {
                throw new ArgumentException("Book not found");
            }

            _db.Books.Remove(book);
            await _db.SaveChangesAsync();
        }




        public async Task IncrementReadingCount(Guid bookId)
        {
            if (bookId == Guid.Empty) throw new ArgumentException("Invalid book ID", nameof(bookId));

            var book = await _db.Books.FindAsync(bookId);
            if (book == null)
            {
                throw new KeyNotFoundException("Book not found");
            }

            if (book.ReadingCount.HasValue)
            {
                book.ReadingCount++;
            }
            else
            {
                book.ReadingCount = 1;
            }

            try
            {
                _db.Entry(book).State = EntityState.Modified;
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Console.WriteLine("Concurrency exception: " + ex.Message);
                throw new InvalidOperationException("A concurrency error occurred while updating the book.");
            }
            catch (DbUpdateException ex)
            {
                Console.WriteLine("Database update exception: " + ex.Message);
                throw new InvalidOperationException("An error occurred while updating the book.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("An unexpected exception occurred: " + ex.Message);
                throw new InvalidOperationException("An unexpected error occurred while updating the book.");
            }

           
        }


        private async Task<string?> GetImageUrl(IFormFile? file)
        {
            if (file == null)
            {
                return null;
            }

            string filename = "";
            try
            {
                var extension = Path.GetExtension(file.FileName);
                filename = DateTime.Now.Ticks.ToString() + extension;

                // Save images to wwwroot/images
                var relativePath = Path.Combine("wwwroot", "images", "books");

                // Combine the relative path with the current directory
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(filepath, filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it)
                Console.WriteLine(ex.Message);
            }
            return $"/images/books/{filename}"; // Return the relative URL path
        }


        private async Task<string?> GetPdfUrl(IFormFile? file)
        {
            if (file == null)
            {
                return null;
            }

            string filename = "";
            try
            {
                var extension = Path.GetExtension(file.FileName);
                filename = DateTime.Now.Ticks.ToString() + extension;

                // Save PDFs to wwwroot/pdf
                var relativePath = Path.Combine("wwwroot", "pdf");
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(filepath, filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception if needed
                Console.WriteLine(ex.Message);
            }
            // Return the relative URL path for the saved PDF
            return $"/pdf/{filename}";
        }


        private async Task<string?> GetVoiceUrl(IFormFile? file)
        {
            if (file == null)
            {
                return null;
            }

            string filename = "";
            try
            {
                var extension = Path.GetExtension(file.FileName);
                filename = DateTime.Now.Ticks.ToString() + extension;

                // Save voices to wwwroot/voices
                var relativePath = Path.Combine("wwwroot", "voices");
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), relativePath);

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(filepath, filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception if needed
                Console.WriteLine(ex.Message);
            }
            // Return the relative URL path for the saved voice
            return $"/voices/{filename}";
        }


    }
}
