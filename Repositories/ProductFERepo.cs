using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Project_sem3.InterFace;
using Project_sem3.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Project_sem3.Repositories
{
    public class ProductFERepo : IProductFE
    {
        private readonly dataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProductFERepo(dataContext context , IHttpContextAccessor httpContextAccessor )
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

     
        public async Task<IEnumerable<Product>> GetAllProducts(int storeid)
        {
            try
            {
                var currentDate = DateTime.Now; // Lấy ngày hiện tại
                var products = await _context.Products
                    .Include(p => p.Properties)
                        .ThenInclude(prop => prop.Discount)
                    .Include(p => p.Properties)
                        .ThenInclude(prop => prop.Flash_Sale)
                    .Include(p => p.Properties)
                        .ThenInclude(prop => prop.OrderDetails)
                     .Include(p => p.Properties)
                        .ThenInclude(prop => prop.Goods)
                    .Include(p => p.Rates)
                    .Include(p => p.Category) // Include Category
    .Include(p => p.Segment) // Include Segment
    .Include(p => p.Subcategory) // Include Subcategory
    .Include(p => p.Brand)

                    .Where(p=>p.Status ==true)
                    .Select(p => new Product
                    {
                        ProductId = p.ProductId,
                        Name = p.Name,
                        Image = p.Image,
                        Category = p.Category != null ? new Category
                        { 
                        Name = p.Category.Name
                        }: null,
                        Subcategory = p.Subcategory != null ? new Subcategory
                        {
                            Name = p.Subcategory.Name
                        } : null,
                        Segment = p.Segment != null ? new Segment
                        {
                            Name = p.Segment.Name
                        } : null,
                        Brand = p.Brand != null ? new Brand
                        {
                            Id = p.Brand.Id,
                            Name = p.Brand.Name,
                            Image = p.Brand.Image,
                            Status = p.Brand.Status
                        } : null,
                        Properties = p.Properties
                           .Where(prop => prop.StoreId == storeid && prop.Status == true)
                      
                            .Select(prop => new Properties
                            {
                                Id = prop.Id,
                                Name = prop.Name,
                                Price = prop.Price,
                                StoreId = prop.StoreId,
                                DiscountId = prop.DiscountId,
                                FlashSaleId = prop.FlashSaleId,
                                Image = prop.Image,
                                Status = prop.Status,
                                Discount = prop.Discount != null && prop.Discount.Status && prop.Discount.Start_date <= currentDate && prop.Discount.End_date >= currentDate ? new Discount
                                {
                                    Id = prop.Discount.Id,
                                    Name = prop.Discount.Name,
                                    Volume = prop.Discount.Volume,
                                    Status = prop.Discount.Status,
                                    Start_date = prop.Discount.Start_date,
                                    End_date = prop.Discount.End_date
                                } : null,
                                Flash_Sale = prop.Flash_Sale != null && prop.Flash_Sale.Status && prop.Flash_Sale.Start_Date <= currentDate && prop.Flash_Sale.End_Date >= currentDate ? new Flash_Sale
                                {
                                    Id = prop.Flash_Sale.Id,
                                    Name = prop.Flash_Sale.Name,
                                    Volume = prop.Flash_Sale.Volume,
                                    End_Date = prop.Flash_Sale.End_Date,
                                    Start_Date = prop.Flash_Sale.Start_Date,
                                    Status = prop.Flash_Sale.Status
                                } : null,
                                OrderDetails = prop.OrderDetails.Select(or => new Order_Detail
                                {
                                    Id = or.Id,
                                    Quantity = or.Quantity
                                }).ToList(),
                                Goods = prop.Goods.Select(or => new Goods
                                {
                                    Id = or.Id,
                                    Arrival_date = or.Arrival_date,
                                    Expiry_date = or.Expiry_date,
                                    Stock = or.Stock
                                }).ToList(),
                            }).ToList(),
                        Rates = p.Rates.Select(rate => new Rate
                        {
                            Id = rate.Id,
                            Rating = rate.Rating,
                        }).ToList(),
                    })
                    .Where(p => p.Properties.Any()) // Lọc ra các sản phẩm có ít nhất một bản ghi trong Properties
                    .ToListAsync();
                foreach (var product in products)
                {

                        if (product.Image.StartsWith("; "))
                        {
                            product.Image = product.Image.Substring(2);
                        }

                        string[] parts = product.Image.Split("; ");
                            product.Image = null;
                        foreach (var part in parts)
                        {
                            product.Image = product.Image + ", " + $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/ProductImage/{part}";
                        }     
                        foreach(var iteam in product.Properties)
                    {
                            iteam.Image = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/PropertiesImage/{iteam.Image}";
                    }

                }

                return products;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Product>> GetFlashSale(int storeid)
        {
            try
            {
                var currentDate = DateTime.Now;

                var productsInFlashSale = await _context.Products
                     .Include(p => p.Properties)
                    .ThenInclude(prop => prop.Discount)
                    .Include(p => p.Properties)
                        .ThenInclude(prop => prop.Flash_Sale)
                    .Include(p => p.Properties)
                        .ThenInclude(prop => prop.OrderDetails)
                             .Include(p => p.Properties)
                        .ThenInclude(prop => prop.Goods)

                    .Include(p => p.Rates)
                      .Where(p => p.Status == true)
                    .Where(p => p.Properties.Any(prop => prop.Flash_Sale != null &&
                                                         prop.StoreId == storeid &&
                                                         prop.Status == true &&
                                                         prop.Flash_Sale.Start_Date <= currentDate &&
                                                         prop.Flash_Sale.End_Date >= currentDate &&
                                                         prop.Flash_Sale.Status == true))
                    .Select(p => new Product
                    {
                        ProductId = p.ProductId,
                        Name = p.Name,
                        Image = p.Image,
                        Status = p.Status,
                       
                        Properties = p.Properties
                         .Where(prop => prop.StoreId == storeid && prop.Status == true)
                        .Select(prop => new Properties
                        {
                            Id = prop.Id,
                            Name = prop.Name,
                            Price = prop.Price,
                            StoreId = prop.StoreId,
                            DiscountId = prop.DiscountId,
                            FlashSaleId = prop.FlashSaleId,
                         
                            Image = prop.Image,
                            Status = prop.Status,
                            Flash_Sale = prop.Flash_Sale != null ? new Flash_Sale
                            {
                                Id = prop.Flash_Sale.Id,
                                Name = prop.Flash_Sale.Name,
                                Volume = prop.Flash_Sale.Volume,
                                End_Date = prop.Flash_Sale.End_Date,
                                Start_Date = prop.Flash_Sale.Start_Date,
                                Status = prop.Flash_Sale.Status
                            } : null,
                            Discount = prop.Discount != null ? new Discount
                            {
                                Id = prop.Discount.Id,
                                Name = prop.Discount.Name,
                                Volume = prop.Discount.Volume,
                                Status = prop.Discount.Status,
                                Start_date = prop.Discount.Start_date,
                                End_date = prop.Discount.End_date
                            } : null,
                            OrderDetails = prop.OrderDetails.Select(od => new Order_Detail
                            {
                                Id = od.Id,
                                Quantity = od.Quantity

                            }).ToList(),
                            Goods = prop.Goods.Select(or => new Goods
                            {
                                Id = or.Id,
                                Arrival_date = or.Arrival_date,
                                Expiry_date = or.Expiry_date,
                                Stock = or.Stock
                            }).ToList(),
                        }).ToList(),
                        Rates = p.Rates.Select(rate => new Rate
                        {
                            Id = rate.Id,
                            Rating = rate.Rating,

                        }).ToList(),
                    })
                     .Where(p => p.Properties.Any()) // Lọc ra các sản phẩm có ít nhất một bản ghi trong Properties
                    .ToListAsync();


                foreach (var product in productsInFlashSale)
                {

                    if (product.Image.StartsWith("; "))
                    {
                        product.Image = product.Image.Substring(2);
                    }

                    string[] parts = product.Image.Split("; ");
                    product.Image = null;
                    foreach (var part in parts)
                    {
                        product.Image = product.Image + ", " + $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/ProductImage/{part}";
                    }
                    foreach (var iteam in product.Properties)
                    {
                        iteam.Image = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/PropertiesImage/{iteam.Image}";
                    }

                }

                return productsInFlashSale;
            }catch(Exception ex) {
                return null;
            }
        }

        public async Task<Product> GetProductsById(int storeid,string id){
            try
            {
                var currentDate = DateTime.Now;
                var product = await _context.Products
                   .Include(p => p.Properties)
                       .ThenInclude(prop => prop.Discount)
                   .Include(p => p.Properties)
                       .ThenInclude(prop => prop.Flash_Sale)
                   .Include(p => p.Properties)
                       .ThenInclude(prop => prop.OrderDetails)
                        .Include(p => p.Properties)
                       .ThenInclude(prop => prop.Goods)
                   .Include(p => p.Rates)
                   .ThenInclude(p=>p.Order_Detail)
                    .Include(p => p.Rates)
                   .ThenInclude(p => p.Rate_Replies)
                     .Include(p => p.Questions)
                     .ThenInclude(p=>p.Question_Replies)
                         .Include(p => p.Questions)
                     .ThenInclude(prop => prop.User)
                    .Include(p => p.Category)
                     .Include(p => p.Subcategory)
                      .Include(p => p.Segment)
                       .Include(p => p.Brand)
                     
                   .Where(p => p.ProductId == id && p.Status == true)
                   .Select(p => new Product
                   {
                       ProductId = p.ProductId,
                       Name = p.Name,
                       Image = p.Image,
                       Description = p.Description,
                       MadeIn = p.MadeIn,
                       Weight = p.Weight,
                       Volume = p.Volume,
                       Shelf_life = p.Shelf_life,
                       Expiry_date = p.Expiry_date,


                       Properties = p.Properties
                       .Where(prop => prop.StoreId == storeid && prop.Status == true)
                       .Select(prop => new Properties
                       {
                           Id = prop.Id,
                           Name = prop.Name,
                           Price = prop.Price,
                           StoreId = prop.StoreId,
                           DiscountId = prop.DiscountId,
                           FlashSaleId = prop.FlashSaleId,
                        
                           Image = prop.Image,
                           Status = prop.Status,
                           Discount = prop.Discount != null && prop.Discount.Status && prop.Discount.Start_date <= currentDate && prop.Discount.End_date >= currentDate ? new Discount
                           {
                               Id = prop.Discount.Id,
                               Name = prop.Discount.Name,
                               Volume = prop.Discount.Volume,
                               Status = prop.Discount.Status,
                               Start_date = prop.Discount.Start_date,
                               End_date = prop.Discount.End_date
                           } : null,
                           Flash_Sale = prop.Flash_Sale != null && prop.Flash_Sale.Status && prop.Flash_Sale.Start_Date <= currentDate && prop.Flash_Sale.End_Date >= currentDate ? new Flash_Sale
                           {
                               Id = prop.Flash_Sale.Id,
                               Name = prop.Flash_Sale.Name,
                               Volume = prop.Flash_Sale.Volume,
                               End_Date = prop.Flash_Sale.End_Date,
                               Start_Date = prop.Flash_Sale.Start_Date,
                               Status = prop.Flash_Sale.Status
                           } : null,
                           OrderDetails = prop.OrderDetails.Select(or => new Order_Detail
                           {
                               Id = or.Id,
                               Quantity = or.Quantity
                           }).ToList(),
                           Goods = prop.Goods.Select(or => new Goods
                           {
                               Id = or.Id,
                               Arrival_date = or.Arrival_date,
                               Expiry_date = or.Expiry_date,
                               Stock = or.Stock
                           }).ToList(),
                       }).ToList(),
                       Rates = p.Rates.Select(rate => new Rate
                       {
                           Id = rate.Id,
                           Rating = rate.Rating,
                           Content = rate.Content,
                           Like = rate.Like,
                           Create_at = rate.Create_at,
                           User = rate.User != null ? new User
                           {
                               Id = rate.User.Id,
                               FullName = rate.User.FullName,
                               Image = rate.User.Image,
                               Status = rate.User.Status,
                               Role = rate.User.Role
                           } : null,
                           Order_Detail = rate.Order_Detail != null? new Order_Detail
                           {
                               Id = rate.Order_Detail.Id,
                               Properties = rate.Order_Detail.Properties != null? new Properties
                               {
                                   Name = rate.Order_Detail.Properties.Name
                               }:null,
                           }: null,
                           Rate_Replies = rate.Rate_Replies != null ? new Rate_Reply
                           {
                               Id = rate.Rate_Replies.Id,
                               Content = rate.Rate_Replies.Content,
                               Create_at = rate.Rate_Replies.Create_at,
                               Like = rate.Rate_Replies.Like,
                           } : null
                       }).ToList(),
                       Category = p.Category != null ? new Category
                       {
                           Id = p.Category.Id,
                           Name = p.Category.Name,
                           CodeCategory = p.Category.CodeCategory,
                           Status = p.Category.Status
                       } : null,
                       Subcategory = p.Subcategory != null ? new Subcategory
                       {
                           Id = p.Subcategory.Id,
                           Name = p.Subcategory.Name,
                           CodeSubcategory = p.Subcategory.CodeSubcategory,
                           Status = p.Subcategory.Status,
                           CategoryId = p.Subcategory.CategoryId
                       } : null,
                       Segment = p.Segment != null ? new Segment
                       {
                           Id = p.Segment.Id,
                           Name = p.Segment.Name,
                           SubCategoryId = p.Segment.SubCategoryId,
                           Status = p.Segment.Status,
                           CodeSegment = p.Segment.CodeSegment
                       } : null,
                       Brand = p.Brand != null ? new Brand
                       {
                           Id = p.Brand.Id,
                           Name = p.Brand.Name,
                           Image = p.Brand.Image,
                           Status = p.Brand.Status
                       } : null,
                       Questions = p.Questions.Select(qes => new Question
                       {
                           Id = qes.Id,
                           Like = qes.Like,
                           Content = qes.Content,
                           Create_at = qes.Create_at,
                           User = qes.User != null ? new User
                           {
                               Id = qes.User.Id,
                               FullName = qes.User.FullName,
                               Image = qes.User.Image,
                               Status = qes.User.Status,
                               Role = qes.User.Role
                           } : null,
                           Question_Replies = qes.Question_Replies != null ? new Question_Reply
                           {
                               Content = qes.Question_Replies.Content,
                               Like = qes.Question_Replies.Like,
                               Create_at = qes.Question_Replies.Create_at
                           }:null
                       }).ToList(),


                   })
                    .Where(p => p.Properties.Any()) // Lọc ra các sản phẩm có ít nhất một bản ghi trong Properties
                   .FirstOrDefaultAsync();

                    if (product.Image.StartsWith("; "))
                    {
                        product.Image = product.Image.Substring(2);
                    }

                    string[] parts = product.Image.Split("; ");
                    product.Image = null;
                    foreach (var part in parts)
                    {
                        product.Image = product.Image + ", " + $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/ProductImage/{part}";
                    }
                    foreach (var iteam in product.Properties)
                    {
                        iteam.Image = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/PropertiesImage/{iteam.Image}";
                    }

                
                return product;
            }
            catch (Exception ex)
            {
                return null;
            }

              

               
            }




        public async Task<IEnumerable<Product>> GetProductsCategory(int storeid,string grant, int id)
        {
            try
            {
                var currentDate = DateTime.Now;
                if (grant == "category")
                {
                    var list = await _context.Products
                   .Include(p => p.Category)
                   .Include(p => p.Subcategory)
                        .Include(p => p.Segment)
                   .Include(p => p.Properties)
                       .ThenInclude(prop => prop.Discount)
                           .Include(p => p.Properties)
                       .ThenInclude(prop => prop.Goods)
                   .Include(p => p.Properties)
                       .ThenInclude(prop => prop.Flash_Sale)
                   .Include(p => p.Properties)
                       .ThenInclude(prop => prop.OrderDetails)
                   .Include(p => p.Rates)
                      .Include(p => p.Brand)
                   .Where(p => p.CategoryID == id && p.Status == true)
                   .Select(p => new Product
                   {
                       ProductId = p.ProductId,
                       Name = p.Name,
                       Image = p.Image,
                      
                       Category = p.Category != null ? new Category
                       {
                           Id = p.Category.Id,
                           Name =  p.Category.Name

                       }:null,
                       Subcategory = p.Subcategory != null ? new Subcategory
                       {
                           Id = p.Subcategory.Id,
                           Name = p.Subcategory.Name

                       } : null,
                       Segment = p.Segment != null ? new Segment
                       {
                           Id = p.Segment.Id,
                           Name = p.Segment.Name

                       } : null,
                       Brand = p.Brand != null ? new Brand
                       {
                           Id = p.Brand.Id,
                           Name = p.Brand.Name,
                           Image = p.Brand.Image,
                           Status = p.Brand.Status
                       } : null,
                       Properties = p.Properties
                          .Where(prop => prop.StoreId == storeid && prop.Status == true)
                           .Select(prop => new Properties
                           {
                               Id = prop.Id,
                               Name = prop.Name,
                               Price = prop.Price,
                               StoreId = prop.StoreId,
                               DiscountId = prop.DiscountId,
                               FlashSaleId = prop.FlashSaleId,
                            
                               Image = prop.Image,
                               Status = prop.Status,
                               Discount = prop.Discount != null && prop.Discount.Status && prop.Discount.Start_date <= currentDate && prop.Discount.End_date >= currentDate ? new Discount
                               {
                                   Id = prop.Discount.Id,
                                   Name = prop.Discount.Name,
                                   Volume = prop.Discount.Volume,
                                   Status = prop.Discount.Status,
                                   Start_date = prop.Discount.Start_date,
                                   End_date = prop.Discount.End_date
                               } : null,
                               Flash_Sale = prop.Flash_Sale != null && prop.Flash_Sale.Status && prop.Flash_Sale.Start_Date <= currentDate && prop.Flash_Sale.End_Date >= currentDate ? new Flash_Sale
                               {
                                   Id = prop.Flash_Sale.Id,
                                   Name = prop.Flash_Sale.Name,
                                   Volume = prop.Flash_Sale.Volume,
                                   End_Date = prop.Flash_Sale.End_Date,
                                   Start_Date = prop.Flash_Sale.Start_Date,
                                   Status = prop.Flash_Sale.Status
                               } : null,
                               OrderDetails = prop.OrderDetails.Select(or => new Order_Detail
                               {
                                   Id = or.Id,
                                   Quantity = or.Quantity
                               }).ToList(),
                               Goods = prop.Goods.Select(or => new Goods
                               {
                                   Id = or.Id,
                                   Arrival_date = or.Arrival_date,
                                   Expiry_date = or.Expiry_date,
                                   Stock = or.Stock
                               }).ToList(),
                           }).ToList(),
                       Rates = p.Rates.Select(rate => new Rate
                       {
                           Id = rate.Id,
                           Rating = rate.Rating,
                       }).ToList(),
                   })
                   .Where(p => p.Properties.Any()) // Lọc ra các sản phẩm có ít nhất một bản ghi trong Properties
                   .ToListAsync();

                    foreach (var product in list)
                    {

                        if (product.Image.StartsWith("; "))
                        {
                            product.Image = product.Image.Substring(2);
                        }

                        string[] parts = product.Image.Split("; ");
                        product.Image = null;
                        foreach (var part in parts)
                        {
                            product.Image = product.Image + ", " + $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/ProductImage/{part}";
                        }
                        foreach (var iteam in product.Properties)
                        {
                            iteam.Image = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/PropertiesImage/{iteam.Image}";
                        }

                    }
                    return list;
                }
                else if(grant == "subcategory")
                {
                    var list = await _context.Products
                        .Include(p=>p.Category)
                       .Include(p => p.Subcategory)
                        .Include(p => p.Segment)
                           .Include(p => p.Brand)
                   .Include(p => p.Properties)
                       .ThenInclude(prop => prop.Discount)
                   .Include(p => p.Properties)
                       .ThenInclude(prop => prop.Flash_Sale)
                   .Include(p => p.Properties)
                       .ThenInclude(prop => prop.OrderDetails)
                         .Include(p => p.Properties)
                       .ThenInclude(prop => prop.Goods)
                   .Include(p => p.Rates)
                   .Where(p => p.SubCategoryId == id && p.Status == true)
                   .Select(p => new Product
                   {
                       ProductId = p.ProductId,
                       Name = p.Name,
                       Image = p.Image,
                       Category = p.Category != null ? new Category
                       {
                           Id = p.Category.Id,
                           Name = p.Category.Name

                       } : null,
                       Subcategory = p.Subcategory != null ? new Subcategory
                       {
                           Id = p.Subcategory.Id,
                           Name = p.Subcategory.Name

                       } : null,
                       Segment = p.Segment != null ? new Segment
                       {
                           Id = p.Segment.Id,
                           Name = p.Segment.Name

                       } : null,
                       Brand = p.Brand != null ? new Brand
                       {
                           Id = p.Brand.Id,
                           Name = p.Brand.Name,
                           Image = p.Brand.Image,
                           Status = p.Brand.Status
                       } : null,
                       Properties = p.Properties
                            .Where(prop => prop.StoreId == storeid && prop.Status == true)
                           .Select(prop => new Properties
                           {
                               Id = prop.Id,
                               Name = prop.Name,
                               Price = prop.Price,
                               StoreId = prop.StoreId,
                               DiscountId = prop.DiscountId,
                               FlashSaleId = prop.FlashSaleId,
                           
                               Image = prop.Image,
                               Status = prop.Status,
                               Discount = prop.Discount != null && prop.Discount.Status && prop.Discount.Start_date <= currentDate && prop.Discount.End_date >= currentDate ? new Discount
                               {
                                   Id = prop.Discount.Id,
                                   Name = prop.Discount.Name,
                                   Volume = prop.Discount.Volume,
                                   Status = prop.Discount.Status,
                                   Start_date = prop.Discount.Start_date,
                                   End_date = prop.Discount.End_date
                               } : null,
                               Flash_Sale = prop.Flash_Sale != null && prop.Flash_Sale.Status && prop.Flash_Sale.Start_Date <= currentDate && prop.Flash_Sale.End_Date >= currentDate ? new Flash_Sale
                               {
                                   Id = prop.Flash_Sale.Id,
                                   Name = prop.Flash_Sale.Name,
                                   Volume = prop.Flash_Sale.Volume,
                                   End_Date = prop.Flash_Sale.End_Date,
                                   Start_Date = prop.Flash_Sale.Start_Date,
                                   Status = prop.Flash_Sale.Status
                               } : null,
                               OrderDetails = prop.OrderDetails.Select(or => new Order_Detail
                               {
                                   Id = or.Id,
                                   Quantity = or.Quantity
                               }).ToList(),
                               Goods = prop.Goods.Select(or => new Goods
                               {
                                   Id = or.Id,
                                   Arrival_date = or.Arrival_date,
                                   Expiry_date = or.Expiry_date,
                                   Stock = or.Stock
                               }).ToList(),
                           }).ToList(),
                       Rates = p.Rates.Select(rate => new Rate
                       {
                           Id = rate.Id,
                           Rating = rate.Rating,
                       }).ToList(),
                   })
                   .Where(p => p.Properties.Any()) // Lọc ra các sản phẩm có ít nhất một bản ghi trong Properties
                   .ToListAsync();
                    foreach (var product in list)
                    {

                        if (product.Image.StartsWith("; "))
                        {
                            product.Image = product.Image.Substring(2);
                        }

                        string[] parts = product.Image.Split("; ");
                        product.Image = null;
                        foreach (var part in parts)
                        {
                            product.Image = product.Image + ", " + $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/ProductImage/{part}";
                        }
                        foreach (var iteam in product.Properties)
                        {
                            iteam.Image = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/PropertiesImage/{iteam.Image}";
                        }

                    }
                    return list;
                }
                else if (grant == "segment")
                {
                    var list = await _context.Products
                         .Include(p => p.Category)
                        .Include(p => p.Subcategory)
                        .Include(p=>p.Segment)
                           .Include(p => p.Brand)
                    .Include(p => p.Properties)
                        .ThenInclude(prop => prop.Discount)
                    .Include(p => p.Properties)
                        .ThenInclude(prop => prop.Flash_Sale)
                           .Include(p => p.Properties)
                        .ThenInclude(prop => prop.Goods)
                    .Include(p => p.Properties)
                        .ThenInclude(prop => prop.OrderDetails)
                    .Include(p => p.Rates)
                    .Where(p => p.SegmentId == id && p.Status == true)
                    .Select(p => new Product
                    {
                        ProductId = p.ProductId,
                        Name = p.Name,
                        Image = p.Image,
                        Category = p.Category != null ? new Category
                        {
                            Id = p.Category.Id,
                            Name = p.Category.Name

                        } : null,
                        Subcategory = p.Subcategory != null ? new Subcategory
                        {
                            Id = p.Subcategory.Id,
                            Name = p.Subcategory.Name

                        } : null,
                        Segment = p.Segment != null ? new Segment
                        {
                            Id = p.Segment.Id,
                            Name = p.Segment.Name

                        } : null,
                        Brand = p.Brand != null ? new Brand
                        {
                            Id = p.Brand.Id,
                            Name = p.Brand.Name,
                            Image = p.Brand.Image,
                            Status = p.Brand.Status
                        } : null,

                        Properties = p.Properties
                             .Where(prop => prop.StoreId == storeid && prop.Status == true)
                            .Select(prop => new Properties
                            {
                                Id = prop.Id,
                                Name = prop.Name,
                                Price = prop.Price,
                                StoreId = prop.StoreId,
                                DiscountId = prop.DiscountId,
                                FlashSaleId = prop.FlashSaleId,
                               
                                Image = prop.Image,
                                Status = prop.Status,
                                Discount = prop.Discount != null && prop.Discount.Status && prop.Discount.Start_date <= currentDate && prop.Discount.End_date >= currentDate ? new Discount
                                {
                                    Id = prop.Discount.Id,
                                    Name = prop.Discount.Name,
                                    Volume = prop.Discount.Volume,
                                    Status = prop.Discount.Status,
                                    Start_date = prop.Discount.Start_date,
                                    End_date = prop.Discount.End_date
                                } : null,
                                Flash_Sale = prop.Flash_Sale != null && prop.Flash_Sale.Status && prop.Flash_Sale.Start_Date <= currentDate && prop.Flash_Sale.End_Date >= currentDate ? new Flash_Sale
                                {
                                    Id = prop.Flash_Sale.Id,
                                    Name = prop.Flash_Sale.Name,
                                    Volume = prop.Flash_Sale.Volume,
                                    End_Date = prop.Flash_Sale.End_Date,
                                    Start_Date = prop.Flash_Sale.Start_Date,
                                    Status = prop.Flash_Sale.Status
                                } : null,
                                OrderDetails = prop.OrderDetails.Select(or => new Order_Detail
                                {
                                    Id = or.Id,
                                    Quantity = or.Quantity
                                }).ToList(),
                                Goods = prop.Goods.Select(or => new Goods
                                {
                                    Id = or.Id,
                                    Arrival_date = or.Arrival_date,
                                    Expiry_date = or.Expiry_date,
                                    Stock = or.Stock
                                }).ToList(),
                            }).ToList(),
                        Rates = p.Rates.Select(rate => new Rate
                        {
                            Id = rate.Id,
                            Rating = rate.Rating,
                        }).ToList(),
                    })
                    .Where(p => p.Properties.Any()) // Lọc ra các sản phẩm có ít nhất một bản ghi trong Properties
                    .ToListAsync();
                    foreach (var product in list)
                    {

                        if (product.Image.StartsWith("; "))
                        {
                            product.Image = product.Image.Substring(2);
                        }

                        string[] parts = product.Image.Split("; ");
                        product.Image = null;
                        foreach (var part in parts)
                        {
                            product.Image = product.Image + ", " + $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/ProductImage/{part}";
                        }
                        foreach (var iteam in product.Properties)
                        {
                            iteam.Image = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/PropertiesImage/{iteam.Image}";
                        }

                    }
                    return list;
                }
                else
                {
                    return null;
                }

               

            }catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Product>> GetProductSearch()
        {
            try
            {
                var list = await _context.Products.Include(p => p.Properties)
                        .ThenInclude(prop => prop.Discount)
                    .Include(p => p.Properties)
                        .ThenInclude(prop => prop.Flash_Sale)
                    .ToListAsync();
                return list;
            }catch (Exception ex)
            {
                return null;
            }
        }
    }




    
}
