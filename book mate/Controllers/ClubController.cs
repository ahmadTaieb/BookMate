using BookMate.DataAccess.IRepository;
using BookMate.Entities;
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
    //[Authorize]
    public class ClubController : ControllerBase
    {
        private Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private IClubService _clubService;
        private IUnitOfWork _unitOfWork;

        public ClubController(IClubService clubService, IUnitOfWork unitOfWork,Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager) 
        {
            _userManager = userManager;
            _clubService =  clubService;
            _unitOfWork = unitOfWork;

        }

        [HttpGet("getClub/{id}")]
        public async Task<IActionResult> getClub(string id)
        {
            var club = await _clubService.GetClub(id);
            return new JsonResult(club);

        }

        [HttpPost("CreateClub")]
        public async Task<IActionResult> createClub([FromQuery] ClubAddRequest club)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email); 
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);
            var adminId = user.Id;
            
            return new JsonResult(_clubService.AddClubAsync(adminId, club));
        }


        [HttpGet("AdminClubs")]
        public async Task<IActionResult> getAdminClubs()
        {

            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);
            var clubs =await _clubService.GetAdminClubsAsync(user.Id.ToString());

            return new JsonResult(clubs);
        }

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

        [HttpPost("AddMember")]
        public async Task<IActionResult> AddMember([FromQuery]string clubId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);

            await _clubService.AddMember( user.Id,new Guid(clubId));

            return new JsonResult(new { status = 200, message = "member added successfully" });
        }

        [HttpPost("GetMembers")]
        public async Task<IActionResult> GetMembers([FromQuery]string clubId)
        {
            return new JsonResult(_clubService.GetMembers(new Guid(clubId)));
        }
    }
}
