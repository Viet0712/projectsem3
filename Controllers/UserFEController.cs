              using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_sem3.InterFace;
using Project_sem3.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

  
    public class UserFEController : ControllerBase
    {
        private readonly IUserFE repo;

        public UserFEController(IUserFE repo)
        {
            this.repo = repo;
        }


        [HttpPost("UpdateInfo")]
      [Authorize(Roles = "customer")]
        public async Task<ActionResult> UpdateInfo(User user)
        {
           
            var acc = await repo.UpdateInfo(user);
            if (acc == 200)
            {
                return Ok(new CustomResult { Status = 200, Message = "Update Success", data = null });
            }else if(acc == 201)
            {
                return Ok(new CustomResult { Status = 201, Message = "Update không thành công", data = null });

            }
            else if (acc == 202)
            {
                return Ok(new CustomResult { Status = 202, Message = "Account không tồn tại", data = null });

            }
            else
            {
                return Ok(new CustomResult { Status = 500, Message = "Error", data = null });
            }

        }


        [HttpPost("UpdateCard")]
        [Authorize(Roles = "customer")]
        public async Task<ActionResult> UpdateCard(User user)
        {
            var acc = await repo.UpdateCard(user);
            if (acc == 200)
            {
                return Ok(new CustomResult { Status = 200, Message = "Update Success", data = null });
            }
            else if (acc == 201)
            {
                return Ok(new CustomResult { Status = 201, Message = "Update không thành công", data = null });

            }
            else if (acc == 202)
            {
                return Ok(new CustomResult { Status = 202, Message = "Account không tồn tại", data = null });

            }
            else
            {
                return Ok(new CustomResult { Status = 500, Message = "Error", data = null });
            }

        }


        [HttpPost("UpdatePassword")]
        [Authorize(Roles = "customer")]
        public async Task<ActionResult> UpdatePassword(User user)
        {
            var acc = await repo.UpdatePassword(user);
            if (acc == 200)
            {
                return Ok(new CustomResult { Status = 200, Message = "Update Success", data = null });
            }
            else if (acc == 201)
            {
                return Ok(new CustomResult { Status = 201, Message = "password sai", data = null });

            }
            else if (acc == 202)
            {
                return Ok(new CustomResult { Status = 202, Message = "Account không tồn tại", data = null });
            }
            else if (acc == 203)
            {
                return Ok(new CustomResult { Status = 203, Message = "Login Google Không tồn tại password", data = null });
            }
            else
            {
                return Ok(new CustomResult { Status = 500, Message = "Error", data = null });
            }

        }

    }
}
