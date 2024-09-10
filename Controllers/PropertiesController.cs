using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_sem3.InterFace;
using Project_sem3.Models;
using Project_sem3.Repositories;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PropertiesController : ControllerBase
    {
        private readonly IProperties _propertiesRepo;
        public PropertiesController(IProperties properties)
        {
            _propertiesRepo = properties;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("{storeId}")]
        public async Task<ActionResult> GetAll(int storeId)
        {
            var result = await _propertiesRepo.GetAll(storeId);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [Authorize(Roles = "SAdmin,Admin")]
        [HttpGet("")]
        public async Task<ActionResult> GetAllTrue()
        {
            var result = await _propertiesRepo.GetAllTrue();
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [Authorize(Roles = "SAdmin")]
        [HttpGet("SAdmin")]
        public async Task<ActionResult> GetAll()
        {
            var result = await _propertiesRepo.GetAll();
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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ChangeStatus(int id)
        {
            var result = await _propertiesRepo.ChangeStatus(id);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpGet("ChangeStatusSAdmin/{id}")]
        [Authorize(Roles = "SAdmin")]
        public async Task<ActionResult> ChangeStatusSAdmin(int id)
        {
            var result = await _propertiesRepo.ChangeStatusSadmin(id);
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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Create([FromForm] Properties c)
        {
            var result = await _propertiesRepo.Create(c);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpPost("SAdmin")]
        [Authorize(Roles = "SAdmin")]
        public async Task<ActionResult> Create([FromForm] Product_Properties c)
        {
            var result = await _propertiesRepo.Create(c);
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
        [Authorize(Roles = "SAdmin")]
        public async Task<ActionResult> Update([FromForm] Product_Properties c)
        {
            var result = await _propertiesRepo.Update(c);
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
        [Authorize(Roles = "SAdmin")]
        public async Task<ActionResult> GetById(int id)
        {
            var result = await _propertiesRepo.GetById(id);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpGet("GetByIdRoleAdmin/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> GetByIdRoleAdmin(int id)
        {
            var result = await _propertiesRepo.GetByIdAdmin(id);
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
