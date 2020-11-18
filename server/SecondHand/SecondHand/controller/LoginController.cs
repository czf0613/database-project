using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecondHand.model;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SecondHand.controller
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly Databases databases;

        public LoginController(Databases databases)
        {
            this.databases = databases;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> SignUp(string userName, string password, [FromBody] Student student)
        {
            var countUserName = databases.Users.Where(s => s.UserName == userName).CountAsync();
            var countPhone = databases.Users.Where(s => s.Phone == student.Phone).CountAsync();
            student.UserName = userName;
            student.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(password);
            if (await countUserName != 0)
                return BadRequest("User Name already exist");

            if (await countPhone != 0)
                return BadRequest("Phone Number already exist");

            try
            {
                var addStudent = await databases.Students.AddAsync(student);
                await databases.SaveChangesAsync();

                return Ok(addStudent.Entity);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("Concurrent Modification occurs");
            }
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

            try
            {
                var addAdmin = await databases.Admins.AddAsync(admin);
                await databases.SaveChangesAsync();

                return Ok(addAdmin.Entity);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("Concurrent Modification occurs");
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(string userName, string password)
        {
            var personCnt = await databases.Students.Where(s => s.UserName == userName).CountAsync();
            if (personCnt == 0)
                return BadRequest("Login Failed");

            var person = await databases.Students.Where(s => s.UserName == userName).FirstAsync();
            if (BCrypt.Net.BCrypt.EnhancedVerify(password, person.Password))
            {
                return Ok(person);
            }
            else
            {
                return BadRequest("Login Failed");
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AdminLogin(string userName, string password)
        {
            var personCnt = await databases.Admins.Where(a => a.UserName == userName).CountAsync();
            if (personCnt == 0)
                return BadRequest("Login Failed");

            var person = await databases.Admins.Where(a => a.UserName == userName).FirstAsync();
            if (BCrypt.Net.BCrypt.EnhancedVerify(password, person.Password))
            {
                return Ok(person);
            }
            else
            {
                return BadRequest("Login Failed");
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Self([FromBody] Student student)
        {
            try
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
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("Fail to modify self-information");
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AdminSelf([FromBody] Admin admin)
        {
            try
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
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return BadRequest("Fail to modify self-information");
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> ChangePassword(string userName, string oldPassword, string newPassword)
        {
            var personCnt = await databases.Users.Where(u => u.UserName == userName).CountAsync();
            if (personCnt == 0)
                return BadRequest("Change Password Failed");

            var person = databases.Users.Where(u => u.UserName == userName).First();
            if (BCrypt.Net.BCrypt.EnhancedVerify(oldPassword, person.Password))
            {
                person.Password = BCrypt.Net.BCrypt.EnhancedHashPassword(newPassword);
                await databases.SaveChangesAsync();
                return Ok("Change Password Success");
            }
            else
            {
                return BadRequest("Change Password Failed");
            }
        }

        [Route("/error")]
        public IActionResult Error()
        {
            return BadRequest("Something Wrong Happened");
        }
    }
}
