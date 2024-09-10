using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_sem3.InterFace;
using Project_sem3.Models;
using Project_sem3.Repositories;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SAdmin,Admin")]
    public class FlashSaleController : ControllerBase
    {
        private readonly IFlashSale _flashSalerepo;
        public FlashSaleController(IFlashSale flashSale)
        {
            _flashSalerepo = flashSale;
        }
        [HttpPost]
        public async Task<ActionResult> Create ([FromForm]Flash_Sale f)
        {
            var result = await _flashSalerepo.Create(f);

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
        public async Task<ActionResult> Update([FromForm] Flash_Sale f)
        {
            var result = await _flashSalerepo.Update(f);

            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetAllTrue()
        {
            var result = await _flashSalerepo.GetAllTrue();

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
            var result = await _flashSalerepo.GetAll();

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
        public async Task<ActionResult> GetById(int id)
        {
            var result = await _flashSalerepo.GetById(id);

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
        public async Task<ActionResult> ChangeStatus(int id)
        {
            var result = await _flashSalerepo.ChangeStatus(id);

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
        public async Task<ActionResult> Search([FromForm]SearchRequest s)
        {
            var result = await _flashSalerepo.Search(s.name,s.expired,s.status);

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
            public string? name {  get; set; }

            public bool? status { get; set; }

            public bool? expired { get; set; }
        }
    }
}
