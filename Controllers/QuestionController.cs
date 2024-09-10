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
    public class QuestionController : ControllerBase
    {
        private readonly IQuestion _questionRepo;
        public QuestionController(IQuestion question)
        {
            _questionRepo = question;
        }
        [Authorize(Roles = "SAdmin")]
        [HttpGet()]
        public async Task<ActionResult> GetAll()
        {
            var result = await _questionRepo.GetAll();
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
        [HttpPost()]
        public async Task<ActionResult> SendFeedBack([FromForm] Question_Reply q)
        {
            var result = await _questionRepo.SendFeedBack(q);
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
        [HttpGet("GetByIdQues/{id}")]
        public async Task<ActionResult> GetByIdQues(int id)
        {
            var result = await _questionRepo.GetById(id);
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
        [HttpPut()]
        public async Task<ActionResult> UpdateFeedBack([FromForm] Question_Reply q)
        {
            var result = await _questionRepo.UpdateFeedBack(q);
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
