using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_sem3.InterFace;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SAdmin,Admin")]
    public class EventController : ControllerBase
    {
        private readonly IEvent _eventRepo;

        public EventController(IEvent ievent)
        {
            _eventRepo = ievent;
        }
        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var result = await _eventRepo.Get();
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpGet("RemoveDiscount/{id}")]
        public async Task<ActionResult> RemoveDiscount(int id)
        {
            var result = await _eventRepo.RemoveDiscount(id);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpGet("RemoveFlashSale/{id}")]
        public async Task<ActionResult> RemoveFlashSale(int id)
        {
            var result = await _eventRepo.RemoveFlashSale(id);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpPost("SetDiscount")]
        public async Task<ActionResult> SetDiscount([FromForm] List<int> PropertiesId , [FromForm] int DiscountId)
        {
            var result = await _eventRepo.SetDiscount(PropertiesId , DiscountId);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpPost("SetFlashSale")]
        public async Task<ActionResult> SetFlashSale([FromForm] List<int> PropertiesId, [FromForm] int FlashSaleId)
        {
            var result = await _eventRepo.SetFlashSale(PropertiesId, FlashSaleId);
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
