using BookMate.DataAccess.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using System.Security.Claims;

namespace book_mate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class RecommendationController : ControllerBase
    {

        private readonly IRecommendationService   _recommendationService;
        private ApplicationDbContext? _db;

        public RecommendationController(IRecommendationService recommendationService, ApplicationDbContext db)
        {
            _db = db;
            _recommendationService = recommendationService;
        }

        [HttpPost]
        [Route("AddFavoriteCategories")]
        public async Task<IActionResult> addFavoriteCategories([FromBody]List<string> categories)
        {
            try
            {
                // Extract the user ID from the token
                var email = User.FindFirstValue(ClaimTypes.Email);

                string? userId = _db.Users.FirstOrDefault(u => u.Email == email)?.Id;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User ID is not available in the token.");
                }


                await _recommendationService.AddFavoriteCategories(categories, userId);

                return Ok("categories added successfully");
            }
            catch (Exception ex){

                return BadRequest(ex.Message);

            }


        }


        [HttpGet]
        [Route("RecommendationsBooks")]

        public async Task<IActionResult> recommendationsBooks()
        {
            try
            {
                // Extract the user ID from the token
                var email = User.FindFirstValue(ClaimTypes.Email);

                string? userId = _db.Users.FirstOrDefault(u => u.Email == email)?.Id;

                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest("User ID is not available in the token.");
                }


                var response =await _recommendationService.RecommendationsBooks( userId);

                return Ok(response);
            }
            catch (Exception ex)
            {

                return BadRequest(ex.Message);

            }


        }



    }
}
