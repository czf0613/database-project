using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecondHand.model;
using SecondHand.Model;

namespace SecondHand.controller
{
    [ApiController]
    [Route("[controller]")]
    public class StatisticController : ControllerBase
    {
        private readonly Databases databases;

        public StatisticController(Databases databases)
        {
            this.databases = databases;
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> MyCommodities(string userName)
        {
            var student = await databases.Students.FirstAsync(s => s.UserName == userName);
            var body = new MyGoods
            {
                AllMyCommodities = student.AllMyCommodities,
                Sold = student.Sold,
                Bought = student.Bought
            };

            return Ok(body);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> ListStudents(int? limit = 100)
        {
            var body = await databases.Students.ToListAsync();
            return Ok(body.GetRange(0, Math.Min(body.Count, limit ?? 100)));
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> ListCommodities(int? limit = 100)
        {
            var body = await databases.Commodities.ToListAsync();
            return Ok(body.GetRange(0, Math.Min(body.Count, limit ?? 100)));
        }
        
        [HttpGet("[action]")]
        public async Task<ActionResult> ListSalesRecords(int? limit = 100)
        {
            var body = await databases.SalesRecords.ToListAsync();
            return Ok(body.GetRange(0, Math.Min(body.Count, limit ?? 100)));
        }
        
        [HttpGet("[action]")]
        public async Task<ActionResult> ListAdmins(int? limit = 100)
        {
            var body = await databases.Admins.ToListAsync();
            return Ok(body.GetRange(0, Math.Min(body.Count, limit ?? 100)));
        }
    }
}