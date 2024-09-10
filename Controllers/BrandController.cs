using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_sem3.InterFace;
using Project_sem3.Models;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly IBrand _brand;
        public BrandController(IBrand brand)
        {
            _brand = brand;
        }

        [HttpGet]
        [Authorize(Roles = "SAdmin,Admin")]
        public async Task<ActionResult> GetAllBrandTrue()
        {
            var result = await _brand.GetAllBrandTrue();
            if(result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpGet("SAdmin")]
        [Authorize(Roles = "SAdmin,Admin")]
        public async Task<ActionResult> GetAllBrand()
        {
            var result = await _brand.GetAllBrand();
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
            var result = await _brand.ChangeStatus(id);
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
        public async Task<ActionResult> CreateBrands([FromForm] Brand brand)
        {
            var result = await _brand.CreateBrand(brand);
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
        public async Task<ActionResult> UpdateBrands([FromForm] Brand brand)
        {
            var result = await _brand.UpdateBrand(brand);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpGet("Search/{name}")]
        [Authorize(Roles = "SAdmin,Admin")]
        public async Task<ActionResult> SearchByName (string name)
        {
            var result = await _brand.SearchByName(name);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else if (result.Status == 205)
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
            var result = await _brand.GetById(id);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else if (result.Status == 205)
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
