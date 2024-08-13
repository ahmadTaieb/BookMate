using BookMate.DataAccess.Data;
//using BookMate.DataAccess.Migrations;
using BookMate.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;


[Authorize]
[ApiController]
[Route("api/[controller]")]


public class LibraryController : ControllerBase
{
    private readonly ILibraryService _libraryService;
    private readonly ApplicationDbContext _applicationDbContext;
    public LibraryController(ILibraryService libraryService,ApplicationDbContext applicationDbContext)
    {
        _libraryService = libraryService;
        _applicationDbContext = applicationDbContext;
    }

    [HttpPost("/AddToReadBook")]
    public async Task<IActionResult> AddToReadBook([FromBody] Guid bookId)
    {

        string status = "ToRead";
        try
        {
            // Extract the user ID from the token
            var email= User.FindFirstValue(ClaimTypes.Email);

            string?userId = _applicationDbContext.Users.FirstOrDefault(u => u.Email == email)?.Id;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is not available in the token.");
            }
            



            await _libraryService.AddBookToLibrary(userId, bookId,status);
            return Ok("Book Add to library Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("/AddReadingBook")]
    public async Task<IActionResult> AddReadingBook([FromBody] Guid bookId)
    {

        string status = "Reading";
        try
        {
            // Extract the user ID from the token
            var email = User.FindFirstValue(ClaimTypes.Email);

            string? userId = _applicationDbContext.Users.FirstOrDefault(u => u.Email == email)?.Id;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is not available in the token.");
            }




            await _libraryService.AddBookToLibrary(userId, bookId, status);
            return Ok("Book Add to library Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }


    [HttpPost("/AddReadBook")]
    public async Task<IActionResult> AddReadBook([FromBody] Guid bookId)
    {

        string status = "Read";
        try
        {
            // Extract the user ID from the token
            var email = User.FindFirstValue(ClaimTypes.Email);

            string? userId = _applicationDbContext.Users.FirstOrDefault(u => u.Email == email)?.Id;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is not available in the token.");
            }




            await _libraryService.AddBookToLibrary(userId, bookId, status);
            return Ok("Book Add to library Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }



    [HttpDelete("/RemoveBookFromLibrary")]
    public async Task <IActionResult> RemoveBook([FromBody] Guid bookId)
    {
        try
        {
            // Extract the user ID from the token
            var email = User.FindFirstValue(ClaimTypes.Email);

            string? userId = _applicationDbContext.Users.FirstOrDefault(u => u.Email == email)?.Id;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is not available in the token.");
            }




            await _libraryService.RemoveBookFromLibrary(userId,bookId);
            return Ok("Book Removed from your Library Successfully");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }


    }




    [HttpGet("/GetToReadBooks")]

    public async Task<IActionResult> GetToReadBooks()
    {

        string status = "ToRead";
        try
        {
            // Extract the user ID from the token
            var email = User.FindFirstValue(ClaimTypes.Email);

            string? userId = _applicationDbContext.Users.FirstOrDefault(u => u.Email == email)?.Id;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is not available in the token.");
            }

            List<BookResponse?> responses=await _libraryService.GetBooksByStatus(userId,status);

            return Ok(responses);



        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }



    [HttpGet("/GetReadingBooks")]

    public async Task<IActionResult> GetReadingBooks()
    {

        string status = "Reading";
        try
        {
            // Extract the user ID from the token
            var email = User.FindFirstValue(ClaimTypes.Email);

            string? userId = _applicationDbContext.Users.FirstOrDefault(u => u.Email == email)?.Id;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is not available in the token.");
            }

            List<BookResponse?> responses = await _libraryService.GetBooksByStatus(userId, status);

            return Ok(responses);



        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpGet("/GetReadBooks")]

    public async Task<IActionResult> GetReadBooks()
    {

        string status = "Read";
        try
        {
            // Extract the user ID from the token
            var email = User.FindFirstValue(ClaimTypes.Email);

            string? userId = _applicationDbContext.Users.FirstOrDefault(u => u.Email == email)?.Id;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is not available in the token.");
            }

            List<BookResponse?> responses = await _libraryService.GetBooksByStatus(userId, status);

            return Ok(responses);



        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }

    }







}

