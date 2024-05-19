using BookMate.DataAccess.Data;
using BookMate.DataAccess.IRepository;
using BookMate.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using ServiceContracts;
using ServiceContracts.DTO;
using System.Security.Claims;

namespace book_mate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private IUserService _userService;
        private IUnitOfWork _unitOfWork;
        private ApplicationDbContext _db;

        private ILibraryService _libraryService;
        public AuthController(Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager, IUserService userService, IUnitOfWork unitOfWork, ApplicationDbContext db, ILibraryService libraryService)
        {
            _userManager = userManager;
            _userService = userService;
            _unitOfWork = unitOfWork;
            _db = db;
            _libraryService = libraryService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> RegisterAsync([FromQuery] RegisterDTO model)
        {
            if (!ModelState.IsValid)
                return BadRequest(new JsonResult(ModelState));

            var result = await _userService.RegisterAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(new JsonResult(result.Message));

            //SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok("successfully!!");
        }

        [HttpPost("login")]
        public async Task<IActionResult> GetTokenAsync([FromQuery] TokenRequest model)
        {
            var result = await _userService.GetTokenAsync(model);

            if (!result.IsAuthenticated)
                return BadRequest(new JsonResult(result.Message));

            if (!string.IsNullOrEmpty(result.RefreshToken))
                SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(new JsonResult(result));
        }

        [HttpPost("addRole")]
        public async Task<IActionResult> AddRoleAsync([FromBody] string userId,string role)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var result = await _userService.AddRoleAsync(userId,role);

            if (!string.IsNullOrEmpty(result))
                return BadRequest(result);

            return Ok();
        }

        [HttpGet("refreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = Request.Cookies["refreshToken"];

            var result = await _userService.RefreshTokenAsync(refreshToken);

            if (!result.IsAuthenticated)
                return BadRequest(result);

            SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return Ok(result);
        }
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> RevokeToken([FromBody] string? t)
        {
            var token = t ?? Request.Cookies["refreshToken"];

            if (string.IsNullOrEmpty(token))
                return BadRequest(new JsonResult("Token is required!"));

            var result = await _userService.RevokeTokenAsync(token);

            if (!result)
                return BadRequest(new JsonResult("Token is invalid!"));

            return Ok();
        }

        private void SetRefreshTokenInCookie(string refreshToken, DateTime expires)
        {
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = expires.ToLocalTime(),
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.None
            };

            Response.Cookies.Append("refreshToken", refreshToken, cookieOptions);
        }
        [Authorize]
        [HttpPost("updateUser")]
        public async Task<IActionResult> updateUser([FromQuery] ApplicationUserUpdateRequest userAddRequest)
        {

            if (userAddRequest == null)
            {
                return Ok();
            }
            var userEmail = User.FindFirstValue(ClaimTypes.Email); // will give the user's userId
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);

            await _userService.UpdateUserAsync(user.Id, userAddRequest);
            _unitOfWork.saveAsync();

            return Ok();
        }
        [Authorize]
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