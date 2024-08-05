using book_mate.Hubs;
using BookMate.DataAccess.Data;
using BookMate.DataAccess.IRepository;
using BookMate.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using ServiceContracts;
using Services;
using System.Security.Claims;

namespace book_mate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> _userManager;
        private IClubService _clubService;
        private IUnitOfWork _unitOfWork;
        private IPostService _postService;
        private IReactService _reactService;
        private IReportService _reportService;
        private INotificationService _notificationService;
        private ApplicationDbContext _db;
        private readonly IHubContext<NotificationHub> _hubContext;


        public ReportController( IHubContext<NotificationHub> hubContext,ApplicationDbContext db, IClubService clubService, IUnitOfWork unitOfWork, Microsoft.AspNetCore.Identity.UserManager<ApplicationUser> userManager, IPostService postService , IReportService reportService, INotificationService notificationService)
        {
            _userManager = userManager;
            _clubService = clubService;
            _unitOfWork = unitOfWork;
            _postService = postService;
            _reportService = reportService;
            _db = db;
            _notificationService = notificationService;
            _hubContext = hubContext;
        }

        [Authorize]
        [HttpGet("report/{postId}")]
        public async Task<IActionResult> report([FromRoute] Guid postId)
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            ApplicationUser user = await _userManager.FindByEmailAsync(userEmail);

            Report report =await _reportService.CreateAsync(new Report
            {
                ApplicationUserId = user.Id,
                PostId = postId,
            });
            await _notificationService.CreateNotificationForReportAsync(report);
            await _hubContext.Clients.All.SendAsync("ReceiveNotification", report.Id, "new report created");

            return new JsonResult (new {status = 200 , data = report});

        }
        [HttpGet("getReport/{id}")]
        public async Task<IActionResult> report(string id)
        {
            Report report =await _reportService.GetAsync(new Guid(id));
            return new JsonResult(new { status = 200 , data = report });

        }

        [HttpDelete("deleteReport/{id}")]
        public async Task<IActionResult> deleteReport([FromRoute] Guid id)
        {
            
            Report report =await _reportService.DeleteAsync(id);

            return new JsonResult (new {status = 200 , data = report  });

        }
        [HttpGet("getAll")]
        public async Task<IActionResult> getAll()
        {
            List<Report> reports = await _reportService.GetAllAsync();
            return new JsonResult (new { status = 200, data = reports });
        }


    }
}
