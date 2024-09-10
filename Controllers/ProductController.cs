using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_sem3.InterFace;
using Project_sem3.Models;
using static Org.BouncyCastle.Asn1.Cmp.Challenge;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class ProductController : ControllerBase
    {
        private readonly IProduct _productRepo;
        public ProductController(IProduct product)
        {
            _productRepo = product;
        }
        [Authorize(Roles = "SAdmin,Admin")]
        [HttpGet]
        public async Task<ActionResult> GetAllTrue()
        {
            var result = await _productRepo.GetAllTrue();
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
        [HttpGet("SAdmin")]
        public async Task<ActionResult> GetAll()
        {
            var result = await _productRepo.GetAll();
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
        public async Task<ActionResult> ChangeStatus(string id)
        {
            var result = await _productRepo.ChangeStatus(id);
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
        public async Task<ActionResult> Create([FromForm] Product c)
        {
            var result = await _productRepo.Create(c);
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
        public async Task<ActionResult> Update([FromForm] Product c)
        {
            var result = await _productRepo.Update(c);
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
        public async Task<ActionResult> GetById(string id)
        {
            var result = await _productRepo.GetById(id);
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
            var result = await _productRepo.Search(s.name, s.status,s.brandId,s.categoryId,s.subcategoryId,s.segmentId);
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

            public int? brandId { get; set; }
            public int? categoryId { get; set; }
            public int? subcategoryId { get; set; }
            public int? segmentId { get; set; }
            public bool? status { get; set; }
        }
    }
}
