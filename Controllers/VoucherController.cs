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
    [Authorize(Roles = "SAdmin,Admin")]
    public class VoucherController : ControllerBase
    {
        private readonly IVoucher _voucherRepo;
        public VoucherController(IVoucher IVoucher)
        {
            _voucherRepo = IVoucher;
        }
        [HttpPost]
        public async Task<ActionResult> Create([FromForm] Voucher f)
        {
            var result = await _voucherRepo.Create(f);

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
        public async Task<ActionResult> Update([FromForm] Voucher f)
        {
            var result = await _voucherRepo.Update(f);

            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpGet]
        public async Task<ActionResult> GetAllTrue()
        {
            var result = await _voucherRepo.GetAllTrue();

            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [HttpGet("SAdmin")]
        [Authorize(Roles = "SAdmin")]
        public async Task<ActionResult> GetAll()
        {
            var result = await _voucherRepo.GetAll();

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
            var result = await _voucherRepo.GetById(id);

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
        public async Task<ActionResult> ChangeStatus(int id)
        {
            var result = await _voucherRepo.ChangeStatus(id);

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
            var result = await _voucherRepo.Search(s.name, s.expired, s.status,s.type);

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
            public string? name { get; set; }
            public string? type { get; set; }
            public bool? status { get; set; }

            public bool? expired { get; set; }
        }
    }
}
