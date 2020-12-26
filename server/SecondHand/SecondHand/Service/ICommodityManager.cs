using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SecondHand.model;

namespace SecondHand.Service
{
    public interface ICommodityManager
    {
        Task<ValueTuple<Commodity, SalesRecord?>> DeleteOneAsync(int commodityId);
    }

    public class CommodityManager : ICommodityManager
    {
        private readonly Databases databases;

        public CommodityManager(Databases databases)
        {
            this.databases = databases;
        }

        public async Task<(Commodity, SalesRecord?)> DeleteOneAsync(int commodityId)
        {
            var commodity = await databases.Commodities.FirstAsync(c => c.Id == commodityId);
            databases.Commodities.Remove(commodity);
            await databases.SaveChangesAsync();
            return (commodity, commodity.SalesRecord);
        }
    }
}