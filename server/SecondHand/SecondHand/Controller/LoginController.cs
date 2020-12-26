using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecondHand.model;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using SecondHand.Service;

namespace SecondHand.controller
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly Databases databases;
        private readonly ICredentialManager credentialManager;

        public LoginController(Databases databases, ICredentialManager credentialManager)
        {
            this.databases = databases;
            this.credentialManager = credentialManager;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SignUp(string userName, string password,
            [FromBody] Student student)
        {
            var countUserName = databases.Users.Where(s => s.UserName == userName).CountAsync();
            var countPhone = databases.Users.Where(s => s.Phone == student.Phone).CountAsync();
            student.UserName = userName;
            student.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(password);
            if (await countUserName != 0)
                return BadRequest("User Name already exist");

            if (await countPhone != 0)
                return BadRequest("Phone Number already exist");

            var addStudent = await databases.Students.AddAsync(student);
            await databases.SaveChangesAsync();

            return Ok(addStudent.Entity);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AdminSignUp(string userName, string password, [FromBody] Admin admin)
        {
            var countUserName = databases.Users.Where(a => a.UserName == userName).CountAsync();
            var countPhone = databases.Users.Where(s => s.Phone == admin.Phone).CountAsync();
            admin.UserName = userName;
            admin.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(password);

            if (await countUserName != 0)
                return BadRequest("User Name already exist");

            if (await countPhone != 0)
                return BadRequest("Phone Number already exist");

            var addAdmin = await databases.Admins.AddAsync(admin);
            await databases.SaveChangesAsync();

            return Ok(addAdmin.Entity);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(string userName, string password, int role = 0)
        {
            var roleType = (Role) role;
            var result = await credentialManager.CreateLoginRecordAsync(userName, password, roleType);
            if (result.Item1 == LoginResult.BADCRIDENTIAL)
                return Unauthorized("Wrong user name or password!");
            if (result.Item1 == LoginResult.TOO_MUCH)
                return BadRequest("You have too many devices logged in.");
            return Ok(result.Item2);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> LogOut(string token)
        {
            await credentialManager.LogOutAsync(token);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> LogOutAnyway(string userName)
        {
            await credentialManager.LogOutAnywayAsync(userName);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Self([FromBody] Student student)
        {
            var old = await databases.Students.Where(s => s.UserName == student.UserName).FirstAsync();

            old.Name = student.Name;
            old.Gender = student.Gender;
            old.IconURL = student.IconURL;
            old.SerialNumber = student.SerialNumber;
            old.Profile = student.Profile;
            old.Phone = student.Phone;
            old.Email = student.Email;
            old.Birthday = student.Birthday;
            old.Major = student.Major;
            old.College = student.College;
            old.Dormitory = student.Dormitory;

            await databases.SaveChangesAsync();
            return Ok("Modify Self Information Success");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AdminSelf([FromBody] Admin admin)
        {
            var old = await databases.Admins.Where(a => a.UserName == admin.UserName).FirstAsync();

            old.Name = admin.Name;
            old.Gender = admin.Gender;
            old.IconURL = admin.IconURL;
            old.SerialNumber = admin.SerialNumber;
            old.Profile = admin.Profile;
            old.Phone = admin.Phone;
            old.Email = admin.Email;
            old.Birthday = admin.Birthday;
            old.Department = admin.Department;
            old.Level = admin.Level;

            await databases.SaveChangesAsync();
            return Ok("Modify Self Information Success");
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ChangePassword(string userName, string oldPassword, string newPassword)
        {
            var personCnt = await databases.Users.Where(u => u.UserName == userName).CountAsync();
            if (personCnt == 0)
                return BadRequest("No such person!");

            var person = await databases.Users.FirstAsync(u => u.UserName == userName);
            if (BCrypt.Net.BCrypt.EnhancedVerify(oldPassword, person.Password))
            {
                person.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(newPassword);
                await databases.SaveChangesAsync();
                return Ok("Change Password Success");
            }

            return BadRequest("Bad Credential");
        }

        [Route("/error")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public string Error()
        {
            return "Something Wrong Happened";
        }
    }
}