using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_sem3.InterFace;
using Project_sem3.Models;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategory _category;

        public CategoryController(ICategory category)
        {
            _category = category;
        }
        [HttpGet]
        [Authorize(Roles = "SAdmin,Admin")]
        public async Task<ActionResult> GetAllTrue()
        {
            var result = await _category.GetAllCategoryTrue();
            if(result.Status == 200) { 
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
            var result = await _category.GetAllCategory();
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
        public async Task<ActionResult> ChangeStatus (int id)
        {
            var result = await _category.ChangeStatus(id);
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
        public async Task<ActionResult> CreateCategory([FromForm]Category c)
        {
            var result = await _category.CreateCategory(c);
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
        public async Task<ActionResult> UpdateCategory([FromForm] Category c)
        {
            var result = await _category.UpdateCategory(c);
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
            var result = await _category.GetById(id);
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
        public async Task<ActionResult> Search([FromForm]SearchRequest s)
        {
            var result = await _category.Search(s.name,s.status);
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
        }
    }
}
