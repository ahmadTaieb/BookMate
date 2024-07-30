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
        private IReactService _reactService;
        private ApplicationDbContext _db;

        public PostController(ApplicationDbContext db, IClubService clubService, IUnitOfWork unitOfWork, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager, IPostService postService,IReactService reactService)
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
            Post post =await _postService.GetAsync(new Guid(id));

            int[] arr = await _reactService.GetCountAsync(new Guid(id));
            
            int total = arr[0] + arr[1] + arr[2] + arr[3];
            PostResponse response = new PostResponse
            {
                Id = post.Id.ToString(),
                Content = post.Content,
                ImageUrl = post.ImageUrl,
                ApplicationUserId = post.ApplicationUserId,
                ClubId = post.ClubId,
                TotalReacts = total,
                Like = arr[0],
                Love = arr[1],
                Laugh = arr[2],
                Sad = arr[3]
            };

            return new JsonResult(new { status = 200,data = response  });
        }

        [HttpGet("getPosts/{clubId}")]
        public async Task<IActionResult> getPosts([FromRoute] string clubId)
        {
            List<Post> posts =await _postService.GetAllAsync(new Guid(clubId));
            List<PostResponse> responseList = new List<PostResponse>();

            foreach(Post post in posts)
            {
                int[] arr = await _reactService.GetCountAsync(post.Id);

                int total = arr[0] + arr[1] + arr[2] + arr[3];
                PostResponse response = new PostResponse
                {
                    Id = post.Id.ToString(),
                    Content = post.Content,
                    ImageUrl = post.ImageUrl,
                    ApplicationUserId = post.ApplicationUserId,
                    ClubId = post.ClubId,
                    TotalReacts = total,
                    Like = arr[0],
                    Love = arr[1],
                    Laugh = arr[2],
                    Sad = arr[3]
                };
                responseList.Add(response);

            }


            return new JsonResult(new { status = 200, data = responseList });
        }
        [Authorize]
        [HttpPost("createPost")]
        public async Task<IActionResult> createPost([FromBody] PostAddRequest request)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);

            bool isMember =await _clubService.CheckIfMember(user.Id,request.ClubId.ToString());
            if (!isMember) 
            {
                return new JsonResult(new { status = 400, message = "you are not member in this club" });
            }

            PostAddRequest p = request;
            p.ApplicationUserId = user.Id;

            var post = await _postService.CreateAsync(p);
            return new JsonResult(new { status = 200, data = post });
        }

        [Authorize]
        [HttpPost("updatePost/{id}")]
        public async Task<IActionResult> updatePost([FromRoute] string id,[FromBody]PostAddRequest request)
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
