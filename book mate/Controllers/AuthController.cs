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
        private IFavoritesService _favoritesService;
        private IEmailService _emailService;
        public AuthController(IEmailService emailService ,Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager, IUserService userService, IUnitOfWork unitOfWork, ApplicationDbContext db, ILibraryService libraryService,IFavoritesService favoriteService)
        {
            _userManager = userManager;
            _userService = userService;
            _unitOfWork = unitOfWork;
            _db = db;
            _libraryService = libraryService;
            _favoritesService = favoriteService;
            _emailService = emailService;
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
            await _favoritesService.CreateFavorite(result.Id);

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
        [HttpDelete("deleteUser")]
        public async Task<IActionResult> deleteUser()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email); // will give the user's userId
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);

            //await _unitOfWork.ApplicationUser.Delete(user);
            //await _userService.DeleteUserAsync(user);
            //var x =await _userManager.DeleteAsync(user);
            //if (x!=null)
            //{
                
            //}
            await _unitOfWork.ApplicationUser.Delete(user);
            _unitOfWork.save();
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
        [HttpGet("getUser/{id}")]
        public async Task<IActionResult> getUser([FromRoute] string id)
        {
            ApplicationUser user =await _userService.GetUserAsync(id);
            return new JsonResult(user);

        }

        [HttpPost("request-reset-password")]
        public async Task<IActionResult> RequestResetPassword([FromBody] ResetPasswordRequestModel model)
        {
            if (!ModelState.IsValid)
                return new JsonResult(new { message = ModelState });

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return new JsonResult(new { message = "User not found." });

            
            var resetCode = new Random().Next(100000, 999999).ToString();

            
            user.PasswordResetCode = resetCode;
            user.ResetCodeExpiration = DateTime.UtcNow.AddMinutes(15); 
            

            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            user.TokenReset = resetToken;
            user.TokenResetExpiration = DateTime.UtcNow.AddMinutes(15);

            await _userManager.UpdateAsync(user);
            await _emailService.SendEmailAsync(user.Email, "Reset Password Code", $"Your password reset code is: {resetCode}");

            return new JsonResult(new { token = resetToken , message = "OTP send to your email" });
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
                new JsonResult(new { message = ModelState });

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                new JsonResult(new {  message = "user Not Found!" });


            if (user.TokenReset != model.Token || user.TokenResetExpiration < DateTime.UtcNow)
                return new JsonResult(new { message = "Invalid or expired token." });

            if (user.PasswordResetCode != model.ResetCode || user.ResetCodeExpiration < DateTime.UtcNow)
                return new JsonResult(new { message = "Invalid or expired OTP code." });


            return new JsonResult(new { status = 200 , message = "success" });


            
        }
        [HttpPost("changePassword")]
        public async Task<IActionResult> changePassword([FromBody] ChangePasswordDTO model) 
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return new JsonResult(new { message = "User not found." });

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
            if (!result.Succeeded)
                return new JsonResult(new { result.Errors });


            user.PasswordResetCode = null;
            user.ResetCodeExpiration = null;
            user.TokenReset = null;
            user.TokenResetExpiration = null;
            await _userManager.UpdateAsync(user);

            TokenRequest x = new TokenRequest
            {
                Email = model.Email,
                Password = model.Password,
            };
            var r = await _userService.GetTokenAsync(x);

            if (!r.IsAuthenticated)
                return new JsonResult(new { status = 400, message = r.Message });

            

            return new JsonResult(new { status = 200, message = "success", token = r.Token , id = user.Id });
        }


    }
}