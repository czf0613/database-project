using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SecondHand.model;

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

        [HttpPost("[action]")]
        public async Task<ActionResult> Update([FromBody] Commodity commodity)
        {
            databases.Commodities.Update(commodity);
            await databases.SaveChangesAsync();
            return Ok(commodity);
            throw new NotImplementedException();
        }

        [HttpDelete("[action]")]
        public async Task<ActionResult> Delete(int commodityId)
        {
            var commodity = await databases.Commodities.FirstAsync(c => c.Id == commodityId);
            if (commodity.Sold)
                return BadRequest("Item has been sold!");

            databases.Commodities.Remove(commodity);
            await databases.SaveChangesAsync();
            return Ok(commodity);
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
    }
}