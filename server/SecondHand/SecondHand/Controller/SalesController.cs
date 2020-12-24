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
    public class SalesController : ControllerBase
    {
        private readonly Databases databases;

        public SalesController(Databases databases)
        {
            this.databases = databases;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Buy(string userName, int commodityId, [FromBody] AddressDetail addressDetail)
        {
            try
            {
                var student = databases.Students.FirstAsync(s => s.UserName == userName);
                var commodity = await databases.Commodities.FirstAsync(c => c.Id == commodityId);
                var record = new SalesRecord
                {
                    Auction = commodity.Price,
                    Buyer = await student,
                    Commodity = commodity,
                    CommodityId = commodity.Id,
                    DeliveryAddress = addressDetail,
                    Seller = commodity.Seller
                };
                commodity.Sold = true;
                record = (await databases.SalesRecords.AddAsync(record)).Entity;
                commodity.SalesRecord = record;
                await databases.SaveChangesAsync();

                return Ok(record);
            }
            catch (DbUpdateConcurrencyException dbUpdateConcurrencyException)
            {
                //处理并发购买问题
                Console.WriteLine(dbUpdateConcurrencyException.Message);
                return BadRequest("This commodity is bought by others.");
            }
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Confirm(int salesRecord)
        {
            var record = await databases.SalesRecords.FirstAsync(s => s.Id == salesRecord);
            record.Check = true;
            await databases.SaveChangesAsync();
            return Ok(record);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Comment(int salesRecord, string comment)
        {
            var record = await databases.SalesRecords.FirstAsync(s => s.Id == salesRecord);
            if (!record.Check)
                return BadRequest("You can not comment before confirming receipt");
            record.Comment = comment;
            await databases.SaveChangesAsync();
            return Ok(record);
        }
    }
}