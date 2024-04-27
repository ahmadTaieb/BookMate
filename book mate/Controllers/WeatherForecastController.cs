using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;


namespace book_mate.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        public WeatherForecastController(ILogger<WeatherForecastController> logger,IConfiguration configuration,IUserService userService)
        {
            _logger = logger;
            _configuration = configuration;
            _userService = userService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }
        
        //[HttpPost("Register")]
        //public async Task<IActionResult> Register(RegisterDTO user)
        //{
        //    if (await _userService.RegisterA(user))
        //    {
        //        return Ok("Successfuly done");
        //    }
        //    return BadRequest("Something went worng");
        //}

        //[HttpPost("Login")]
        //public async Task<IActionResult> Login(LoginDTO user)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest();
        //    }
        //    if (await _userService.Login(user))
        //    {
        //        var tokenString = _userService.GenerateTokenString(user);
        //        return Ok(tokenString);
        //    }
        //    return BadRequest();
        //}
    }
}
