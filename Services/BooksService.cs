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

                book.Categories = _db.Categories.Where(c => editedBook.CategoriesNames.Contains(c.categoryName)).ToList();


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
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                filename = DateTime.Now.Ticks.ToString() + extension;

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "C:\\dotNet\\book mate\\BookMate.DataAccess\\Upload\\Books\\Images\\");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(Directory.GetCurrentDirectory(), "C:\\dotNet\\book mate\\BookMate.DataAccess\\Upload\\Books\\Images\\", filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
            }
            return filename;
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
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                filename = DateTime.Now.Ticks.ToString() + extension;

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "C:\\dotNet\\book mate\\BookMate.DataAccess\\Upload\\Books\\Pdfs\\");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(Directory.GetCurrentDirectory(), "C:\\dotNet\\book mate\\BookMate.DataAccess\\Upload\\Books\\Pdfs\\", filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
            }
            return filename;
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
                var extension = "." + file.FileName.Split('.')[file.FileName.Split('.').Length - 1];
                filename = DateTime.Now.Ticks.ToString() + extension;

                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "C:\\dotNet\\book mate\\BookMate.DataAccess\\Upload\\Books\\Voices\\");

                if (!Directory.Exists(filepath))
                {
                    Directory.CreateDirectory(filepath);
                }

                var exactpath = Path.Combine(Directory.GetCurrentDirectory(), "C:\\dotNet\\book mate\\BookMate.DataAccess\\Upload\\Books\\Voices\\", filename);
                using (var stream = new FileStream(exactpath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception ex)
            {
            }
            return filename;
        }


    }
}
