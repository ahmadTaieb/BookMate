using BookMate.DataAccess.IRepository;
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
    public class FollowController : ControllerBase
    {
        private IFollowService _followService;
        private Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private IUserService _userService;
        public FollowController (IFollowService followService, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager, IUserService userService) 
        {
            _followService = followService;
            _userManager = userManager;
            _userService = userService;
        }
        [Authorize]
        [HttpGet("Follow/{id}")]
        public async Task<IActionResult> Follow([FromRoute]string id)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);

            ApplicationUserRelation relation = new ApplicationUserRelation();
            relation.ApplicationUserParentId = user.Id;
            relation.ApplicationUserChildId = id; 

            _followService.FollowAsync(relation);

            return new JsonResult(new { status = 200 , message = $"successfully user with id {user.Id} followed {id}"  });
        }

        [Authorize]
        [HttpGet("UnFollow/{id}")]
        public async Task<IActionResult> UnFollow([FromRoute] string id)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);

            ApplicationUserRelation relation = new ApplicationUserRelation();
            relation.ApplicationUserParentId = user.Id;
            relation.ApplicationUserChildId = id;

            _followService.UnFollowAsync(relation);

            return new JsonResult(new { status = 200, message = $"successfully user with id {user.Id} Unfollowed {id}" });
        }
        //[Authorize]
        [HttpGet("getFollowers/{id}")]
        public async Task<IActionResult> getFollowers([FromRoute] string id)
        {
            //var userEmail = User.FindFirstValue(ClaimTypes.Email);
            //ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);

            var users =await _followService.GetFollowRequestsAsync(id);

            List<ApplicationUserResponse> response = new List<ApplicationUserResponse>();
            foreach (var u in users)
            {
                response.Add(new ApplicationUserResponse { Id = u.Id, Name = u.Name });
            }

            return new JsonResult(new { status = 200, message = "success", data = response });

        }

        [HttpGet("getFollowing/{id}")]
        public async Task<IActionResult> getFollowing([FromRoute] string id)
        {
            //var userEmail = User.FindFirstValue(ClaimTypes.Email);
            //ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);

            var users = await _followService.GetFollowingAsync(id);

            List<ApplicationUserResponse> response = new List<ApplicationUserResponse>();
            foreach (var u in users)
            {
                response.Add(new ApplicationUserResponse { Id = u.Id, Name = u.Name });
            }

            return new JsonResult(new { status = 200, message = "success", data = response });

        }

        [HttpGet("getFollowersNumber/{id}")]
        public async Task<IActionResult> getFollowersNumber([FromRoute] string id)
        {
            //var userEmail = User.FindFirstValue(ClaimTypes.Email);
            //ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);

            var users = await _followService.GetFollowRequestsAsync(id);

            

            return new JsonResult(new { status = 200, message = "success", data = users.Count });

        }

        [HttpGet("getFollowingNumber/{id}")]
        public async Task<IActionResult> getFollowingNumber([FromRoute] string id)
        {
            //var userEmail = User.FindFirstValue(ClaimTypes.Email);
            //ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);

            var users = await _followService.GetFollowingAsync(id);



            return new JsonResult(new { status = 200, message = "success", data = users.Count });

        }
        [Authorize]
        [HttpGet("checkIfFollowing/{id}")]
        public async Task<IActionResult> check([FromRoute] string id )
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);

            ApplicationUserRelation relation = new ApplicationUserRelation();
            relation.ApplicationUserParentId = user.Id;
            relation.ApplicationUserChildId = id;

            bool res =await _followService.IsFollowing(relation);

            return new JsonResult(res);

        }

    }
}
