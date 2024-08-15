using BookMate.DataAccess.IRepository;
using BookMate.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using Services;
using System.Security.Claims;

namespace book_mate.Controllers
{
    [Authorize]
    public class UserController : ControllerBase
    {
        private Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private IUserService _userService;
        private IUnitOfWork _unitOfWork;
        public UserController(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager, IUserService userService, IUnitOfWork unitOfWork)
        {   
            _userManager = userManager;
            _userService = userService;
            _unitOfWork = unitOfWork;
        
        }
        [Authorize]
        [HttpGet("getCurrentUser")]
        public async Task<IActionResult> getCurrentUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);

            return new JsonResult(new { status = "200", message = "successfully" ,data = user });
        }
        [AllowAnonymous]
        [HttpGet("getUser/{id}")]
        public async Task<IActionResult> getCurrentUser(string id)
        {
            var user = _userService.GetUserAsync(id);

            return new JsonResult(new { status = "200", message = "successfully", data = user.Result });
        }
        [AllowAnonymous]
        [HttpPost("searchUser")]
        public async Task<IActionResult> SearchClubByName([FromBody] string search)
        {
            var AllUsers = _userService.GetAllUsersAsync();
            var users = AllUsers.Result.Where(o => o.Name.ToLower().Contains(UserName.Trim().ToLower()));

            return new JsonResult(new { status = 200, message = "success", data = users });

        }
        [HttpDelete("deleteUserFromAdmin")]
        public async Task<IActionResult> deleteUser(string id)
        {
            ApplicationUser user =await _userService.GetUserAsync(id);

            await _unitOfWork.ApplicationUser.Delete(user);
            _unitOfWork.save();
            return Ok();
        }

    }
}
