using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Project_sem3.InterFace;
using Project_sem3.Models;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShippingController : ControllerBase
    {
        private readonly IShipping _ishipping;
        public ShippingController(IShipping shipping)
        {
            _ishipping=shipping;
        }
        [HttpGet]
        [Authorize(Roles = "SAdmin,Admin")]
        public async Task<ActionResult> GetAll()
        {
            var result = await _ishipping.GetAllShipping();
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpGet("SearchByName/{name}")]
        [Authorize(Roles = "SAdmin,Admin")]
        public async Task<ActionResult> SearchByName(string name)
        {
            var result = await _ishipping.SearchByName(name);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpGet("ChangeStatus/{id}")]
        [Authorize(Roles = "SAdmin,Admin")]
        public async Task<ActionResult> ChangeStatus(int id)
        {
            var result = await _ishipping.ChangeStatus(id);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpPost]
        [Authorize(Roles = "SAdmin,Admin")]
        public async Task<ActionResult> CreateShipping(Shipping shipping)
        {
            var result = await _ishipping.CreateShipping(shipping);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpPut]
        [Authorize(Roles = "SAdmin,Admin")]
        public async Task<ActionResult> UpdateShipping(Shipping shipping)
        {
            var result = await _ishipping.UpdateShipping(shipping);
            if (result.Status == 200)
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
