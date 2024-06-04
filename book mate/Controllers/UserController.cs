using BookMate.DataAccess.IRepository;
using BookMate.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
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
        [HttpGet("Follow/{id}")]
        public async Task<IActionResult> Follow()  
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email); 
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);



            return new JsonResult (new { status = "200" , message = "successfully followed!" });
        }

    }
}
