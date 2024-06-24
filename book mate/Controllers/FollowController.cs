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
        [HttpGet("getFollowRequests")]
        public async Task<IActionResult> getFollowRequests()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);

            var users =await _followService.GetFollowRequestsAsync(user.Id);

            List<ApplicationUserResponse> response = new List<ApplicationUserResponse>();
            foreach (var u in users)
            {
                response.Add(new ApplicationUserResponse { Id = u.Id, Name = u.Name });
            }

            return new JsonResult(new { status = 200, message = "success", data = response });

        }

    }
}
