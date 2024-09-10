using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_sem3.MobileInterface;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MbProductController : ControllerBase
    {
        private MbIProduct _mbiProduct;
        public MbProductController(MbIProduct MbIProduct)
        {
            _mbiProduct = MbIProduct;
        }

        [HttpGet("GetAll")]
        public async Task<ActionResult> GetAll()
        {
            var result = await _mbiProduct.GetAll();

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
