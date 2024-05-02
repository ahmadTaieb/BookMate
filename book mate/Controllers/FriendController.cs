using BookMate.DataAccess.Data;
using BookMate.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using System.Security.Claims;

namespace book_mate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FriendController : ControllerBase
    {
        private readonly ApplicationDbContext _db;
        private readonly IUserService _userService;
        private readonly UserManager<ApplicationUser> _userManager;
        public FriendController(ApplicationDbContext db,IUserService userService, UserManager<ApplicationUser> userManager) 
        {
            _db = db;
            _userService = userService;
            _userManager = userManager;
        }

        //[HttpPost("AddFriend")]
        //public async Task<IActionResult> AddFriend(string friendId)
        //{
        //    //var claimsIdentity = (ClaimsIdentity)User.Identity;
        //    //var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

        //    //var user = await _userManager.FindByIdAsync(claims.Value);
        //    //var user = await _userManager.GetUserAsync(User);
        //    //var Id = User;
        //    var userEmail = User.FindFirstValue(ClaimTypes.Email); // will give the user's userId
        //    ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);

        //    if (user == null)
        //        return BadRequest();
        //    var userId = user.Id;
           
        //    ApplicationUserApplicationUser f = new() {
        //        ApplicationUserChildId = userId,
        //        ApplicationUserParentId = friendId,
        //        confirm = false
        //    };
        //    var result =  _db.Friends.Add(f);
        //    await _db.SaveChangesAsync();

        //    return Ok(result);
        //}
        [HttpGet("getId")]
        public async Task<IActionResult> GetCurrentId()
        {

            var userEmail = User.FindFirstValue(ClaimTypes.Email); // will give the user's userId
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);
            var Id = user.Id;
            if(Id != null)
                return Ok(Id);
            return BadRequest();
        }

        
    }
}
