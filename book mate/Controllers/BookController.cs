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
namespace book_mate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookController : ControllerBase
    {

        private readonly IBooksService _booksService;
        private ApplicationDbContext applicationDbContext;

        public BookController(IBooksService booksService) { 

            _booksService = booksService;

        }


        
        



        
        [HttpGet]
        [Route("/Books")]
        public IActionResult GetBookByTitle([FromQuery] string title = null, [FromQuery] Guid id = default(Guid) )
        {
            if (!string.IsNullOrEmpty(title))
            {
                // If title is provided, return the book with that title
                BookResponse? response = _booksService.GetBookByBookTitle(title);
                if (response != null)
                {
                    return Ok(response);
                }
            } 

            if (id != Guid.Empty)
            {
                // If id is provided, return the book with that id
                BookResponse? response = _booksService.GetBookByBookId(id);
                if (response != null)
                {
                    return Ok(response);
                }
            }

           


            List<BookResponse> Books = _booksService.GetAllBooks();

            return Ok(Books);


        }


        [HttpGet("/Books/categories")]
        public async Task<ActionResult<List<BookResponse>>> GetBooksByCategory([FromQuery] List<string> categoriesName )
        {
            if (categoriesName == null || categoriesName.Count == 0)
            {
                return BadRequest("Category IDs must be provided.");
            }

           
            var books = await _booksService.GetBooksByCategory(categoriesName);
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
        [Route("editBook")]
        public async Task<IActionResult> EditBook([FromQuery] Guid? id, [FromForm] BookAddRequest editedBook)
        {
            
            if(editedBook == null)
            {

                return BadRequest("Edited Book is null");
            }

            if(id ==null)
            {
                return BadRequest("Book id is null");
            }
            try
            {
                await _booksService.EditBookAsync(id, editedBook); // Await the EditBookAsync method call

                return Ok("Book edited successfully");

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while editing the book");
            }
        }



        [HttpPost("increment-reading-count/{bookId}")]
        public async Task<ActionResult<BookResponse>> IncrementReadingCount(Guid bookId)
        {
            try
            {
                await _booksService.IncrementReadingCount(bookId);
                return Ok("done");
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Book not found");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine("Invalid operation exception: " + ex.Message);
                return StatusCode(500, ex.Message);
            }
            catch (Exception ex)
            {
           
                return StatusCode(500, "An unexpected exception occurred: " + ex.Message);
            }
        }















    }






}

