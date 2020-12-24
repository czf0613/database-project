using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecondHand.model;
using SecondHand.Model;
using SecondHand.Service;

namespace SecondHand.controller
{
    [ApiController]
    [Route("[controller]")]
    public class StatisticController : ControllerBase
    {
        private readonly Databases databases;
        private readonly ICommodityManager commodityManager;

        public StatisticController(Databases databases, ICommodityManager commodityManager)
        {
            this.databases = databases;
            this.commodityManager = commodityManager;
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

        [HttpDelete("[action]")]
        public async Task<ActionResult> DeleteStudent(int stuId)
        {
            var cntOfCommodities = (await databases.Commodities.CountAsync(c => c.Seller.Id == stuId)) == 0;
            var cntOfSalesRecord = (await databases.SalesRecords.CountAsync(s => s.Buyer.Id == stuId)) == 0;
            var ableToDelete = cntOfCommodities;
            ableToDelete = ableToDelete && cntOfSalesRecord;

            if (!ableToDelete)
                return BadRequest(
                    "Relation between students and commodities exists, you are not allowed to remove such a student or it will cause chaos.");

            var student = await databases.Students.FirstAsync(s => s.Id == stuId);
            databases.Students.Remove(student);
            await databases.SaveChangesAsync();
            return Ok(student);
        }

        [HttpDelete("[action]")]
        public async Task<ActionResult> DeleteAdmin(int teacherId)
        {
            var teacher = await databases.Admins.FirstAsync(a => a.Id == teacherId);
            databases.Admins.Remove(teacher);
            await databases.SaveChangesAsync();
            return Ok(teacher);
        }

        [HttpDelete("[action]")]
        public async Task<ActionResult> DeleteCommodity(int commodityId)
        {
            var tuple = await commodityManager.DeleteOneAsync(commodityId);
            return Ok(tuple.Item1);
        }

        [HttpDelete("[action]")]
        public async Task<ActionResult> DeleteSalesRecord(int commodityId)
        {
            var tuple = await commodityManager.DeleteOneAsync(commodityId);
            return Ok(tuple.Item2);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> ModifyStudent([FromBody] Student student)
        {
            var entry = databases.Students.Update(student);
            await databases.SaveChangesAsync();
            return Ok(entry.Entity);
        }
    }
}