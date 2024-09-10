using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Project_sem3.InterFace;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermission  _permission;
        public PermissionController(IPermission permission)
        {
            _permission = permission;
        }

        [HttpGet("GetAll")]
        [Authorize(Roles = "SAdmin")]
        public async Task<IActionResult> GetAll()
        {
            var result = await _permission.getAll();
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("GetbyId/{id}")]
        [Authorize(Roles = "SAdmin,Admin")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _permission.getByIdAdmin(id);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpGet("UpdatePermission/{id}/{type}")]
        [Authorize(Roles = "SAdmin")]
        public async Task<IActionResult> UpdatePermission(int id , string type)
        {
            var result = await _permission.Update(id , type);
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
