using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookMate.Entities;
using ServiceContracts.DTO;
using ServiceContracts;
using Services;
using Xunit;
using BookMate.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
namespace BookMateTests
{
    public class BookServiceTest
    {
        private readonly IBooksService _booksService ;
        private ApplicationDbContext applicationDbContext;

        public BookServiceTest()
        {
            _booksService = new BooksService(new ApplicationDbContext(new DbContextOptionsBuilder<ApplicationDbContext>().Options));
        }


        #region AddBook

        [Fact]
        public void AddBook_NullBook()
        {
            //Arrange
            BookAddRequest? request = null;

            //Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                //Act
                _booksService.AddBook(request);
            });

         
           
        }

        [Fact]
        public void AddBook_TitleIsNull() { 

            BookAddRequest request = new BookAddRequest()
            {
                Title=null
            };

            Assert.Throws<ArgumentException>(() =>
            {
                _booksService.AddBook(request);
            });
            
        
        }

        [Fact]
        public void AddBook_DuplicateCountryName()
        {
            BookAddRequest? request1 = new BookAddRequest()
            {
                Title = "temp",
                Author="aa",

            };
            BookAddRequest? request2 = new BookAddRequest()
            {
                Title = "temp",
                Author ="aa"
            };

            Assert.Throws<ArgumentException>(() =>
            {

                _booksService.AddBook(request1);
                _booksService.AddBook(request2);
            });
        

        }

        [Fact]
        public void AddBook_IdIsNotNull () {

            BookAddRequest? request = new BookAddRequest()
            {
                Title="temp"
            };

            BookResponse response = _booksService.AddBook(request);

            Assert.True(response.Id!=Guid.Empty);
        }
        

        #endregion



    }
}
