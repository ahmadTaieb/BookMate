using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace book_mate.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class TestController : ControllerBase
    {       
        
            [HttpGet]
            public IActionResult GetData()
            {
                return Ok("Hello...");
            }
        
    }
}
