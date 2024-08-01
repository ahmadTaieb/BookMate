using Azure.Core;
using BookMate.DataAccess.Data;
using BookMate.DataAccess.IRepository;
using BookMate.Entities;
using BookMate.Entities.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using System.Security.Claims;

namespace book_mate.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class ReactController : ControllerBase
    {
        private Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private IClubService _clubService;
        private IUnitOfWork _unitOfWork;
        private IPostService _postService;
        private IReactService _reactService;
        private IUserService _userService;
        private ApplicationDbContext _db;

        public ReactController(ApplicationDbContext db, IClubService clubService, IUnitOfWork unitOfWork, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager, IPostService postService, IReactService reactService, IUserService userService)
        {
            _userManager = userManager;
            _clubService = clubService;
            _unitOfWork = unitOfWork;
            _postService = postService;
            _reactService = reactService;
            _userService = userService;
            _db = db;
        }

        [Authorize]
        [HttpPost("react/{postId}")]
        public async Task<IActionResult> react([FromRoute]Guid postId, [FromBody] int number)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);

            Post post =await _postService.GetAsync(postId);

            bool isMember = await _clubService.CheckIfMember(user.Id, post.ClubId.ToString());
            if (!isMember)
            {
                return new JsonResult(new { status = 400, message = "you are not member in this club" });
            }

            Reaction reaction = new Reaction();
            if (number == 0)
                reaction = Reaction.Like;
            else if (number == 1)
                reaction = Reaction.Love;
            else if(number == 2)
                reaction = Reaction.Laugh;
            else 
                reaction = Reaction.Sad;
            ReactAddRequest reactAddRequest = new ReactAddRequest
            {
                Reaction = reaction,
                ApplicationUserId = user.Id,
                PostId = postId,
            };

            React react = await _reactService.CreateAsync(reactAddRequest);
            if(react != null)
            {
                return new JsonResult(new {status = 200 , data =  react });
            }
            return new JsonResult(new { status = 400, data = "failed" });

        }


        [Authorize]
        [HttpDelete("deleteReact/{postId}")]
        public async Task<IActionResult> deleteReact([FromRoute] Guid postId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);

            React react = await _reactService.GetAsync(user.Id, postId);
            React r = await _reactService.DeleteAsync(react.Id);
           
            return new JsonResult(new { status = 200, data = react });
        }

        //[HttpGet("getReacts/{postId}")]
        //public async Task<IActionResult> getReacts([FromRoute]Guid postId)
        //{
        //    List<ReactResponse> list =await _reactService.GetAllResponseAsync(postId);
        //    return new JsonResult(new { status = 200, data = list });
        //}


        [HttpGet("getReactsCount/{postId}")]
        public async Task<IActionResult> getReactsCount([FromRoute] Guid postId)
        {
            int[] arr =await _reactService.GetCountAsync(postId);
            return new JsonResult(new { status = 200 , total = arr[0] + arr[1] + arr[2] + arr[3] ,Like = arr[0] ,Love = arr[1] ,Laugh = arr[2] ,Sad = arr[3] });
        }
        [HttpGet("getUsersReact/{postId}")]
        public async Task<IActionResult> getUsersReact([FromRoute]Guid postId)
        {
            return new JsonResult(_unitOfWork.React.GetAllUsersReact(postId).Result);
        }


    }
}

