
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Project_sem3.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;
using System.Text;

namespace Project_sem3
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly dataContext _dataContext;
        private readonly IConfiguration _configuration;
        public AuthController(dataContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            _dataContext = context;
        }



        [AllowAnonymous]
        [HttpPost("CheckLogin")]
        public async Task<ActionResult> Login([FromForm] AccountLogin accountLogin)
        {
            var accCheck = await Authenticate(accountLogin);
            if (accCheck == null)
            {
                return NotFound("User Not Found");
            }
            var token = GenerateToken(accCheck);
            var Store = "";
            if(accCheck.Store != null)
            {
                Store = accCheck.Store.Address + " " + accCheck.Store.District + " " + accCheck.Store.City;
            }
            accCheck.Image = $"{this.Request.Scheme}://{this.Request.Host}{this.Request.PathBase}/AdminImage/{accCheck.Image}";
            return Ok(new { token = token, role = accCheck.Role , email = accCheck.Email , StoreAddress = Store, StoreId = accCheck.StoreId, image = accCheck.Image , AdminId = accCheck.Id });
        }
        [NonAction]
        private string GenerateToken(Admin account)
        {
            var security = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(security, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
               
                new Claim("Email",account.Email),
                new Claim("Id",account.Id.ToString()),
                new Claim(ClaimTypes.Role,account.Role),

            };
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"], _configuration["Jwt:Audience"],
                claims, expires: DateTime.Now.AddHours(24), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        [NonAction]
        private async Task<Admin> Authenticate(AccountLogin accountLogin)
        {
            var ac = await _dataContext.Admins.Include(e=>e.Store).SingleOrDefaultAsync(a=>a.Email==accountLogin.Email);
            if(ac != null&& ac.Status && BCrypt.Net.BCrypt.Verify(accountLogin.Password, ac.Password))
            {
                ac.IsOnline = true;
                _dataContext.Admins.Update(ac);
                await _dataContext.SaveChangesAsync();
                return ac;
            }
            else
            {
                return null;
            }

              
            
        }
       
    }
}
