using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_sem3.InterFace;
using Project_sem3.Models;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly IDiscount _discountrepo;
        public DiscountController(IDiscount discount)
        {
            _discountrepo = discount;
        }
        [HttpGet]
        [Authorize(Roles = "SAdmin,Admin")]
        public async Task<ActionResult> GetAllTrue()
        {
            var result = await _discountrepo.GetAllTrue();
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpGet("SAdmin")]
        [Authorize(Roles = "SAdmin")]
        public async Task<ActionResult> GetAll()
        {
            var result = await _discountrepo.GetAll();
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
        public async Task<ActionResult> Create([FromForm] Discount d)
        {
            var result = await _discountrepo.Create(d);
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
        public async Task<ActionResult> Update([FromForm] Discount d)
        {
            var result = await _discountrepo.Update(d);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpGet("GetById/{id}")]
        [Authorize(Roles = "SAdmin,Admin")]
        public async Task<ActionResult> GetById(int id)
        {
            var result = await _discountrepo.GetById(id);
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
            var result = await _discountrepo.ChangeStatus(id);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpPost("Search")]
        [Authorize(Roles = "SAdmin,Admin")]
        public async Task<ActionResult> Search([FromForm] SearchRequest s)
        {
            var result = await _discountrepo.Search(s.name,s.expired,s.status);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        public class SearchRequest
        {
            public string? name { get; set; }

            public bool? status { get; set; }

            public bool? expired { get; set; }
        }
    }
}
