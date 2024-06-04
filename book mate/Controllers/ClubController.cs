using BookMate.DataAccess.Data;
using BookMate.DataAccess.IRepository;
using BookMate.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using ServiceContracts.DTO;
using System.Security.Claims;

namespace book_mate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class ClubController : ControllerBase
    {
        private Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private IClubService _clubService;
        private IUnitOfWork _unitOfWork;
        private ApplicationDbContext _db;

        public ClubController(ApplicationDbContext db,IClubService clubService, IUnitOfWork unitOfWork,Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager) 
        {
            _userManager = userManager;
            _clubService =  clubService;
            _unitOfWork = unitOfWork;
            _db = db;

        }


        [Authorize]
        [HttpPost("CreateClub")]
        public async Task<IActionResult> createClub([FromBody] ClubAddRequest club)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email); 
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);
            var adminId = user.Id;
            
            return new JsonResult(_clubService.AddClubAsync(adminId, club).Result);
            
        }


        [Authorize]
        [HttpGet("AdminClubs")]
        public async Task<IActionResult> getAdminClubs()
        {

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);
            var clubs = _clubService.GetAdminClubsAsync(user.Id.ToString()).Result;

            return new JsonResult( clubs);
        }


        [Authorize]
        [HttpPost("UpdateClub/{clubId}")]
        public async Task<IActionResult> UpdateClub([FromQuery]string clubId,[FromBody] ClubAddRequest club)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email); 
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);
            var adminId = user.Id;
            var c =await  _clubService.GetClub(clubId);
            if (c.ApplicationUserId  != adminId)
            {
                return new JsonResult("you are not admin in this club");
            }
            var UpdatedClub = _clubService.UpdateAsync(adminId, club).Result;

            return new JsonResult(UpdatedClub);
        }


        [Authorize]
        [HttpPost("AddMember")]
        public async Task<IActionResult> AddMember([FromBody]string clubId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);

            
            await _clubService.AddMember( user.Id,new Guid(clubId));

            return new JsonResult(new { status = 200, message = "member added successfully" });
        }
        

        [HttpGet("getMembers/{id}")]
        public async Task<IActionResult> getClub(string id)
        {

            var members = _clubService.GetMembers(id).Result;
            List<ApplicationUserResponse> response = new List<ApplicationUserResponse>();
            foreach (var u in members)
            {
                response.Add(new ApplicationUserResponse { Id = u.Id, Name = u.Name });
            }

            return new JsonResult(new { status = 200, message = "success", data = response });

        }


        [Authorize]
        [HttpGet("GetClubsMember")]
        public async Task<IActionResult> GetClubsMember()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);

            var clubs = _clubService.GetClubsMember(user.Id).Result;

            return new JsonResult(clubs);
        }


        [Authorize]
        [HttpGet("deleteClub/{id}")]
        public async Task<IActionResult> DeleteClub(string id) 
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);

            bool ok = await _clubService.DeleteAsync(id, user.Id);

            if (ok)
            {
                return new JsonResult(new { status = 200, message = "success" });
            }
            return new JsonResult(new {status = 400 ,message = "failed"});
        }


        [HttpGet("getClub/{id}")]
        public async Task<IActionResult> GetClub(string id)
        {
            return new JsonResult(new { status = 200, message = "success", data = _clubService.GetClub(id) });
        }

        [HttpPost("searchClub")]
        public async Task<IActionResult> SearchClubByName([FromBody] string search)
        {
            var AllClubs = _clubService.GetAllClubsAsync();
            var clubs = AllClubs.Result.Where(o => o.Name.ToLower().Contains(search.Trim().ToLower()));

            return new JsonResult(new { status = 200, message = "success", data = clubs});

        }

    }
}
