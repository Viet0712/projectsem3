using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Project_sem3.Repositories;
using System;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SeedDataController : ControllerBase
    {

        private  IServiceProvider _serviceProvider;
        public SeedDataController(IServiceProvider IServiceProvider)
        {
            _serviceProvider = IServiceProvider;
        }
        [HttpGet("{year}/{month}/{day}/{recodrs}")]
        public async Task<ActionResult> Seed(int year , int month , int day, int recodrs)
        {
            var result = await SeedData.SeedDataFuction(_serviceProvider,year , month , day,recodrs);
            if (result == "Success")
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
