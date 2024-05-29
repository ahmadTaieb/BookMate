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
        public async Task<IActionResult> createClub([FromQuery] ClubAddRequest club)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email); 
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);
            var adminId = user.Id;
            //Club c = new Club() {
            //    Name = club.Name 
            //    ,ApplicationUserId = adminId
            //};
            ////c.ApplicationUsersMember.Add(new ApplicationUserClub { Club = c,ApplicationUserId=adminId});
            //_db.Clubs.Add(c);
            //_db.SaveChanges();
            
            //AddMember(c.Id.ToString(), user.Id);
            return new JsonResult(_clubService.AddClubAsync(adminId, club).Result);
            //return new JsonResult(c);
        }

        [Authorize]
        [HttpGet("AdminClubs")]
        public async Task<IActionResult> getAdminClubs()
        {

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);
            var clubs =await _clubService.GetAdminClubsAsync(user.Id.ToString());

            return new JsonResult(clubs);
        }
        [Authorize]
        [HttpPost("UpdateClub")]
        public async Task<IActionResult> UpdateClub([FromQuery]string clubId,[FromQuery] ClubAddRequest club)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email); 
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);
            var adminId = user.Id;
            var c =await  _clubService.GetClub(clubId);
            if (c.ApplicationUserId  != adminId)
            {
                return new JsonResult("you are not admin in this club");
            }

            return new JsonResult(await _clubService.UpdateAsync(clubId, club));
        }
        [Authorize]
        [HttpPost("AddMember")]
        public async Task<IActionResult> AddMember([FromQuery]string clubId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);

            
            await _clubService.AddMember( user.Id,new Guid(clubId));

            return new JsonResult(new { status = 200, message = "member added successfully" });
        }
        //public async Task<ApplicationUserClub> AddMember([FromQuery] string clubId,string userId)
        //{
            
            
        //    return await _clubService.AddMember(userId, new Guid(clubId));
           
        //}

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

    }
}
