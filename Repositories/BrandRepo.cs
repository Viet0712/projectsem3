using Microsoft.EntityFrameworkCore;
using Project_sem3.InterFace;
using Project_sem3.Models;
using System.Collections.Generic;
using static System.Runtime.InteropServices.JavaScript.JSType;
using TableDependency.SqlClient.Base.Messages;

namespace Project_sem3.Repositories
{
    public class BrandRepo : IBrand
    {
        private readonly dataContext _dataContext;
        private IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BrandRepo(dataContext dataContext, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<CustomResult> ChangeStatus(int id)
        {
            try
            {
                var data = await _dataContext.Brands.SingleOrDefaultAsync(e => e.Id == id);
                if (data == null)
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Record Not Found!",
                    };
                }
                else
                {
                    data.Status = !data.Status;
                    data.Update_at = DateTime.Now;
                    _dataContext.Brands.Update(data);
                    await _dataContext.SaveChangesAsync();
                   

                    return new CustomResult()
                    {

                        Status = 200,
                        Message = "Change Status Success!",
                        data = data
                    };
                }
            }
            catch(Exception ex)
            {
               
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error " + ex.Message,

                };
            }
        }

        public async Task<CustomResult> CreateBrand(Brand brand)
        {
            try
            {
                var data = await _dataContext.Brands.SingleOrDefaultAsync(e=>e.Name.ToLower()== brand.Name.ToLower());  
                if (data != null)
                {
                    return new CustomResult()
                    {

                        Status = 205,
                        Message = "Duplicate Brand Name!",
                        data = brand
                    };
                }
                brand.Create_at = DateTime.Now;
                if (brand.UploadImage != null)
                {
                    var filename = GetUniqueFilename(brand.UploadImage.FileName);
                    var upload = Path.Combine(_env.WebRootPath, "BrandImage");
                    var filePath = Path.Combine(upload, filename);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await brand.UploadImage.CopyToAsync(stream);
                    }
                    brand.Image = filename;
                }
                else
                {
                    brand.Image = "null.jpg";
                }
                brand.Status = false;
                _dataContext.Brands.Add(brand);
                await _dataContext.SaveChangesAsync();
               

                return new CustomResult()
                {

                    Status = 200,
                    Message = "Create Brand Success!",
                    data = brand
                };
            }

            catch (Exception ex) {

               
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error " + ex.Message,

                };
            }
        }

        public async Task<CustomResult> GetAllBrandTrue()
        {
            try
            {
                var list = await _dataContext.Brands.Where(e=>e.Status==true).ToListAsync();
                foreach (var item in list)
                {
                    item.Image = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/BrandImage/{item.Image}";
                }
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Get Brand Success!",
                    data = list
                };
            }
            catch (Exception ex)
            {
                
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error " + ex.Message,

                };
            }
        }

        public async Task<CustomResult> SearchByName(string name)
        {
            try
            {
                var query = _dataContext.Brands.AsQueryable();

                if (!string.IsNullOrEmpty(name)&& name!="all")
                    query = query.Where(s => s.Name.Contains(name));
                if(name == "all")
                {
                    var a = await _dataContext.Brands.ToListAsync();
                    foreach (var item in a)
                    {
                        item.Image = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/BrandImage/{item.Image}";
                    }
                    return new CustomResult()
                    {
                        Status = 200,
                        Message = "Brand Match List!",
                        data = a
                    };
                }

                var result = await query.ToListAsync();
                foreach (var item in result)
                {
                    item.Image = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/BrandImage/{item.Image}";
                }
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Brand Match List!",
                    data = result
                };

            }


            catch (Exception ex)
            {

                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error " + ex.Message,

                };
            }
        }

        public async Task<CustomResult> UpdateBrand(Brand brand)
        {
            try
            {
                var dataOld = await _dataContext.Brands.SingleOrDefaultAsync(e=>e.Id== brand.Id);  
                if (dataOld != null)
                {
                    if (dataOld.Name.ToLower() != brand.Name.ToLower())
                    {
                        var data = await _dataContext.Brands.SingleOrDefaultAsync(e=>e.Name.ToLower()== brand.Name.ToLower());
                        if (data != null)
                        {
                            return new CustomResult()
                            {

                                Status = 205,
                                Message = "Duplicate Brand Name!",
                                data = dataOld
                            };
                        }
                      
                    }
                    if (brand.UploadImage != null)
                    {
                        var filename = GetUniqueFilename(brand.UploadImage.FileName);
                        var upload = Path.Combine(_env.WebRootPath, "BrandImage");
                        var filePath = Path.Combine(upload, filename);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await brand.UploadImage.CopyToAsync(stream);
                        }
                        dataOld.Image =filename ;
                    }

                    dataOld.Update_at = DateTime.Now;
                    dataOld.Description = brand.Description;
                  
                    dataOld.Name = brand.Name;

                    _dataContext.Brands.Update(dataOld);
                    await _dataContext.SaveChangesAsync();
                   

                    return new CustomResult()
                    {

                        Status = 200,
                        Message = "Update Brand Success!",
                        data = dataOld
                    };
                }
                else
                {
                   

                    return new CustomResult()
                    {

                        Status = 205,
                        Message = "Update Fail Brand Not Found!",
                        data = brand
                           
                    };
                }
               
            }

            catch (Exception ex)
            {

               
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error " + ex.Message,

                };
            }
        }

        public string GetUniqueFilename(string file)
        {
            file = Path.GetFileName(file);
            return Path.GetFileNameWithoutExtension(file) + "_" + DateTime.Now.Ticks + Path.GetExtension(file);
        }

        public async Task<CustomResult> GetById(int id)
        {
            try
            {
                var data = await _dataContext.Brands.SingleOrDefaultAsync(e=>e.Id== id);
                if (data == null)
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Brand Not Found!"
                    };
                }
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Get Brand Success!",
                    data = data
                };
            }catch (Exception ex)
            {
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error " + ex.Message,

                };
            }
        }

        public async Task<CustomResult> GetAllBrand()
        {
            try
            {
                var list = await _dataContext.Brands.ToListAsync();
                foreach (var item in list)
                {
                    item.Image = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/BrandImage/{item.Image}";
                }
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Get Brand Success!",
                    data = list
                };
            }
            catch (Exception ex)
            {

                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error " + ex.Message,

                };
            }
        }
    }
}
