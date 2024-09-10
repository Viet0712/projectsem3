using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_sem3.InterFace;
using Project_sem3.Models;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RateController : ControllerBase
    {
        private readonly IRate _rateRepo;
        public RateController(IRate IRate)
        {
            _rateRepo = IRate;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetReview/{storeid}")]
        public async Task<ActionResult> GetAll(int storeid)
        {
            var result = await _rateRepo.GetAll(storeid);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("GetByIdRateRep/{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var result = await _rateRepo.GetById(id);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPut()]
        public async Task<ActionResult> UpdateRateRep([FromForm] Rate_Reply r)
        {
            var result = await _rateRepo.UpdateFeedBack(r);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [Authorize(Roles = "Admin")]
        [HttpPost()]
        public async Task<ActionResult> SendFeedBack([FromForm] Rate_Reply r)
        {
            var result = await _rateRepo.SendFeedBack(r);
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
