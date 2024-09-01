using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services;
using BookMate.DataAccess.Data;
using ServiceContracts ;
using Microsoft.AspNetCore.Http;

using System;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using ServiceContracts.DTO;
using NuGet.Protocol;
using BookMate.Entities;
using Azure.Core;
using NuGet.Protocol.Plugins;
using System.Security.Claims;
namespace book_mate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookWebController : ControllerBase
    {

        private readonly IBooksService _booksService;
        private ApplicationDbContext? applicationDbContext;

        public BookWebController(IBooksService booksService) { 

            _booksService = booksService;

        }


        
        



        
        [HttpGet]
        [Route("/Books")]
        public IActionResult GetAllBooks( )
        {

        

            List<BookResponse> Books = _booksService.GetAllBooks(null);
            
            return Ok(Books);


        }


        [HttpDelete]
        [Route("/DeleteBook")]
        public IActionResult DeleteBook([FromBody] BookAddRequest request)
        {
            if (!string.IsNullOrEmpty(request.Title))
            {
                // If title is provided, return the book with that title
                BookResponse? response = _booksService.GetBookByBookTitle(request.Title);

                if (response != null)
                {
                    _booksService.DeleteBook(request.Title);
                    return Ok("Book deleted successfully");
                }
            }
           
                return NotFound();

        }


        [HttpGet]
        [Route("/Books/{Title}")]
        public IActionResult GetBookByTitle(string Title)
        {
            if (!string.IsNullOrEmpty(Title))
            {
                // If title is provided, return the book with that title
                BookResponse? response = _booksService.GetBookByBookTitle(Title);
                if (response != null)
                {
                    return Ok(response);
                }
            }

            return NotFound();

        }









        [HttpGet("/Books/categories")]
        public async Task<ActionResult<List<BookResponse?>>>? GetBooksByCategory([FromQuery] List<string> categoriesName )
        {
            if (categoriesName == null || categoriesName.Count == 0)
            {
                return BadRequest("Category IDs must be provided.");
            }

           
            var books = await _booksService.GetBooksByCategory(categoriesName,null);

            if (books.Count == 0)
            {
                return NotFound("No books found for the specified categories.");
            }
            return Ok(books);
        }







        [HttpPost]
        [Route("/addBook")]
        public async Task<IActionResult> addBook([FromForm] BookAddRequest? model)
        {

            if(model == null)
            {
                return BadRequest("The Title filed is required");
            }




            try
            {
                await _booksService.AddBook(model);

                return Ok("The book has been added successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while adding the book");
            }

        }



        [HttpPost]
        [Route("/editBook/{Title}")]
        public async Task<IActionResult> EditBook( string Title , [FromForm] BookAddRequest editedBook)
        {
            
            if(editedBook == null)
            {

                return BadRequest("Edited Book is null");
            }

            if(Title ==null)
            {
                return BadRequest("Book id is null");
            }
            try
            {
                await _booksService.EditBookAsync(Title, editedBook); // Await the EditBookAsync method call

                return Ok("Book edited successfully");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while editing the book");
            }
        }

        [HttpGet]
        [Route("/SearchByTitle")]
        public async Task<ActionResult<List<BookResponse?>>>? Search([FromQuery]string title)
        {

           var response = await _booksService.Search(title);

            return Ok(response);
           
        }

        [HttpGet]
        [Route("/AddReadingCount")]
        public async Task<IActionResult> addReadingCount([FromQuery]int num,Guid bookId)
        {
            
            _booksService.AddReadingCount(num, bookId);
            return Ok("done");
               


        }


















    }






}

