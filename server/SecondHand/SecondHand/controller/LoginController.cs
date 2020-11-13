using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecondHand.model;

namespace SecondHand.controller
{
    [ApiController]
    [Route("api")]
    public class LoginController : ControllerBase
    {
        private readonly Databases _databases;

        public LoginController(Databases databases)
        {
            _databases = databases;
        }

        [HttpGet("[action]")]
        public IActionResult Students()
        {
            return Ok(_databases.Students.ToListAsync());
        }

        [HttpGet("[action]")]
        public IActionResult Hello(int Id)
        {
            return Ok($"Hello, {Id}");
        }
    }
}
