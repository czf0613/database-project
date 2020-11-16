using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecondHand.model;
using SecondHand.model.json;
using System.Threading.Tasks;

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

        [HttpPost("[action]")]
        public async Task<IActionResult> SignUp(string userName, string password, [FromBody]StudentInfo studentInfo)
        {
            var student = new Student
            {
                UserName = userName,
                Password = BCrypt.Net.BCrypt.EnhancedHashPassword(password),
                Gender = studentInfo.Gender,
                Name = studentInfo.Name,
                Profile = studentInfo.Profile,
                Phone = studentInfo.Phone,
                Email = studentInfo.Email,
                Birthday = studentInfo.Birthday,
                SerialNumber = studentInfo.SerialNumber,
                College = studentInfo.College,
                Major = studentInfo.Major,
                Dormitory = studentInfo.Dormitory
            };
            return Ok((await _databases.Students.AddAsync(student)).Entity);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Hello()
        {
            return Ok(await _databases.Students.ToListAsync());
        }
    }
}
