using Microsoft.AspNetCore.Mvc;

namespace SecondHand.controller
{
    [ApiController]
    [Route("api")]
    public class LoginController : ControllerBase
    {
        [HttpGet("[action]")]
        public IActionResult Hello(int Id)
        {
            return Ok($"Hello, {Id}");
        }
    }
}
