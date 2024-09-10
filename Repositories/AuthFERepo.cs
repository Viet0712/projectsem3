using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Project_sem3.InterFace;
using Project_sem3.Model;
using Project_sem3.Models;
using Project_sem3.SendMail;
using System.IO;
using System.Threading.Tasks;

namespace Project_sem3.Repositories
{
    public class AuthFERepo : IAuthFE
    {
        private readonly dataContext db;
        private readonly PasswordHasher<User> passwordHasher;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;

        public AuthFERepo(dataContext db, IHttpContextAccessor httpContextAccessor, IEmailService emailService)
        {
            this.db = db;
            this.passwordHasher = new PasswordHasher<User>();
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailService;
        }

        public async Task<User> Get(string email)
        {
            try
            {
                var acc = await db.Users.SingleOrDefaultAsync(p=>p.Email == email);
                if(acc != null)
                {
                    if(!acc.Image.Contains("http") && acc.Image != null && acc.Image!="")
                    {
                        acc.Image = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/UserImage/{acc.Image}";
                    }
                }
                return acc;
            }catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<User> Login(AccountLogin acc)
        {
            try
            {
                var user = await db.Users.SingleOrDefaultAsync(p => p.Email == acc.Email);
                if (user != null)
                {
                    
                    var result = passwordHasher.VerifyHashedPassword(user, user.Password, acc.Password);

                    if (result == PasswordVerificationResult.Success)
                    {
                        return user; 
                    }
                }
                return null;
            }
            catch (Exception ex)
            {
               
                return null;
            }
        }

        public async Task<CustomResult> LoginGoogle(User user)
        {
            try
            {
                var ac = await db.Users.SingleOrDefaultAsync(u=>u.Email == user.Email);
                if(ac != null)
                {
                    return new CustomResult()
                    {
                        Status = 200,
                        Message = "Duplicate Account",
                        data = ac
                    };
                }
                else
                {
                    user.Role = "customer";
                    db.Users.Add(user);
                    var result = await db.SaveChangesAsync();
                    if (result > 0)
                    {
                        return new CustomResult()
                        {
                            Status = 200,
                            Message = "Login Success",
                            data = user
                        };
                    }
                }
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Login fail"
                };
            }catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<int> Register(User user)
        {
            try
            {
                var acc = await db.Users.SingleOrDefaultAsync(p => p.Email == user.Email);
                if (acc != null)
                {
                    return 201;
                }
                user.Password = passwordHasher.HashPassword(user, user.Password);

                user.Role = "customer";
                if(user.Image == null)
                {
                    user.Image = "null.jpg";
                }
                db.Users.Add(user);
                var result = await db.SaveChangesAsync();

                if (result > 0)
                {
                    return 200;
                }
                return 201;
            }
            catch (Exception ex)
            {
                return 203;
            }
        }

        public async Task<int> Verify(string email, string timer)
        {
            try
            {

                DateTime timerDateTime = DateTime.ParseExact(timer, "dd-MM-yyyy-HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);
                DateTime now = DateTime.Now;
                TimeSpan difference = now - timerDateTime;
                if (difference.TotalHours <= 3 && difference.TotalHours > 0)
                {
                    var user = await db.Users.SingleOrDefaultAsync(u => u.Email == email);
                    if(user != null)
                    {
                        user.Status = "active";
                        await db.SaveChangesAsync();
                        return 200;
                    }
                    return 201;
                }
                else
                {
                    return 202;
                }
            }
            catch (Exception ex)
            {
                return 0;
            }
        }


        public async Task<CustomResult> ForgotPassword(string email)
        {
            try
            {
                var user = await db.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    return new CustomResult { Status = 404, Message = "User not found" };
                }

                var newPassword = GenerateRandomPassword(8);
                user.Password = passwordHasher.HashPassword(user, newPassword);

                db.Users.Update(user);
                var rs = await db.SaveChangesAsync();
                if (rs > 0)
                {
                    var mailrequest = new MailRequest()
                    {
                        ToEmail = email,
                        Subject = "Reset Password",
                        Body = $"Password: {newPassword}"
                    };
                    await _emailService.SendMailResetPass(mailrequest);
                    return new CustomResult { Status = 200, Message = "Password reset successfully" };
                }


                return new CustomResult { Status = 201, Message = "Password reset fails" };

            }
            catch (Exception ex)
            {
                return new CustomResult { Status = 203, Message = "Password reset catch", data = ex.Message };
            }
        }


        private string GenerateRandomPassword(int length)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            var random = new Random();
            return new string(Enumerable.Repeat(validChars, length)
                                        .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
