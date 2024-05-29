﻿using BookMate.DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using System.Security.Claims;

namespace book_mate.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BookMobileController : ControllerBase
    {

        private readonly IBooksService _booksService;
        private ApplicationDbContext? _applicationDbContext;

        public BookMobileController(IBooksService booksService, ApplicationDbContext? applicationDbContext)
        {

            _booksService = booksService;
            _applicationDbContext = applicationDbContext;
        }
        

        [HttpGet]
        [Route("/GetAllBooks")]
        public IActionResult GetAllBooks()
        {
            var email = User.FindFirstValue(ClaimTypes.Email);

            string? userId = _applicationDbContext.Users.FirstOrDefault(u => u.Email == email)?.Id;



            List<BookResponse> Books = _booksService.GetAllBooks(userId);

            return Ok(Books);


        }


        [HttpGet("/Books/ByCategories")]
        public async Task<ActionResult<List<BookResponse>>> GetBooksByCategory([FromBody] List<string> categoriesName)
        {

            var email = User.FindFirstValue(ClaimTypes.Email);

            string? userId = _applicationDbContext.Users.FirstOrDefault(u => u.Email == email)?.Id;



            if (categoriesName == null || categoriesName.Count == 0)
            {
                return BadRequest("Category IDs must be provided.");
            }
           

            var books = await _booksService.GetBooksByCategory(categoriesName,userId);
            if (books.Count == 0)
            {
                return NotFound("No books found for the specified categories.");
            }
            return Ok(books);
        }

    }
}
