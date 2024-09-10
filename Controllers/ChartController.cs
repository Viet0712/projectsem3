using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_sem3.InterFace;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly IChart _chartRepo;
        public ChartController(IChart chart)
        {
            _chartRepo = chart;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("ChartStorePerDay")]
        public async Task<ActionResult> ChartStorePerDay([FromQuery] ChartStorePerDayParams chart)
        {
            var result = await _chartRepo.ChartStorePerDay(chart.StoreId,chart.date);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        
        [Authorize(Roles = "SAdmin")]
        [HttpGet("ChartStorePerDay/SAdmin")]
        public async Task<ActionResult> ChartStorePerDaySAdmin([FromQuery] ChartStorePerDayParams chart)
        {
            var result = await _chartRepo.ChartStorePerDay(chart.StoreId, chart.date);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("CompareStorePerDay")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CompareStorePerDay([FromQuery] CompareStorePerDayParams chart)
        {
            var result = await _chartRepo.CompareStorePerDay(chart.StoreId, chart.date1 , chart.date2 , chart.BrandId);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("CompareStorePerDay/SAdmin")]
        [Authorize(Roles = "SAdmin")]
        public async Task<ActionResult> CompareStorePerDaySAdmin([FromQuery] CompareStorePerDayParams chart)
        {
            var result = await _chartRepo.CompareStorePerDay(chart.StoreId, chart.date1, chart.date2, chart.BrandId);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("CompareStorePerMonth")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CompareStorePerMonth([FromQuery] CompareStorePerDayParams chart)
        {
            var result = await _chartRepo.CompareStorePerMonth(chart.StoreId, chart.date1, chart.date2, chart.BrandId);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("CompareStorePerMonth/SAdmin")]
        [Authorize(Roles = "SAdmin")]
        public async Task<ActionResult> CompareStorePerMonthSAdmin([FromQuery] CompareStorePerDayParams chart)
        {
            var result = await _chartRepo.CompareStorePerMonth(chart.StoreId, chart.date1, chart.date2, chart.BrandId);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("CompareStoreBrandPerDay")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CompareStoreBrandPerDay([FromQuery] CompareStoreBrandPerDayParams chart)
        {
            var result = await _chartRepo.CompareStoreBrandPerDay(chart.StoreId, chart.date, chart.BrandId1, chart.BrandId2);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("CompareStoreBrandPerDay/SAdmin")]
        [Authorize(Roles = "SAdmin")]
        public async Task<ActionResult> CompareStoreBrandPerDaySAdmin([FromQuery] CompareStoreBrandPerDayParams chart)
        {
            var result = await _chartRepo.CompareStoreBrandPerDay(chart.StoreId, chart.date, chart.BrandId1, chart.BrandId2);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("CompareStoreBrandPerMonth")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> CompareStoreBrandPerMonth([FromQuery] CompareStoreBrandPerDayParams chart)
        {
            var result = await _chartRepo.CompareStoreBrandPerMonth(chart.StoreId, chart.date, chart.BrandId1, chart.BrandId2);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("CompareStoreBrandPerMonth/SAdmin")]
        [Authorize(Roles = "SAdmin")]
        public async Task<ActionResult> CompareStoreBrandPerMonthSAdmin([FromQuery] CompareStoreBrandPerDayParams chart)
        {
            var result = await _chartRepo.CompareStoreBrandPerMonth(chart.StoreId, chart.date, chart.BrandId1, chart.BrandId2);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("CompareStore_StorePerDaySAdmin/SAdmin")]
        [Authorize(Roles = "SAdmin")]
        public async Task<ActionResult> CompareStore_StorePerDaySAdmin([FromQuery] CompareStore_StorePerDayParams chart)
        {
            var result = await _chartRepo.CompareStore_StorePerDay(chart.date, chart.Store1, chart.Store2);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpGet("CompareStore_StorePerMonthSAdmin/SAdmin")]
        [Authorize(Roles = "SAdmin")]
        public async Task<ActionResult> CompareStore_StorePerMonthSAdmin([FromQuery] CompareStore_StorePerDayParams chart)
        {
            var result = await _chartRepo.CompareStore_StorePerMonth(chart.date, chart.Store1, chart.Store2);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }



        public class ChartStorePerDayParams
        {
           
            public int? StoreId { get; set; }
            public DateTime date { get; set; }
        }
        public class CompareStorePerDayParams
        {

            public int? StoreId { get; set; }
            public DateTime date1 { get; set; }

            public DateTime date2 { get; set; }

            public int? BrandId { get; set; }
        }
        public class CompareStoreBrandPerDayParams
        {

            public int? StoreId { get; set; }
         

            public DateTime date { get; set; }

            public int BrandId1 { get; set; }

            public int BrandId2 { get; set; }
        }

        public class CompareStore_StorePerDayParams
        {

            public DateTime date { get; set; }

            public int Store1 { get; set; }

            public int Store2 { get; set; }
        }
    }
   


}
