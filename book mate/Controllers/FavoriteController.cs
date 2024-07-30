using BookMate.DataAccess.Data;
using BookMate.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using Services;
using System.Net;
using System.Security.Claims;

namespace book_mate.Controllers
{
    [Authorize] 
    [Route("api/[controller]")]
    [ApiController]
    public class FavoriteController : ControllerBase
    {

        private readonly IFavoritesService? _favoritesService;
        private readonly ApplicationDbContext _applicationDbContext;

        public FavoriteController(ApplicationDbContext applicationDbContext, IFavoritesService? favoritesService)
        {
            _favoritesService = favoritesService;
            _applicationDbContext = applicationDbContext;
        }

        [HttpPost]
        [Route("/addBookToFav")]
        public async Task<IActionResult> add([FromBody] Guid bookId)
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




                await _favoritesService.AddBookToFav(userId,bookId);
                return Ok("Book Add to library Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }


        [HttpGet]
        [Route("/GetFavoritesBooks")]
        public async Task<IActionResult> getFavoritesBooks()
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

                List<BookResponse?>? response = await _favoritesService.GetFavoriteBooks(userId);


              
                return Ok(response);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }


        }

        [HttpDelete]
        [Route("/RemoveBookFromFavorite")]
        public async Task<IActionResult> remove([FromBody] Guid bookId)
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




                await _favoritesService.RemoveBookFromFav(userId, bookId);
                return Ok("Book Remove from favorite  Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }



        }





    }
}
