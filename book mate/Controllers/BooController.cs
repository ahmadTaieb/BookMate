using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
namespace book_mate.Controllers
{
    public class BooController : Controller
    {



        [Route("/a")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
