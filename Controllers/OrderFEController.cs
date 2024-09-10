using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_sem3.InterFace;
using Project_sem3.Models;
using System.Collections.Generic;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderFEController : ControllerBase
    {
        private readonly IOrderFE repo;
        public OrderFEController(IOrderFE repo)
        {
            this.repo = repo;
        }

        [HttpPost("Create/{email}")]
        [Authorize(Roles = "customer")]
        public async Task<ActionResult> Create(string email, PaymentRequest a)
        {
            var rs = await repo.Create(email, a);
            if (rs.Status == 200)
            {
                return Ok(new CustomResult { Status =200, Message = "Payment Success", data = a });
            }
            else if (rs.Status == 201)
            {
                return Ok(new CustomResult { Status = 201, Message = "USER NOT FOUND", data = a });
            }
            else if (rs.Status == 401)
            {
                return Ok(new CustomResult { Status = 401, Message = "STOCK OUT", data = a });
            }

            else if (rs.Status == 400)
            {
                return Ok(new CustomResult { Status = 400, Message = "OUT OF STOCK", data = a });
            }
         
            else
            {
                return BadRequest(rs);
                //return Ok(new CustomResult { Status = 404, Message = "Payment Found", data = null });
            }



        }


        [HttpPost("Get/{email}")]
        [Authorize(Roles = "customer")]
        public async Task<ActionResult> Create(string email)
        {
            var list = await repo.Get(email);
            if (list == null)
            {
                return Ok(new CustomResult { Status = 501, Message = "Get data fail", data = null });
            }
            if (list.Count() == 0)
            {
                return Ok(new CustomResult { Status = 201, Message = "Data is empty", data = null });
            }
            return Ok(new CustomResult { Status = 200, Message = "Get data success", data = list });

        }


        [HttpGet("GetOrderDetail/{id}")]
         [Authorize(Roles = "customer")]
        public async Task<ActionResult> GetOrderDetail(string id)
        {
            var cart = await repo.GetOrderDetails(id);
            if (cart == null)
            {
                return Ok(new CustomResult { Status = 201, Message = "Get data fails", data = null });
            }
            return Ok(new CustomResult { Status = 200, Message = "Get data success", data = cart });
        }



        [HttpGet("UpdateStatusRate/{id}")]
        [Authorize(Roles = "customer")]
        public async Task<ActionResult> UpdateStatusRate(string id)
        {
            var rs = await repo.UpdateStatusOrder(id);
            if (rs == null)
            {
                return Ok(new CustomResult { Status = 201, Message = "update status rating fails", data = null });
            }
            return Ok(new CustomResult { Status = 200, Message = "update status rating success", data = rs });
        }
    }
}
