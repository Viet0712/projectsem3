using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_sem3.Models;
using System.Diagnostics;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaceloginController : ControllerBase
    {
        private readonly dataContext _dataContext;
        public FaceloginController(dataContext dataContext)
        {
            _dataContext = dataContext;   
        }
        [HttpGet]
        public async Task<ActionResult> GetImage()
        {
            var list = await _dataContext.Admins.ToListAsync();
           
            foreach (var item in list)
            {
                item.Image = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/AdminImage/{item.Image}";
              
              
            }
            return Ok(list);
        }
    }
}
