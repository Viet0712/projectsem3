using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_sem3.InterFace;
using Project_sem3.Models;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartFEController : ControllerBase
    {
        private readonly ICartFE repo;

        public CartFEController(ICartFE repo)
        {
            this.repo = repo;
        }
        [HttpGet("Get/{email}")]
        [Authorize(Roles ="customer")]
        public async Task<ActionResult> Get(string email)
        {
            var list = await repo.GetCart(email);
            if (list == null)
            {
                return Ok(new CustomResult { Status = 501, Message = "Get cart fail", data = null });
            }
            if (list.Count() == 0)
            {
                return Ok(new CustomResult { Status = 201, Message = "Data is empty", data = null });
            }
            return Ok(new CustomResult { Status = 200, Message = "Get data success", data = list });
        }

        [HttpPost("Update")]
        [Authorize(Roles = "customer")]
        public async Task<ActionResult> Update(Cart item)
        {
            var list = await repo.UpdateCart(item);
            if (list == null)
            {
                return Ok(new CustomResult { Status = 501, Message = "Update cart fail", data = null });
            }
            return Ok(new CustomResult { Status = 200, Message = "Get data success", data = list });
        }

        [HttpPost("Delete")]
        [Authorize(Roles = "customer")]
        public async Task<ActionResult> Delete(int[] id)
        {
            var list = await repo.DeleteCart(id);
            if (list == 0)
            {
                return Ok(new CustomResult { Status = 501, Message = "Update cart fail", data = null });
            }
            return Ok(new CustomResult { Status = 200, Message = "Delete success", data = list });
        }



        [HttpPost("Create")]
      [Authorize(Roles = "customer")]
        public async Task<ActionResult> Create(CartCreate ca)
        {
            var list = await repo.CreateCart(ca);
            if (list == null)
            {
                return Ok(new CustomResult { Status = 501, Message = "Create cart fail", data = null });
            }
            return Ok(new CustomResult { Status = 200, Message = "Create success", data = list });
        }
    }
}
