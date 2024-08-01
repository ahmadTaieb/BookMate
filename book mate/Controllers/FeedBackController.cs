using BookMate.DataAccess.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;

namespace book_mate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedBackController : ControllerBase
    {

        private readonly IFeedBackService _feedBackService;
        private readonly ApplicationDbContext _applicationDbContext;

        public FeedBackController(ApplicationDbContext applicationDbContext, IFeedBackService feedBackService)
        {
            _feedBackService = feedBackService;
            _applicationDbContext = applicationDbContext;
        }



        [HttpGet]
        [Route("CategoriesPercent")]
        public async Task<IActionResult>CategoriesPercent ()
        {


            List<CategoryPercentResponse?>? responses = await _feedBackService.CategoriesPercent();
            return Ok(responses);



        }


    }
}
