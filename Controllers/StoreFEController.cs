using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_sem3.InterFace;
using Project_sem3.Models;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StoreFEController : ControllerBase
    {
        private readonly IStoreFE repo;

        public StoreFEController(IStoreFE repo)
        {
            this.repo = repo;
        }

        [HttpGet("Get")]
        public async Task<ActionResult> Get()
        {
            var list = await repo.Get();
            if (list == null)
            {
                return Ok(new CustomResult { Status = 501, Message = "Get data fail", data = null });
            }
            if (list.Count() == 0)
            {
                return Ok(new CustomResult { Status = 201, Message = "Data is empty", data = null });
            }
            return Ok(new CustomResult { Status = 200, Message = "Get data success", data = list });

        }
    }
}
