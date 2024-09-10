using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.Wordprocessing;
using Irony.Parsing;
using Microsoft.EntityFrameworkCore;
using Project_sem3.InterFace;
using Project_sem3.Models;



namespace Project_sem3.Repositories
{
    public class ProductRepo : IProduct
    {
        private readonly dataContext _dataContext;
        private IWebHostEnvironment _env;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public ProductRepo(dataContext dataContext ,IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _env = env;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<CustomResult> ChangeStatus(string id)
        {
            try
            {
                var data = await _dataContext.Products.SingleOrDefaultAsync(e => e.ProductId == id);
                if (data != null)
                {
                    data.Status = !data.Status;
                   
                    _dataContext.Products.Update(data);
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

        public async Task<CustomResult> Create(Product e)
        {
            try
            {
                var data = await _dataContext.Products.SingleOrDefaultAsync(a=> a.Name.ToLower() == e.Name.ToLower()&& a.CategoryID == e.CategoryID && a.SubCategoryId==e.SubCategoryId && a.SegmentId==e.SegmentId && a.BrandId==e.BrandId && a.MadeIn==e.MadeIn);
                if (data != null)
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Duplicate Product Name!"
                    };
                }
               
                if (e.UploadImagesProduct != null)
                {
                    foreach(var item in e.UploadImagesProduct)
                    {
                        var filename = GetUniqueFilename(item.FileName);
                        var upload = Path.Combine(_env.WebRootPath, "ProductImage");
                        var filePath = Path.Combine(upload, filename);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await item.CopyToAsync(stream);
                        }
                        e.Image = e.Image +"; " + filename;
                    }
                   
                }
                else
                {
                    e.Image = "null.jpg";
                }
                e.Status = false;
                _dataContext.Products.Add(e);
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

        public async Task<CustomResult> GetAllTrue()
        {
            try
            {
                var list = await _dataContext.Products.Where(e=>e.Status==true).ToListAsync();

                foreach (var item in list)
                {
                   
                    if (item.Image.StartsWith("; ")){
                        item.Image = item.Image.Substring(2);
                    }
                  
                    string[] parts = item.Image.Split("; ");
                    item.Image = null;
                    foreach (var part in parts)
                    {
                        item.Image = item.Image +", " + $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/ProductImage/{part}";
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
        
        public async Task<CustomResult> GetAll()
        {
            try
            {
                var list = await _dataContext.Products.Include(e => e.Brand).Include(e => e.Category).Include(e => e.Subcategory).Include(e => e.Segment).Select(p => new ProductRes ()
                {
                    ProductId = p.ProductId,
                    BrandName = p.Brand.Name,
                    CategoryName = p.Category.Name,
                    SubcategoryName = p.Subcategory.Name,
                    SegmentName = p.Segment.Name,
                    CategoryID = p.CategoryID,
                    SubcategoryId = p.SubCategoryId,
                    SegmentId = p.SegmentId,
                    BrandId = p.BrandId,
                    Status = p.Status,
                    Name = p.Name,
                    Description = p.Description,
                    Image = p.Image,
                }).ToListAsync();

                foreach (var item in list)
                {

                    if (item.Image.StartsWith("; "))
                    {
                        item.Image = item.Image.Substring(2);
                    }

                    string[] parts = item.Image.Split("; ");
                    item.Image = null;
                    foreach (var part in parts)
                    {
                        item.Image = item.Image + ", " + $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/ProductImage/{part}";
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

        public async Task<CustomResult> GetById(string id)
        {
            try
            {
                var data = await _dataContext.Products.SingleOrDefaultAsync(e => e.ProductId == id);
                
                if (data == null)
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Record Not Found!",
                    };
                }
                
                string[] parts = data.Image.Split("; ");
                data.Image = "";
                foreach (var part in parts)
                {
                    data.Image = data.Image + ", " + $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/ProductImage/{part}";
                }
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

        public Task<CustomResult> Search(string? name, bool? status, int? brandId, int? categoryId, int? subcategoryId, int? segmentId)
        {
            throw new NotImplementedException();
        }

        public async Task<CustomResult> Update(Product e)
        {
            try
            {
                var dataOle = await _dataContext.Products.SingleOrDefaultAsync(a=>a.ProductId == e.ProductId);
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
                    var data = await _dataContext.Products.SingleOrDefaultAsync(a => a.Name.ToLower() == e.Name.ToLower() && a.CategoryID == e.CategoryID && a.SubCategoryId == e.SubCategoryId && a.SegmentId == e.SegmentId && a.BrandId == e.BrandId && a.MadeIn == e.MadeIn);
                    if (data != null)
                    {
                        return new CustomResult()
                        {
                            Status = 205,
                            Message = "Duplicate Product Name!"
                        };
                    }
                }
                if (e.UploadImagesProduct != null)
                {
                    foreach (var item in e.UploadImagesProduct)
                    {
                        var filename = GetUniqueFilename(item.FileName);
                        var upload = Path.Combine(_env.WebRootPath, "ProductImage");
                        var filePath = Path.Combine(upload, filename);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await item.CopyToAsync(stream);
                        }
                        e.Image = e.Image + "; " + filename;
                    }
                    dataOle.Image = e.Image;
                }
              
                dataOle.Name = e.Name;
                dataOle.Description = e.Description;
                dataOle.Expiry_date = e.Expiry_date;
                dataOle.Shelf_life = e.Shelf_life;
                dataOle.MadeIn = e.MadeIn;
                dataOle.Weight = e.Weight;
                dataOle.Volume = e.Volume;
                dataOle.CategoryID = e.CategoryID;
                dataOle.SegmentId = e.SegmentId;
                dataOle.SubCategoryId = e.SubCategoryId;
                dataOle.BrandId = e.BrandId;

                _dataContext.Products.Update(dataOle);
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

        public string GetUniqueFilename(string file)
        {
            file = Path.GetFileName(file);
            return Path.GetFileNameWithoutExtension(file) + "_" + DateTime.Now.Ticks + Path.GetExtension(file);
        }

        private class ProductRes
        {
            public string ProductId { get; set; }
            public  string BrandName { get; set; }
            public string CategoryName { get; set; }
            public string SubcategoryName { get; set; }

            public string SegmentName { get; set; }

            public int? CategoryID { get; set; }

            public int? SubcategoryId { get; set;}

            public int? SegmentId { get; set; }

            public int? BrandId { get; set; }

            public bool Status { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public string Image { get; set; }


        }
       
    }
}
