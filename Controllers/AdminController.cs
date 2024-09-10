using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Project_sem3.InterFace;
using Project_sem3.Model;
using Project_sem3.Models;
using Project_sem3.SendMail;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
  
    public class AdminController : ControllerBase
    {
        private readonly IAdmin _adminRepo;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;
        public AdminController(IAdmin admin, IConfiguration configuration, IEmailService emailSender)
        {
            _adminRepo = admin;
            _configuration = configuration;
            _emailService = emailSender;
        }
        [HttpGet]
        [Authorize(Roles = "SAdmin")]
        public async Task<ActionResult> GetAll()
        {
           
            var result = await _adminRepo.GetAllAdmin();

            if(result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }



        }
        [HttpPost]
        [Authorize(Roles = "SAdmin")]
        public async Task<ActionResult> CreateAdmin([FromForm] Admin a)
        {
            var result = await _adminRepo.CreateAdmin(a);
            if (result.Status == 200)
            {

                //MailRequest mailRequest = new MailRequest();
                //mailRequest.Subject = "Verify Sign Up";
                //mailRequest.UserName = a.FullName;
                //mailRequest.ToEmail = a.Email;
                //mailRequest.Body = "Congratulations on successfully registering as a member! Please click the attached link to activate your account.";

                //await _emailService.SendEmailAsync(mailRequest);
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        
        [HttpPut("Update")]
        [Authorize(Roles = "SAdmin,Admin")]
        public async Task<ActionResult> Update([FromForm] Admin a)
        {
            var result = await _adminRepo.UpdateAdmin(a);
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
        [Authorize(Roles = "SAdmin")]
        public async Task<ActionResult> ChangeStatus (int id)
        {
            var result = await _adminRepo.ChangeStatus(id);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [AllowAnonymous]
        [HttpGet("Verify/{email}/{timeCreate}")]
        public async Task<ActionResult> VerifyEmail(string email , string timeCreate)
        {
            var result = await _adminRepo.Verify(email, timeCreate);
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
        [Authorize(Roles = "SAdmin,Admin")]
        public async Task<ActionResult> GetById(int id)
        {
            var result = await _adminRepo.GetById(id);
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
        [Authorize(Roles = "SAdmin")]
        public async Task<ActionResult> Search([FromForm] SearchRequest s)
        {
            var result = await _adminRepo.Search(s.name,s.role ,s.status);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [AllowAnonymous]
        [HttpGet("LogOut/{email}")]
        public async Task<ActionResult> Logout (string email)
        {
            var result = await _adminRepo.LogOut(email);
            if (result.Status == 200)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
        [AllowAnonymous]
        [HttpGet("ForgotPassword/{email}")]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            var result = await _adminRepo.ForgotPassword(email);
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
            public string? role { get; set; }
            public bool? status { get; set; }
        }

    }
}
