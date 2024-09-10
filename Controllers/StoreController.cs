using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_sem3.InterFace;
using Project_sem3.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SAdmin,Admin")]
    public class StoreController : ControllerBase
    {
        private readonly IStore _store;
        public StoreController(IStore store)
        {
            _store = store;
        }
        [HttpGet]

        public async Task<ActionResult> GetAllStore()
        {
            var result = await _store.GetAllStore();
            
           
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
        public async Task<ActionResult> CreateStore ([FromForm] Store s)
        {
            var result = await _store.CreateStore(s);
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
        public async Task<ActionResult> UpdateStore([FromForm] Store s)
        {
            var result = await _store.UpdateStore(s);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        //[HttpDelete("{id}")]
        //public async Task<ActionResult> Delete(int id)
        //{
        //    var result = await _store.DeleteStore(id);
        //    return Ok(result);
        //}
        [HttpGet("ChangeStatus/{id}")]
        public async Task<ActionResult> ChangeStatus (int id)
        {
            var result = await _store.ChangeStatus(id);
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
            var result = await _store.GetById(id);
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
        public async Task<ActionResult> Search([FromForm] SearchRequest s)
        {
            var result = await _store.Search(s.address,s.city,s.district,s.status);
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
            public string? address { get; set; }
            public string? district { get; set; }
            public string? city { get; set; }
            public bool? status { get; set; }
        }

    }
}
