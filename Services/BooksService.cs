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

            return _db.Books.Select(book=>book.ToBookResponse()).ToList();
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

            Book book = bookAddRequest.ToBook();
            book.Id=Guid.NewGuid();
            book.AverageRating = 0;
            book.RatingsCount = 0;

            _db.Books.Add(book);
            _db.SaveChanges();


            return book.ToBookResponse();
        }

        public List<BookResponse> GetBooksByCategory(string? Category)
        {

            if (Category == null)
                return new List<BookResponse>();

            List<BookResponse> books = _db.Books.Where(temp => temp.Category == Category).Select(temp=>temp.ToBookResponse()).ToList();
            if (books.Count ==0 ) {
                return new List<BookResponse>();
            }
            

            return books;
        }
    }
}
