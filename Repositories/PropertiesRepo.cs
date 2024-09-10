using Microsoft.EntityFrameworkCore;
using Project_sem3.InterFace;
using Project_sem3.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.Net.WebSockets;

namespace Project_sem3.Repositories
{
    public class PropertiesRepo : IProperties
    {
        private readonly dataContext _dataContext;
        private IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PropertiesRepo(dataContext dataContext, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<CustomResult> ChangeStatus(int id)
        {
            try
            {
                var data = await _dataContext.Properties.SingleOrDefaultAsync(e => e.Id == id);
                if (data != null)
                {
                    data.Status = !data.Status;

                    _dataContext.Properties.Update(data);
                    await _dataContext.SaveChangesAsync();
                    return new CustomResult()
                    {
                        Status = 200,
                        Message = "Change Status Success",

                    };
                }
                return new CustomResult()
                {
                    Status = 205,
                    Message = "Record Not Found!"
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

        public async Task<CustomResult> Create(Properties e)
        {
            try
            {
                
               var data = await _dataContext.Properties.SingleOrDefaultAsync(a=>a.ProductId == e.ProductId && a.Name.ToLower()==e.Name.ToLower()&&a.StoreId==e.StoreId);
                if (data!=null)
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Duplicate Properties!"
                    };
                }
                e.Status = false;
                e.Create_at = DateTime.Now;
                _dataContext.Properties.Add(e);
                await _dataContext.SaveChangesAsync();


                return new CustomResult()
                {

                    Status = 200,
                    Message = "Create Brand Success!",
                    data = e
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
      
        public async Task<CustomResult> GetAll(int id)
        {
            try
            {
                var listStock = await _dataContext.Goods.Include(e=>e.Properties).Where(e=>e.Properties.StoreId==id && e.Status==true).GroupBy(e=>e.PropertiesId).Select(g=>new { ProductId = g.Key, Stock = g.Sum(e => e.Stock) }).ToListAsync();
                var list = await _dataContext.Properties.Include(e => e.Product).ThenInclude(e => e.Brand).Include(e=>e.Store).Include(e=>e.Discount).Include(e=>e.Flash_Sale).Include(e=>e.Goods).Where(e=>e.StoreId==id) .Select(e=>new PropertiesRes()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Status = e.Status,
                    ProductId = e.ProductId,
                    StoreId = e.StoreId,
                    BrandId = e.Product.Brand.Id,
                    DiscountId = e.DiscountId,
                    FlashSaleId = e.FlashSaleId,
                    Image = e.Image,
                    CostPrice = e.CostPrice,
                    Price = e.Price,  
                    BrandName = e.Product.Brand.Name,
                    ProductName = e.Product.Name,
                    DiscountName = e.Discount.Name,
                    FlashSaleName=e.Flash_Sale.Name,
                  
                    Create_at = e.Create_at,
                    Update_at = e.Update_at,
                })
                    .ToListAsync();
                
                foreach (var item in list)
                {
                    item.Image = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/PropertiesImage/{item.Image}";
                    foreach (var item2 in listStock)
                    {
                        if (item.Id == item2.ProductId)
                        {
                            item.Stock = item2.Stock;
                        }
                    }
                }
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Get  Success!",
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

        public async Task<CustomResult> GetById(int id)
        {
            try
            {
                var data = await _dataContext.Product_Properties.Include(e=>e.Product).SingleOrDefaultAsync(e => e.Id == id);

                if (data == null)
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Record Not Found!",
                    };
                }
                data.Image = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/PropertiesImage/{data.Image}";
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Get  Success!",
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

        public Task<CustomResult> Search(string? name, bool? status, int? brandId, int? categoryId, int? subcategoryId, int? segmentId, int? storeId, bool? expiried)
        {
            throw new NotImplementedException();
        }

        //public async Task<CustomResult> Update(Properties e)
        //{
        //    try
        //    {
        //        var dataOle = await _dataContext.Properties.SingleOrDefaultAsync(a => a.Id == e.Id);
        //        if (dataOle == null)
        //        {
        //            return new CustomResult()
        //            {
        //                Status = 205,
        //                Message = "Record Not Found!",
        //            };
        //        }
               
        //        e.Update_at = DateTime.Now;
        //        dataOle.CostPrice = e.CostPrice;
              
        //        dataOle.ProductId = e.ProductId;
        //        dataOle.Price = e.Price;
               
        //        dataOle.Name = e.Name;
               
        //        _dataContext.Properties.Update(dataOle);
        //        await _dataContext.SaveChangesAsync();


        //        return new CustomResult()
        //        {

        //            Status = 200,
        //            Message = "Create Brand Success!",
        //            data = e
        //        };
        //    }

        //    catch (Exception ex)
        //    {


        //        return new CustomResult()
        //        {
        //            Status = 400,
        //            Message = "Server Error " + ex.Message,

        //        };
        //    }
        //}
        public string GetUniqueFilename(string file)
        {
            file = Path.GetFileName(file);
            return Path.GetFileNameWithoutExtension(file) + "_" + DateTime.Now.Ticks + Path.GetExtension(file);
        }

        public async Task<CustomResult> GetAll()
        {
            try
            {
                var list = await _dataContext.Product_Properties.Include(e => e.Product).ThenInclude(e => e.Brand).Select(e=> new PropertiesRes()
                {
                    Id = e.Id,
                    Name = e.Name,
                    Status = e.Status,
                    ProductId = e.ProductId,
                    Image = e.Image,
                    CostPrice = e.CostPrice,
                    Price = e.Price,
                    BrandId = e.Product.Brand.Id,
                    BrandName = e.Product.Brand.Name,
                    ProductName = e.Product.Name,
                    Create_at = e.Create_at,
                    Update_at = e.Update_at,
                }).ToListAsync();
                foreach (var item in list)
                {
                    item.Image = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/PropertiesImage/{item.Image}";
                }
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Get  Success!",
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
        public async Task<CustomResult> GetAllTrue()
        {
            try
            {
                var list = await _dataContext.Product_Properties.Include(e => e.Product).ThenInclude(e => e.Brand).Include(e => e.Product).ThenInclude(e => e.Category).Include(e => e.Product).ThenInclude(e => e.Subcategory).Include(e => e.Product).ThenInclude(e => e.Segment).Where(e=>e.Status==true)
                    .ToListAsync();
                foreach (var item in list)
                {
                    item.Image = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/PropertiesImage/{item.Image}";
                }
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Get  Success!",
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

        public async Task<CustomResult> Create(Product_Properties e)
        {
            try
            {
                var data = await _dataContext.Product_Properties.SingleOrDefaultAsync(a => a.ProductId == e.ProductId && a.Name.ToLower() == e.Name.ToLower());
                if (data != null)
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Duplicate Properties!"
                    };
                }
                if (e.UpLoadImage != null)
                {

                    var filename = GetUniqueFilename(e.UpLoadImage.FileName);
                    var upload = Path.Combine(_env.WebRootPath, "PropertiesImage");
                    var filePath = Path.Combine(upload, filename);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await e.UpLoadImage.CopyToAsync(stream);
                    }
                    e.Image = filename;


                }
                else
                {
                    e.Image = "null.jpg";
                }
                e.Status = true;
                e.Create_at = DateTime.Now;
                _dataContext.Product_Properties.Add(e);
                var listStore = await _dataContext.Stores.ToListAsync();
                foreach (var store in listStore)
                {
                   
                          var item = new Properties()
                        {
                            ProductId = e.ProductId,
                            StoreId = store.Id,
                            Image = e.Image,
                            CostPrice = e.CostPrice,
                            Price = e.Price,
                            Name = e.Name,
                            Status = true,
                            Create_at = DateTime.Now,
                        };
                    _dataContext.Properties.Add(item);
                
                }
                await _dataContext.SaveChangesAsync();


                return new CustomResult()
                {

                    Status = 200,
                    Message = "Create Brand Success!",
                    data = e
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

        public async Task<CustomResult> Update(Product_Properties e)
        {
            try
            {
                var dataOle = await _dataContext.Product_Properties.SingleOrDefaultAsync(a => a.Id == e.Id);
                var dataProperties = await _dataContext.Properties.Where(a => a.ProductId == dataOle.ProductId && a.Name == dataOle.Name).ToListAsync();
                if (dataOle == null)
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Record Not Found!",
                    };
                }
                if (dataOle.Name.ToLower() != e.Name.ToLower())
                {
                    var data = await _dataContext.Product_Properties.SingleOrDefaultAsync(a=>a.Name.ToLower()==e.Name.ToLower());   
                    if(data!=null)
                    {
                        return new CustomResult()
                        {
                            Status = 205,
                            Message = "Duplicate Properties!"
                        };
                    }
                }
                if (e.UpLoadImage != null)
                {

                    var filename = GetUniqueFilename(e.UpLoadImage.FileName);
                    var upload = Path.Combine(_env.WebRootPath, "PropertiesImage");
                    var filePath = Path.Combine(upload, filename);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await e.UpLoadImage.CopyToAsync(stream);
                    }
                    dataOle.Image = filename;


                }
                e.Update_at = DateTime.Now;
                dataOle.CostPrice = e.CostPrice;
                dataOle.Name = e.Name;
                dataOle.ProductId = e.ProductId;
                dataOle.Price = e.Price;
                foreach(var item in dataProperties)
                {
                    item.Price = e.Price;
                    item.CostPrice = e.CostPrice;
                    item.Image = dataOle.Image;
                    item.Name = e.Name;
                    item.ProductId = e.ProductId;
                    _dataContext.Update(item);
                }
                //dataOle.Name = e.Name;

                _dataContext.Product_Properties.Update(dataOle);
               
                await _dataContext.SaveChangesAsync();


                return new CustomResult()
                {

                    Status = 200,
                    Message = "Create Brand Success!",
                    data = e
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

        public async Task<CustomResult> ChangeStatusSadmin(int id)
        {
            try
            {
                var data = await _dataContext.Product_Properties.SingleOrDefaultAsync(e => e.Id == id);
                var listProperties = await _dataContext.Properties.Where(e=>e.ProductId==data.ProductId&& e.Name==data.Name).ToListAsync();
                if (data != null)
                {
                    if (data.Status == true)
                    {
                        data.Status = false;
                        foreach (var item in listProperties)
                        {
                            item.Status = false;
                        }
                    }
                    else
                    {
                        data.Status = true;
                        foreach (var item in listProperties)
                        {
                            item.Status = true;
                        }
                    }
                   
                    _dataContext.Product_Properties.Update(data);
                    await _dataContext.SaveChangesAsync();
                    return new CustomResult()
                    {
                        Status = 200,
                        Message = "Change Status Success",

                    };
                }
                return new CustomResult()
                {
                    Status = 205,
                    Message = "Record Not Found!"
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

        public async Task<CustomResult> GetByIdAdmin(int id)
        {
            try
            {
                var data = await _dataContext.Properties.Include(e => e.Product).SingleOrDefaultAsync(e => e.Id == id);

                if (data == null)
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Record Not Found!",
                    };
                }
                data.Image = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/PropertiesImage/{data.Image}";
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Get  Success!",
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

        private class PropertiesRes
        {
             public int? Stock {  get; set; }   
            public int? Id { get; set; }

            public string? ProductId { get; set; }

            public int? BrandId { get; set; }

            public int? StoreId { get; set; }

            public int? DiscountId { get; set; }

            public int? FlashSaleId { get; set; }

            public string? Image { get; set; }

            public float CostPrice { get; set; }



            public float? Price { get; set; }



            public string? Name { get; set; }

            public bool? Status { get; set; }

            public string? BrandName { get; set; }

            public string? ProductName { get; set; }

            public string? DiscountName { get; set; }

            public string? FlashSaleName { get; set; }

            public DateTime? Create_at { get; set; }

            public DateTime? Update_at { get; set; }



        }
    }
}
