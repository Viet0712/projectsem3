using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_sem3.InterFace;
using Project_sem3.Models;
using System.Collections.Generic;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductFEController : ControllerBase
    {
        private readonly dataContext db;
        private readonly IProductFE proRepo;
        public ProductFEController(dataContext db, IProductFE proRepo)
        {
            this.db = db;
            this.proRepo = proRepo;
        }

        [HttpGet("AllProduct/{storeid}")]
        public async Task<ActionResult> GetPro(int storeid)
        {
          var list = await proRepo.GetAllProducts(storeid);
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

        [HttpGet("Product/{storeid}/{id}")]
        public async Task<ActionResult> GetPro(int storeid, string id)
        {
            var product = await proRepo.GetProductsById(storeid, id);
            if (product == null)
            {
                return Ok(new CustomResult { Status = 201, Message = "Data is empty", data = null });
            }
            
            return Ok(new CustomResult { Status = 200, Message = "Get data success", data = product });
        }

        [HttpGet("FlashSale/{storeid}")]
        public async Task<ActionResult> GetFlashSale(int storeid)
        {
            var list = await proRepo.GetFlashSale(storeid);
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

        [HttpGet("Get/{storeid}/{grant}/{id}")]
        public async Task<ActionResult> GetProductCategory(int storeid ,string grant, int id)
        {
            var list = await proRepo.GetProductsCategory(storeid ,grant, id);
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

        [HttpGet("ProductSearch")]
        public async Task<ActionResult> Search()
        {
            var list = await proRepo.GetProductSearch();
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
