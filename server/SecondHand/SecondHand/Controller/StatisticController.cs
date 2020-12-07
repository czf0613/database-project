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
    }
}