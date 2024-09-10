using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Project_sem3.InterFace;
using Project_sem3.Model;
using Project_sem3.Models;
using Project_sem3.SendMail;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthFEController : ControllerBase
    {
        private readonly dataContext _dataContext;
        private readonly IAuthFE repo;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _mail;

        public AuthFEController(IAuthFE repo, IConfiguration configuration,dataContext _dataContext,IEmailService _mail)
        {
            this.repo = repo;
            _configuration = configuration;
            this._dataContext = _dataContext;
            this._mail = _mail;
        }

        [HttpPost("Login")]
        public async Task<ActionResult> Login(AccountLogin acc)
        {
            var user = await repo.Login(acc);
            if(user == null)
            {
                return Ok(new CustomResult{ Status=202,Message="Not user",data=null});
            }if(user.Status == "inactive")
            {
                return Ok(new CustomResult { Status = 201, Message = "Account non active", data = null });
            }
            var tokken = GenerateToken(user);
            return Ok(new CustomResult { Status = 200, Message = "user login success", data = tokken });
        }



        [HttpPost("LoginGoogle")]
        public async Task<ActionResult> LoginGoogle(User user)
        {
           var kq = await repo.LoginGoogle(user);
            if(kq.Status == 200)
            {
                var tokken = GenerateToken(user);
                return Ok(new CustomResult { Status = 200, Message = "user login success", data = tokken });
            }
            return Ok(new CustomResult { Status = 202, Message = "Login Google Failed", data = null });
        }






        [HttpGet("Get/{email}")]
        [Authorize(Roles = "customer")]
        public async Task<ActionResult> Get(string email)
        {
            var account = await repo.Get(email);
            if(account == null)
            {
                return Ok(new CustomResult {Status = 201,Message = "Account tồn tại", data = null});
            }
            return Ok(new CustomResult { Status = 200, Message = "Get account success", data = account });
        }

        [NonAction]
        private string GenerateToken(User user)
        {
            var security = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(security, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim("Fullname",user.FullName),
                new Claim("Email",user.Email),
               
                new Claim(ClaimTypes.Role,user.Role),

            };
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"],
                claims, expires: DateTime.Now.AddHours(5), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
      


        [HttpPost("Register")]
        public async Task<ActionResult> Register(User user)
        {
            var result = await repo.Register(user);
            switch (result)
            {
                case 200:
                    MailRequest mailRequest = new MailRequest();
                    mailRequest.Subject = "Verify Sign Up";
                    mailRequest.UserName = user.FullName;
                    mailRequest.ToEmail = user.Email;
                    mailRequest.Body = "Congratulations on successfully registering as a member! Please click the attached link to activate your account.";

                    await _mail.SendEmailAsync(mailRequest);
                    return Ok(new CustomResult
                    {
                        Status = 200,
                        Message = "Register success",
                        data = null
                    });

                case 201:
                    return Ok(new CustomResult
                    {
                        Status = 201,
                        Message = "Email tồn tại",
                        data = null
                    });

                case 202:
                    return Ok(new CustomResult
                    {
                        Status = 203,
                        Message = "Register not success!",
                        data = null
                    });

                default:
                    return Ok(new CustomResult
                    {
                        Status = 400,
                        Message = "Error",
                        data = null
                    });
            }
        }

        [HttpGet("Verify/{email}/{timer}")]
        public async Task<ActionResult> VerifyEmail(string email, string timer)
        {
           var rs = await repo.Verify(email, timer);
            if(rs == 200)
            {
                return Ok("Xác thực thành công !");
            }else if(rs == 201)
            {
                return Ok("Tài khoản không tồn tại !");
            }else if(rs == 202)
            {
                return Ok("Đã hết thời gian xác thực !");
            }
            else
            {
                return Ok("Error!! vui lòng liên hệ hỗ trợ từ website");
            }
        }

        [HttpPost("ForgotPassword/{email}")]
        public async Task<ActionResult> ForgotPassword(string email)
        {
            var rs = await repo.ForgotPassword(email);
            return Ok(rs);
        }


    }
}
