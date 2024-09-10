using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_sem3.InterFace;
using Project_sem3.Models;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VoucherFEController : ControllerBase
    {
        private readonly IVoucherFE repo;

        public VoucherFEController(IVoucherFE repo)
        {
            this.repo = repo;
        }

        [HttpGet("Get")]
        public async Task<IActionResult> Get()
        {
            var list = await repo.GetVouchers();
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

        [HttpGet("Get/{email}")]
        public async Task<IActionResult> Get(string email)
        {
            var list = await repo.GetVoucher(email);
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


        [HttpGet("GetCustomer/{email}")]
        public async Task<IActionResult> GetCustomer(string email)
        {
            var list = await repo.GetVouchersCustomer(email);
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
        [HttpGet("Collect/{email}/{id}")]
        public async Task<ActionResult> CollectVoucher(string email, int id)
        {
            var result = await repo.CollectVoucher(email, id);
            if (result == 200)
            {
                return Ok(new CustomResult { Status = 200, Message = "Collect voucher Success", data = null });
            }
            else if (result == 201)
            {
                return Ok(new CustomResult { Status = 201, Message = "Collect voucher fails", data = null });

            }
            else if (result == 202)
            {
                return Ok(new CustomResult { Status = 202, Message = "Account không tồn tại", data = null });
            }
            else if (result == 203)
            {
                return Ok(new CustomResult { Status = 203, Message = "Voucher quantity =0", data = null });
            }
            else
            {
                return Ok(new CustomResult { Status = 500, Message = "Error", data = null });
            }
        }
    }
}
