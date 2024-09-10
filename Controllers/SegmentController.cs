using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_sem3.InterFace;
using Project_sem3.Models;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SegmentController : ControllerBase
    {
        private readonly ISegment _segment;
        public SegmentController(ISegment segment)
        {
            _segment = segment;
        }
        [HttpGet]
        [Authorize(Roles = "SAdmin,Admin")]
        public async Task<ActionResult> GetAllTrue()
        {
            var result = await _segment.GetAllSegmentTrue();
            if(result.Status == 200) { 
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpGet("SAdmin")]
        [Authorize(Roles = "SAdmin,Admin")]
        public async Task<ActionResult> GetAll()
        {
            var result = await _segment.GetAllSegment();
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
        public async Task<ActionResult> CreateSegment([FromForm]Segment s)
        {
            var result = await _segment.CreateSegment(s);
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
        public async Task<ActionResult> UpdateSegment([FromForm] Segment s)
        {
            var result = await _segment.UpdateSegment(s);
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
            var result = await _segment.ChangeStatus(id);
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
            var result = await _segment.GetById(id);
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
            var result = await _segment.Search(s.name,s.status,s.categoryID,s.subcategoryID);
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
            public int? subcategoryID { get; set; }
            public bool? status { get; set; }
        }


    }
}
