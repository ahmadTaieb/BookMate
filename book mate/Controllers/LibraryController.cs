using BookMate.DataAccess.Data;
using BookMate.DataAccess.Migrations;
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

    [HttpPost("add")]
    public async Task<IActionResult> Add([FromBody] AddBookToLibrary book)
    {
        try
        {
            // Extract the user ID from the token
            var email= User.FindFirstValue(ClaimTypes.Email);

            string?userId = _applicationDbContext.Users.FirstOrDefault(u => u.Email == email)?.Id;

            if (string.IsNullOrEmpty(userId))
            {
                return BadRequest("User ID is not available in the token.");
            }
            ReadingStatus status;
           

            await _libraryService.AddBookToLibrary(userId, book.bookId,book.ReadingStatus);
            return Ok("done");
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

