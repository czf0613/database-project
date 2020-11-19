using Microsoft.AspNetCore.Mvc;
using SecondHand.model;

namespace SecondHand.controller
{
    [ApiController]
    [Route("commodity")]
    public class CommodityController: ControllerBase
    {
        private readonly Databases databases;

        public CommodityController(Databases databases)
        {
            this.databases = databases;
        }
    }
}