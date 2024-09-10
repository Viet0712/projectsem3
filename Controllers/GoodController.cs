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
    public class GoodController : ControllerBase
    {
        private readonly IGoods _goodsRepo;
        public GoodController(IGoods goods)
        {
            _goodsRepo = goods;
        }
        [Authorize(Roles = "SAdmin,Admin")]
        [HttpGet("GetByStore/{id}")]
        public async Task<ActionResult> GetAll(int id )
        {
            var result = await _goodsRepo.GetAll(id);
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
            var result = await _goodsRepo.ChangeStatus(id);
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
        public async Task<ActionResult> Create([FromForm] Goods c)
        {
            var result = await _goodsRepo.Create(c);
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
        public async Task<ActionResult> Update([FromForm] Goods c)
        {
            var result = await _goodsRepo.Update(c);
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
            var result = await _goodsRepo.GetById(id);
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
