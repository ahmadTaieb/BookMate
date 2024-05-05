using BookMate.DataAccess.IRepository;
using BookMate.Entities;
using Microsoft.AspNet.Identity;
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
    public class TestController : ControllerBase
    {
        private Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private IUserService _userService;
        private IUnitOfWork _unitOfWork;
        public TestController(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager,IUserService userService,IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _userService = userService;
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);


            return Ok("Hello...");
        }

        [HttpPost("updateUser")]
        public async Task<IActionResult> updateUser([FromQuery]ApplicationUserUpdateRequest userAddRequest)
        {
             
            if (userAddRequest == null) 
            {
                return Ok();
            }
            var userEmail = User.FindFirstValue(ClaimTypes.Email); // will give the user's userId
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);

            await _userService.UpdateUserAsync(user.Id,userAddRequest);
            _unitOfWork.saveAsync();

            return Ok();
        }
        [HttpGet("deleteUser")]
        public async Task<IActionResult> deleteUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email); // will give the user's userId
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);

            _userService.DeleteUserAsync(user);
            _unitOfWork.saveAsync();
            return Ok();
        }

        [HttpGet("getAllUsers")]
        [AllowAnonymous]
        public async Task<List<ApplicationUser>> getAll()
        {
            return await _userService.GetAllUsersAsync();
        }

    }
}
