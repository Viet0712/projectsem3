using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_sem3.InterFace;
using Project_sem3.Repositories;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrder _orderRepo;
        public OrderController(IOrder IOrder)
        {
            _orderRepo = IOrder;
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("GetByStore")]
        public async Task<ActionResult> GetByStore([FromForm] OrderForm o)
        {
            var result = await _orderRepo.GetByStore(o.StoreID,o.Month);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("ChangeStatus/{id}")]
        public async Task<ActionResult> ChangeStatus(string id)
        {
            var result = await _orderRepo.ChangeStatus(id);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetOrderDetail/{id}")]
        public async Task<ActionResult> GetOrderDetail(string id)
        {
            var result = await _orderRepo.OrderDetail(id);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        public class OrderForm
        {
            public int StoreID { get; set; }

            public DateTime Month { get; set; }
        }
    }
}
