using BookMate.DataAccess.Data;
using BookMate.DataAccess.IRepository;
using BookMate.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;

namespace book_mate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostWebController : ControllerBase
    {

        private Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private IClubService _clubService;
        private IUnitOfWork _unitOfWork;
        private IPostService _postService;
        private IReactService _reactService;
        private ApplicationDbContext _db;

        public PostWebController(ApplicationDbContext db, IClubService clubService, IUnitOfWork unitOfWork, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager, IPostService postService, IReactService reactService)
        {
            _userManager = userManager;
            _clubService = clubService;
            _unitOfWork = unitOfWork;
            _postService = postService;
            _reactService = reactService;
            _db = db;
        }


        [HttpGet("getPost/{id}")]
        public async Task<IActionResult> getPost([FromRoute] string id)
        {
            Post post = await _postService.GetAsync(new Guid(id));

            ApplicationUser user = await _userManager.FindByIdAsync(post.ApplicationUserId);

            int[] arr = await _reactService.GetCountAsync(new Guid(id));

            int total = arr[0] + arr[1] + arr[2] + arr[3];
            PostResponse response = new PostResponse
            {
                Id = post.Id.ToString(),
                Content = post.Content,
                ImageUrl = post.ImageUrl,
                ApplicationUserId = post.ApplicationUserId,
                ClubId = post.ClubId,
                ApplicationUserName = user.Name,
                ApplicationUserImageUrl = user.ImageUrl,
                TotalReacts = total,
                Like = arr[0],
                Love = arr[1],
                Laugh = arr[2],
                Sad = arr[3]
            };

            return new JsonResult(new { status = 200, data = response });
        }

        [HttpDelete("deletePost/{id}")]
        public async Task<IActionResult> deletePost([FromRoute] string id)
        {
            var ok = await _postService.DeleteAsync(new Guid(id));
            if (ok != null)
                return new JsonResult(new { status = 200, message = $"success deleted {ok}" });
            return new JsonResult(new { status = 400, message = "failed" });
        
        }

    }
}
