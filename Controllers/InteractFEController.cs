using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_sem3.InterFace;
using Project_sem3.Models;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InteractFEController : ControllerBase
    {
        private readonly IInteractFE repo;

        public InteractFEController(IInteractFE repo)
        {
            this.repo = repo;
        }

        [HttpPost("CreateQuestion")]
        [Authorize(Roles = "customer")]
        public async Task<ActionResult> CreateQuestion(Question question)
        {
           var rs = await repo.CreateQuestion(question);
            if(rs == 200)
            {
                return Ok(new CustomResult { Status = 200, Message = "Send question success", data = question });
            }else if (rs == 201)
            {
                return Ok(new CustomResult { Status = 201, Message = "Send question fails", data = question });
            }
            return Ok(new CustomResult { Status = 500, Message = "Cath error", data = question });
        }



        [HttpPost("CreateRate")]
        [Authorize(Roles = "customer")]
        public async Task<ActionResult> CreateRate([FromBody] ListRate list)
        {
            var rs = await repo.CreateRate(list);
            if (rs == 200)
            {
                return Ok(new CustomResult { Status = 200, Message = "Send question success", data = list });
            }
            else if (rs == 201)
            {
                return Ok(new CustomResult { Status = 201, Message = "Send question fails", data = list });
            }
            return Ok(new CustomResult { Status = 500, Message = "Cath error", data = list });
        }

    }
}
