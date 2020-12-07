using Microsoft.AspNetCore.Mvc;
using SecondHand.model;

namespace SecondHand.controller
{
    [ApiController]
    [Route("sales")]
    public class SalesController: ControllerBase
    {
        private readonly Databases databases;

        public SalesController(Databases databases)
        {
            this.databases = databases;
        }
    }
}