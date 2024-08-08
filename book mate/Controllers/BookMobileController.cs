using BookMate.DataAccess.Data;
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
        public async Task<ActionResult<List<BookResponse>>> GetBooksByCategory([FromBody] List<string?>? CategoriesNames)
        {

            var email = User.FindFirstValue(ClaimTypes.Email);

            string? userId = _applicationDbContext.Users.FirstOrDefault(u => u.Email == email)?.Id;



            if (CategoriesNames == null || CategoriesNames.Count == 0)
            {
                List<BookResponse> Books = _booksService.GetAllBooks(userId);

                return Ok(Books);
            }
           

            var  books = await _booksService.GetBooksByCategory(CategoriesNames,userId);
            if (books.Count == 0)
            {
                return NotFound("No books found for the specified categories.");
            }
            return Ok(books);
        }

        [HttpGet]
        [Route("/SearchBooks")]
        public async Task<ActionResult<List<BookResponse?>>>? Search([FromQuery] string title)
        {

            var response = await _booksService.Search(title);

            return Ok(response);

        }

    }
}
