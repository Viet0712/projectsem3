using Microsoft.EntityFrameworkCore;
using Project_sem3.InterFace;
using Project_sem3.Model;
using Project_sem3.Models;
using Project_sem3.SendMail;
using System.Collections.Generic;
using System.Globalization;
using TableDependency.SqlClient.Base.Messages;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Project_sem3.Repositories
{
    public class AdminRepo : IAdmin
    {
        private readonly dataContext _dataContext;
        private IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailService;

        public AdminRepo(dataContext dataContext, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor, IConfiguration configuration, IEmailService emailSender)
        {
            _dataContext = dataContext;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
            _emailService = emailSender;

        }
        public async Task<CustomResult> ChangeStatus(int id)
        {
            try
            {
                var data = await _dataContext.Admins.SingleOrDefaultAsync(e => e.Id== id);
                if (data == null)
                {
                  
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Record Not Found! ",
                 
                    };
                }
                else
                {
                    data.Status = !data.Status;
                    data.Update_at = DateTime.Now;
                    _dataContext.Admins.Update(data);
                    await _dataContext.SaveChangesAsync();
                   
                    return new CustomResult()
                    {
                        Status = 200,
                        Message = "Change Status Success ",
                        data = data

                    };
                }
            }
            catch (Exception ex)
            {
                
               
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error! " + ex.Message,


                };
            }
          
        }

        public async Task<CustomResult> CreateAdmin(Admin e)
        {

            var data =await _dataContext.Admins.SingleOrDefaultAsync(a=>a.Email == e.Email);
            if(data != null)
            {
                return new CustomResult()
                {
                    Status = 205,
                    Message = "Create Fail Email Duplicate!!",
                    data = e
                };
            }
           
            e.Create_at = DateTime.Now;
            e.IsOnline = false;
            try
            {
                if (e.UploadImage != null)
                {
                    var filename = GetUniqueFilename(e.UploadImage.FileName);
                    var upload = Path.Combine(_env.WebRootPath, "AdminImage");
                    var filePath = Path.Combine(upload, filename);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await e.UploadImage.CopyToAsync(stream);
                    }


                    e.Image = filename;
               

                }
                else
                {
                    e.Image = "null.jpg";
                   
                }
                e.Password = BCrypt.Net.BCrypt.HashPassword(e.Password);
                e.Status = false;
                _dataContext.Admins.Add(e);
                await _dataContext.SaveChangesAsync();
                var admin = await _dataContext.Admins.SingleOrDefaultAsync(e=>e.Email==e.Email);
                var permission = new Permissions() { AdminId = admin.Id , AddGoods = true , AddProperties = true, SetEven = true };
                _dataContext.Permissions.Add(permission);
                await _dataContext.SaveChangesAsync();


                return new CustomResult()
                {
                    Status = 200,
                    Message = "Create Success",
                    data = e

                };

            }
            catch (Exception ex)
            {
                
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error! " + ex.Message,


                };
            }
        }

        public async Task<CustomResult> GetAllAdmin()
        {
            try
            {
                var list = await _dataContext.Admins.Include("Store").Where(e=>e.Role=="Admin").ToListAsync();
                foreach (var admin in list)
                {
                    admin.Image = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/AdminImage/{admin.Image}";
                }
               
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Success!",
                    data = list
                };
            }
            catch (Exception ex)
            {
               
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error! " + ex.Message,

                };
            }
        }

        public async Task<CustomResult> UpdateAdmin(Admin e)
        {
           
            try
            {
               
                var dataOld = await _dataContext.Admins.SingleOrDefaultAsync(a=>a.Id == e.Id);
              
                if (dataOld != null)
                {
                    if (dataOld.Email != e.Email)
                    {
                        var checkemail = await _dataContext.Admins.SingleOrDefaultAsync(a => a.Email == e.Email);
                        if (checkemail != null)
                        {
                            return new CustomResult()
                            {
                                Status = 205,
                                Message = "Update Admin Fail , Duplicate Email!",
                                data = e

                            };
                        }
                    }
                    if (e.UploadImage != null)
                    {
                        var filename = GetUniqueFilename(e.UploadImage.FileName);
                        var upload = Path.Combine(_env.WebRootPath, "AdminImage");
                        var filePath = Path.Combine(upload, filename);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await e.UploadImage.CopyToAsync(stream);
                        }


                        dataOld.Image = filename;
                      

                    }
                    dataOld.Role = e.Role;
                    dataOld.Email = e.Email;
                    if (dataOld.Password!= e.Password)
                    {
                        dataOld.Password = BCrypt.Net.BCrypt.HashPassword(e.Password);
                    }
                   
                    dataOld.FullName = e.FullName;
                  
                    dataOld.Phone = e.Phone;
                    dataOld.StoreId = e.StoreId;
                    dataOld.Update_at = DateTime.Now;
                    _dataContext.Admins.Update(dataOld);
                    await _dataContext.SaveChangesAsync();

                    return new CustomResult()
                    {
                        Status = 200,
                        Message = "Update Success",
                        data = dataOld

                    };

                }

                else
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Update Fail Admin Not Found",
                        data = e

                    };
                }

            }
            catch (Exception ex)
            {
               
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error! " + ex.Message,


                };
            }
        }
        public string GetUniqueFilename(string file)
        {
            file = Path.GetFileName(file);
            return Path.GetFileNameWithoutExtension(file) + "_" + DateTime.Now.Ticks + Path.GetExtension(file);
        }

        public async Task<CustomResult> Search(string? name, string? role, bool? status)
        {
            try
            {
                var query = _dataContext.Admins.Where(e=>e.Role=="Admin").AsQueryable();

                if (!string.IsNullOrEmpty(name))
                    query = query.Where(s => s.FullName.Contains(name));

                if (!string.IsNullOrEmpty(role))
                    query = query.Where(s => s.Role==role);

              

                if (status.HasValue)
                    query = query.Where(s => s.Status == status.Value);

                var result = await query.ToListAsync();
                foreach (var admin in result)
                {
                    admin.Image = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/AdminImage/{admin.Image}";
                }

                return new CustomResult()
                {
                    Status = 200,
                    Message = "Search Success!",
                    data = result
                };
            }
            catch (Exception ex)
            {

                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error! " + ex.Message,


                };
            }


        }

        public async Task<CustomResult> Verify(string email,string timeCreate)
        {

            string format = "dd-MM-yyyy-HH:mm:ss";
            DateTime dateTimeCreate = DateTime.ParseExact(timeCreate, format, CultureInfo.InvariantCulture);
            TimeSpan timeDiff = DateTime.Now - dateTimeCreate;
            if (timeDiff.TotalHours >= 1)
            {
                return new CustomResult()
                {
                    Status = 306,
                    Message = "Verify Fail , Link expiry!!",
                   
                };
            }
            else
            {
                try
                {
                    var data = await _dataContext.Admins.SingleOrDefaultAsync(e => e.Email == email);
                    if (data == null)
                    {
                        return new CustomResult()
                        {
                            Status = 205,
                            Message = "Verify Fail , Record Not Found!!",
                            data = data
                        };
                    }
                    else
                    {
                        if (data.Status == false)
                        {
                            data.Status = true;
                           await _dataContext.SaveChangesAsync();
                            return new CustomResult()
                            {
                                Status = 200,
                                Message = "Verify Success!!",
                                data = data
                            };
                        }
                        else
                        {
                            return new CustomResult()
                            {
                                Status = 201,
                                Message = "Email Has Been Verified Already !!",
                                data = data
                            };
                        }
                    }

                }
                catch (Exception ex)
                {
                    return new CustomResult()
                    {
                        Status = 400,
                        Message = "Server Error! " + ex.Message,
                        data = null

                    };
                }
            }
           
        }

        public async Task<CustomResult> GetById(int id)
        {
            try
            {
                var data = await _dataContext.Admins.SingleOrDefaultAsync(e => e.Id == id);
                if(data == null)
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Admin Not Found!"
                    };
                }
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Get Admin Success!",
                    data = data
                };
            }
            catch (Exception ex)
            {
                return new CustomResult()
                {

                    Status = 400,
                    Message = "Server Error! " + ex.Message,
                  

                };
            }
        }

        public async Task<CustomResult> LogOut(string email)
        {
            try {
                var data = await _dataContext.Admins.SingleOrDefaultAsync(e=>e.Email == email);
                data.IsOnline = false;
                _dataContext.Admins.Update(data);
                await _dataContext.SaveChangesAsync();
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Log Out Success!"
                };
            
            }
            catch (Exception ex)
            {
                return new CustomResult()
                {

                    Status = 400,
                    Message = "Server Error! " + ex.Message,


                };
            }
        }

        public async Task<CustomResult> ForgotPassword(string email)
        {
            try { 
                var data = await _dataContext.Admins.SingleOrDefaultAsync(e=>e.Email == email);
                if (data == null)
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Email Not Found!"
                    };
                }
                var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
                var stringChars = new char[8];
                var random = new Random();

                for (int i = 0; i < stringChars.Length; i++)
                {
                    stringChars[i] = chars[random.Next(chars.Length)];
                }

              
                var Password = new System.String(stringChars);
                 data.Password = BCrypt.Net.BCrypt.HashPassword(Password);
                _dataContext.Admins.Update(data);
                await _dataContext.SaveChangesAsync();

                MailRequest mailRequest = new MailRequest();
                mailRequest.Subject = "Forget Password";
                mailRequest.UserName = data.FullName;
                mailRequest.ToEmail = data.Email;
                mailRequest.Body = "Here is your new password.";

                await _emailService.ForgetPassword(mailRequest,Password);

                return new CustomResult()
                {
                    Status = 200,
                    Message = "Reset Password Success!"
                };
            }
            catch(Exception ex)
            {
                return new CustomResult()
                {

                    Status = 400,
                    Message = "Server Error! " + ex.Message,


                };
            }
        }
    }
}
