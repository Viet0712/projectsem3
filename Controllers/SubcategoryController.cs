using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_sem3.InterFace;
using Project_sem3.Models;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SubcategoryController : ControllerBase
    {
        private readonly ISubcategory _subcategory;
        public SubcategoryController(ISubcategory subcategory)
        {
            _subcategory = subcategory;
        }
        [HttpGet]
        [Authorize(Roles = "SAdmin,Admin")]
        public async Task<ActionResult> GetAllTrue()
        {
            var result = await _subcategory.GetAllSubcategoryTrue();
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
            var result = await _subcategory.GetAllSubcategory();
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
        public async Task<ActionResult> CreateSubcategory([FromForm]Subcategory subcategory)
        {
            var result = await _subcategory.CreateCategory(subcategory);
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
        public async Task<ActionResult> UpdateSubcategory([FromForm] Subcategory subcategory)
        {
            var result = await _subcategory.UpdateSubCategory(subcategory);
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
            var result = await _subcategory.ChangeStatus(id);
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
            var result = await _subcategory.GetById(id);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }

        }
        [Authorize(Roles = "SAdmin,Admin")]
        [HttpPost("Search")]
        public async Task<ActionResult> Search([FromForm]SearchRequest s)
        {
            var result = await _subcategory.Search(s.name,s.status,s.categoryID);
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
            public int? categoryID { get; set; }
         
            public bool? status { get; set; }
        }
    }
}
