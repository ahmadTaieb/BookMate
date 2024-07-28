using BookMate.DataAccess.Data;
using BookMate.DataAccess.IRepository;
using BookMate.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using ServiceContracts;
using ServiceContracts.DTO;
using System.Security.Claims;

namespace book_mate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {

        private Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private IClubService _clubService;
        private IUnitOfWork _unitOfWork;
        private IPostService _postService;
        private ApplicationDbContext _db;

        public PostController(ApplicationDbContext db, IClubService clubService, IUnitOfWork unitOfWork, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager, IPostService postService)
        {
            _userManager = userManager;
            _clubService = clubService;
            _unitOfWork = unitOfWork;
            _postService = postService;
            _db = db;
        }

        

        [HttpGet("getPost/{id}")]
        public async Task<IActionResult> getPost([FromRoute] string id)
        {
            Post post =await _postService.GetAsync(new Guid(id));
            return new JsonResult(new { status = 200,data = post });
        }

        [HttpGet("getPosts/{clubId}")]
        public async Task<IActionResult> getPosts([FromRoute] string clubId)
        {
            var posts = _postService.GetAllAsync(new Guid(clubId));
            return new JsonResult(new { status = 200, data = posts.Result });
        }
        [Authorize]
        [HttpPost("createPost")]
        public async Task<IActionResult> createPost([FromBody] PostAddRequest request)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);

            PostAddRequest p = request;
            p.ApplicationUserId = user.Id;

            var post = await _postService.CreateAsync(p);
            return new JsonResult(new { status = 200, data = post });
        }

        [Authorize]
        [HttpPost("updatePost/{id}")]
        public async Task<IActionResult> updatePost([FromRoute] string id,PostAddRequest request)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);

            var post = await _postService.UpdateAsync(new Guid(id), request);
            return new JsonResult(new {status = 200, data = post });

        }
        [Authorize]
        [HttpDelete("deletePost/{id}")]
        public async Task<IActionResult> deletePost([FromRoute] string id)
        {
            var ok = await _postService.DeleteAsync(new Guid(id));
            if(ok != null) 
                return new JsonResult(new { status = 200,message = $"success deleted {ok}"});
            return new JsonResult(new { status = 400, message = "failed" });
        }

    }
}
