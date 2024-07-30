using BookMate.DataAccess.Data;
using BookMate.DataAccess.IRepository;
using BookMate.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using ServiceContracts;
using ServiceContracts.DTO;
using System.Security.Claims;

namespace book_mate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private IClubService _clubService;
        private IUnitOfWork _unitOfWork;
        private IPostService _postService;
        private ApplicationDbContext _db;
        private ICommentService _commentService;

        public CommentController(ApplicationDbContext db, IClubService clubService, IUnitOfWork unitOfWork, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager, IPostService postService, ICommentService commentService)
        {
            _userManager = userManager;
            _clubService = clubService;
            _unitOfWork = unitOfWork;
            _postService = postService;
            _db = db;
            _commentService = commentService;
        }


        [HttpGet("getComment/{commentId}")]
        public async Task<IActionResult> getComment([FromRoute] string commentId)
        {
            Comment comment =await _commentService.GetAsync(new Guid(commentId));

            return new JsonResult(new { status = 200, data = comment });
        }

        [HttpGet("getComments/{postId}")]
        public async Task<IActionResult> getPosts([FromRoute] string postId)
        {
            var comments = _commentService.GetAllAsync(new Guid(postId));
            return new JsonResult(new { status = 200, data = comments.Result });
        }

        [Authorize]
        [HttpPost("createComment")]
        public async Task<IActionResult> createComment([FromBody] CommentAddRequest request)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);

            Post post =await _postService.GetAsync(request.PostId);
            bool isMember = await _clubService.CheckIfMember(user.Id, post.ClubId.ToString());
            if (!isMember)
            {
                return new JsonResult(new { status = 400, message = "you are not member in this club" });
            }

            CommentAddRequest c = request;
            c.ApplicationUserId = user.Id;
            var comment = await _commentService.CreateAsync(c);

            return new JsonResult(new { status = 200, data = comment });
        }

        [Authorize]
        [HttpPost("updateComment/{id}")]
        public async Task<IActionResult> updateComment([FromRoute] string id,[FromBody] string request)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);

            var comment = await _commentService.UpdateAsync(new Guid(id), new CommentAddRequest { Content = request ,ApplicationUserId = user.Id});
            return new JsonResult(new { status = 200, data = comment });

        }

        [Authorize]
        [HttpDelete("deleteComment/{id}")]
        public async Task<IActionResult> deleteComment([FromRoute] string id)
        {
            var ok = await _commentService.DeleteAsync(new Guid(id));
            if (ok != null)
                return new JsonResult(new { status = 200, message = $"success deleted {ok}" });
            return new JsonResult(new { status = 400, message = "failed" });
        }

    }
}
