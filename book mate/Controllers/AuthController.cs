using BookMate.DataAccess.Data;
using BookMate.DataAccess.IRepository;
using BookMate.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
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
        public async Task<IActionResult> RegisterAsync([FromBody] RegisterDTO model)
        {
            if(!model.Email.Contains("@"))
            {
                return new JsonResult(new { status = 400, message = "email not valid" });
            }
            if (model.Password.Length <= 5 )
            {
                return new JsonResult(new { status = 400, message = "password must have at least 6 charecter" });
            }
            if(model.Name == null)
            {
                return new JsonResult(new { status = 400, message = "name is requied" });
            }

            var result = await _userService.RegisterAsync(model);

            if (!result.IsAuthenticated)
            {
                return new JsonResult(new {status = 400 , message = result.Message });
            }

            //SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            await _libraryService.CreateLibrary(result.Id);

            return new JsonResult(new { status = 200, message = "successfully!",token = result.Token ,email = model.Email,id = result.Id });
        }

        [HttpPost("login")]
        public async Task<IActionResult> GetTokenAsync([FromBody] TokenRequest model)
        {
            if (!model.Email.Contains("@"))
            {
                return new JsonResult(new { status = 400, message = "email not valid" });
            }
            if (model.Password.Length <= 5)
            {
                return new JsonResult(new { status = 400, message = "password must have at least 6 charecter" });
            }
            
            var result = await _userService.GetTokenAsync(model);
            
            if (!result.IsAuthenticated)
                return new JsonResult(new {status = 400 , message = result.Message });
            ApplicationUser user = await _userManager.FindByEmailAsync(model.Email);
            //if (!string.IsNullOrEmpty(result.RefreshToken))
            //    SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

            return new JsonResult(new { status = 200 , message = "successfully!" ,token = result.Token, email = model.Email, id = user.Id });
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

        //[HttpGet("refreshToken")]
        //public async Task<IActionResult> RefreshToken()
        //{
        //    var refreshToken = Request.Cookies["refreshToken"];

        //    var result = await _userService.RefreshTokenAsync(refreshToken);

        //    if (!result.IsAuthenticated)
        //        return BadRequest(result);

        //    //SetRefreshTokenInCookie(result.RefreshToken, result.RefreshTokenExpiration);

        //    return Ok(result);
        //}
        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> RevokeToken()
        {

            //if (string.IsNullOrEmpty(s))
            //    return new JsonResult(new { status = 400,message = "Token is required!"});

            //var result = await _userService.RevokeTokenAsync(s);

            //if (!result)
            //    return new JsonResult(new { status = 400, message = result });

            return new JsonResult(new { status = 200, message = "successfully!" });
            
            
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
        public async Task<IActionResult> updateUser([FromBody] ApplicationUserUpdateRequest? userUpdateRequest)
        {

            if (userUpdateRequest == null)
            {
                return new JsonResult(new { status = 200, message = "successfully! nothing changed" });
            }
            var userEmail = User.FindFirstValue(ClaimTypes.Email); // will give the user's userId
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);
            var pass = userUpdateRequest.currentPassword;

            if (!await _userManager.CheckPasswordAsync(user, userUpdateRequest.currentPassword))
                return new JsonResult(new { status = 400, message = "incorrect password!" });
            if (userUpdateRequest.Password != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, userUpdateRequest.currentPassword, userUpdateRequest.Password);
                pass = userUpdateRequest.Password;
            }

            await _userService.UpdateUserAsync(user, userUpdateRequest);
            _unitOfWork.saveAsync();
            var token = await _userService.GetTokenAsync(new TokenRequest { Email = user.Email, Password = pass });

            return new JsonResult(new { status = 200, message = "updated successfully!", newToken = token.Token, user_id = user.Id, email = user.Email });
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

        [Authorize]
        [HttpGet("isAuth")]
        public async Task<IActionResult> isAuth()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email); // will give the user's userId
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);

            return new JsonResult(new { user });

        }
    }
}