using Microsoft.EntityFrameworkCore;
using Project_sem3.MobileInterface;
using Project_sem3.Models;

namespace Project_sem3.MobileRepo
{
    public class MbProductRepo : MbIProduct
    {
        private class ProductRes
        {
            public string ProductId { get; set; }
            public string BrandName { get; set; }
            public string CategoryName { get; set; }
            public string SubcategoryName { get; set; }

            public string SegmentName { get; set; }

            public int? CategoryID { get; set; }

            public int? SubcategoryId { get; set; }

            public int? SegmentId { get; set; }

            public int? BrandId { get; set; }

            public bool Status { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }

            public string Image { get; set; }


        }
        private dataContext _datacontext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public MbProductRepo(dataContext dataContext , IHttpContextAccessor IHttpContextAccessor)
        {
            _datacontext = dataContext;
            _httpContextAccessor = IHttpContextAccessor;
        }
        public async Task<CustomResult> GetAll()
        {
            try {
                var list = await _datacontext.Products.Include(e => e.Brand).Include(e => e.Category).Include(e => e.Subcategory).Include(e => e.Segment).Select(p => new ProductRes()
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
                return new CustomResult() { Status = 200 , Message = "Ok" , data = list };
            }
            catch ( Exception ex )
            {
                return new CustomResult() { Status = 400, Message = "Server error"};
            }
        


        }
    }
}
