using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project_sem3.InterFace;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImportController : ControllerBase
    {
        private readonly IImportFile _importfileRepo;
        public ImportController(IImportFile IImportFile)
        {
            _importfileRepo = IImportFile;
        }
        [HttpPost("ImportProduct")]
        [Authorize(Roles = "SAdmin")]
        public async Task<ActionResult> ImportProduct([FromForm] FormImport f)
        {
            var result = await _importfileRepo.ImportFileProduct(f.file);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        public class FormImport
        {
            public IFormFile file { get; set; }
        }
    }
}
