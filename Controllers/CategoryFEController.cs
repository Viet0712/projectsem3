using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_sem3.InterFace;
using Project_sem3.Models;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryFEController : ControllerBase
    {
        private readonly ICategoryFE repo;

        public CategoryFEController(ICategoryFE repo)
        {
            this.repo = repo;
        }

        [HttpGet("AllCategory")]
        public async Task<ActionResult> GetAllCatelory()
        {
            var list = await repo.GetAllCategory();
            if (list == null)
            {
                return Ok(new CustomResult { Status = 501,Message="Get data fail",data=null });
            }if(list.Count()==0)
            {
                return Ok(new CustomResult { Status = 201, Message = "Data is empty", data = null });
            }
            return Ok(new CustomResult { Status = 200, Message = "Get data success", data = list });
        }
        [HttpGet("Category/{id}")]
        public async Task<ActionResult> GetCatelory(int id)
        {
            var list = await repo.GetCategory(id);
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
        [HttpGet("SubCategory/{id}")]
        public async Task<ActionResult> GetSubCatelory(int id)
        {
            var list = await repo.GetSubCategory(id);
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
        [HttpGet("Segment/{id}")]
        public async Task<ActionResult> Segment(int id)
        {
            var list = await repo.GetSegment(id);
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
