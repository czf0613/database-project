using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecondHand.model;
using SecondHand.Model;

namespace SecondHand.controller
{
    [ApiController]
    [Route("[controller]")]
    public class CommodityController : ControllerBase
    {
        private readonly Databases databases;

        public CommodityController(Databases databases)
        {
            this.databases = databases;
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> New(string userName, [FromBody] Commodity commodity)
        {
            var student = databases.Students.FirstAsync(u => u.UserName == userName);
            commodity.Seller = await student;
            var entity = await databases.Commodities.AddAsync(commodity);
            await databases.SaveChangesAsync();

            return Ok(entity.Entity);
        }

        [HttpGet("[action]")]
        public async Task<ActionResult> Search(string query)
        {
            var keyWords = query.Split(" ");
            var body = new HashSet<Commodity>();

            foreach (var key in keyWords)
            {
                (await databases.Commodities.Where(c =>
                        (EF.Functions.Like(c.Title, $"%{key}%") || EF.Functions.Like(c.Description, $"%{key}%")) &&
                        c.Sold == false)
                    .ToListAsync()).ForEach(item => body.Add(item));
            }

            return Ok(body);
        }

        [HttpPost("[action]")]
        public async Task<ActionResult> Buy(string userName, int commodityId, [FromBody] AddressDetail addressDetail)
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
    }
}