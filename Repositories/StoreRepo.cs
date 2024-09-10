using Microsoft.EntityFrameworkCore;
using Project_sem3.InterFace;
using Project_sem3.Models;
using System.Collections.Generic;
using System.Diagnostics;

namespace Project_sem3.Repositories
{
    public class StoreRepo : IStore
    {
        private readonly dataContext _dataContext;
        private IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public StoreRepo(dataContext dataContext, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<CustomResult> CreateStore(Store store)
        {
            store.Create_at = DateTime.Now;
            store.Status = true;
            try
            {
                var data = await _dataContext.Stores.SingleOrDefaultAsync(e=>e.Address.ToLower()== store.Address.ToLower()&&e.District==store.District&&e.City==store.City);
                if(data != null)
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Duplicate Address",
                      

                    };
                }
                if(store.UploadImage != null)
                {
                    var filename = GetUniqueFilename(store.UploadImage.FileName);
                    var upload = Path.Combine(_env.WebRootPath, "StoreImage");
                    var filePath = Path.Combine(upload, filename);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await store.UploadImage.CopyToAsync(stream);
                    }


                    store.Image  = filename; 
                    _dataContext.Stores.AddAsync(store);
                    await _dataContext.SaveChangesAsync();
                    var newStore = await _dataContext.Stores.SingleOrDefaultAsync(e=>e.Address==store.Address&& e.District==store.District&&e.City==store.City);
                    var listproperties = await _dataContext.Product_Properties.ToListAsync();
                    foreach(var property in listproperties)
                    {
                        var item = new Properties()
                        {
                            ProductId = property.ProductId,
                            StoreId = newStore.Id,
                            Image = property.Image,
                            CostPrice = property.CostPrice,
                            Price = property.Price,
                            Name = property.Name,
                            Status = true,
                            Create_at = DateTime.Now,
                        };
                        _dataContext.Properties.Add(item);
                    }
                    await _dataContext.SaveChangesAsync();
                   
                    return new CustomResult()
                    {
                        Status = 200,
                        Message = "Create Success",
                        data = store

                    };

                }
                else
                {
                    store.Image = "null.jpg";
                    _dataContext.Stores.AddAsync(store);
                    await _dataContext.SaveChangesAsync();
                    var newStore = await _dataContext.Stores.SingleOrDefaultAsync(e => e.Address == store.Address && e.District == store.District && e.City == store.City);
                    var listproperties = await _dataContext.Product_Properties.ToListAsync();
                    foreach (var property in listproperties)
                    {
                        var item = new Properties()
                        {
                            ProductId = property.ProductId,
                            StoreId = newStore.Id,
                            Image = property.Image,
                            CostPrice = property.CostPrice,
                            Price = property.Price,
                            Name = property.Name,
                            Status = true,
                            Create_at = DateTime.Now,
                        };
                        _dataContext.Properties.Add(item);
                    }
                    await _dataContext.SaveChangesAsync();
                    return new CustomResult()
                    {
                        Status = 200,
                        Message = "Create Success",
                        data = store

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

        public async Task<CustomResult> DeleteStore(int id)
        {
            try
            {
                var data = await _dataContext.Stores.SingleOrDefaultAsync(e => e.Id == id);
                if (data != null)
                {
                    _dataContext.Stores.Remove(data);
                    await _dataContext.SaveChangesAsync();
                   
                    return new CustomResult()
                    {
                        Status = 200,
                        Message = "Delete Success",


                    };
                }
                else
                {
                   
                    return new CustomResult()
                    {
                        Status = 404,
                        Message = "Record Not Found!",


                    };
                }
               
            }
            catch(Exception ex) {
               
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error! " + ex.Message,


                };
            }
          
        }

        public async Task<CustomResult> GetAllStore()
        {
            try
            {
               var list = await _dataContext.Stores.Include("Admins").ToListAsync();
                
               foreach(var store in list)
                {
                    store.Image =  $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/StoreImage/{store.Image}";
                }  
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Get All Success",
                    data = list,

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

        public async Task<CustomResult> UpdateStore(Store store)
        {
            try
            {
                var dataOld = await _dataContext.Stores.SingleOrDefaultAsync(e => e.Id == store.Id);
                if (dataOld != null)
                {
                    if(dataOld.Address.ToLower()!= store.Address.ToLower() || dataOld.District != store.District || dataOld.City != store.City)
                    {
                        var data = await _dataContext.Stores.SingleOrDefaultAsync(e => e.Address.ToLower() == store.Address.ToLower() && e.District == store.District && e.City == store.City);
                        if (data != null)
                        {
                            return new CustomResult()
                            {
                                Status = 205,
                                Message = "Duplicate Address",


                            };
                        }
                    }
                    if (store.UploadImage != null)
                    {
                        var filename = GetUniqueFilename(store.UploadImage.FileName);
                        var upload = Path.Combine(_env.WebRootPath, "StoreImage");
                        var filePath = Path.Combine(upload, filename);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await store.UploadImage.CopyToAsync(stream);
                        }

                        dataOld.Image = filename;
                        dataOld.Address = store.Address;
                        dataOld.City = store.City;
                        dataOld.District = store.District;
                        dataOld.Update_at = DateTime.Now;
                        _dataContext.Stores.Update(dataOld);
                        await _dataContext.SaveChangesAsync();

                        return new CustomResult()
                        {
                            Status = 200,
                            Message = "Update Success",
                            data = dataOld,

                        };

                    }

                    else
                    {
                        dataOld.Address = store.Address;
                        dataOld.City = store.City;
                        dataOld.District = store.District;
                        dataOld.Update_at = DateTime.Now;
                        _dataContext.Stores.Update(dataOld);
                        await _dataContext.SaveChangesAsync();

                        return new CustomResult()
                        {
                            Status = 200,
                            Message = "Update Success",
                            data = dataOld,

                        };
                    }
                }
                else
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Update Fail Store Not Found!",
                        data = store,

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

        public async Task<CustomResult> ChangeStatus(int id)
        {
            try
            {
                var data = await _dataContext.Stores.SingleOrDefaultAsync(e => e.Id == id);
                if(data == null)
                {
                   
                    return new CustomResult()
                    {
                        Status = 404,
                        Message = "Record Not Found!"
                    };
                }
                else
                {
                    data.Status = !data.Status;
                    data.Update_at = DateTime.Now;
                    _dataContext.Stores.Update(data);
                    await _dataContext.SaveChangesAsync();
                   
                    return new CustomResult()
                    {
                        Status = 200,
                        Message = "Change Status Success",
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

        public async Task<CustomResult> GetById(int id)
        {
            try
            {
                var data = await _dataContext.Stores.SingleOrDefaultAsync(e=>e.Id == id);
                if (data == null)
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Store Not Found!"
                    };
                }
                else
                {
                    return new CustomResult()
                    {
                        Status = 200,
                        Message = "Get Store Success!",
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

        public async Task<CustomResult> Search(string? address, string? city, string? district, bool? status)
        {
            try {
                var query = _dataContext.Stores.AsQueryable();

                if (!string.IsNullOrEmpty(address))
                    query = query.Where(s => s.Address.Contains(address));

                if (!string.IsNullOrEmpty(city))
                    query = query.Where(s => s.City.Contains(city));

                if (!string.IsNullOrEmpty(district))
                    query = query.Where(s => s.District.Contains(district));

                if (status.HasValue)
                    query = query.Where(s => s.Status == status.Value);

                var result = await query.ToListAsync();
                foreach (var store in result)
                {
                    store.Image = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/StoreImage/{store.Image}";
                }

                return new CustomResult()
                {
                   Status = 200 ,
                   Message = "Search Success!",
                   data = result
                };
            }
            catch (Exception ex) {

                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error! " + ex.Message,


                };
            }
           
        }
    }
}
