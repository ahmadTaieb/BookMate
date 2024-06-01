﻿using Microsoft.AspNetCore.Http;
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
