using BookMate.DataAccess.Data;
using BookMate.DataAccess.IRepository;
using BookMate.Entities;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private ApplicationDbContext _db;
        public TestController(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager,IUserService userService,IUnitOfWork unitOfWork,ApplicationDbContext db)
        {
            _userManager = userManager;
            _userService = userService;
            _unitOfWork = unitOfWork;
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetData()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);


            return Ok("Hello...");
        }

        
        
        

    }
}
