using DocumentFormat.OpenXml.Bibliography;
using Microsoft.EntityFrameworkCore;
using Project_sem3.Models;
using System.Diagnostics;

namespace Project_sem3.Repositories
{
    public class RateRepo : InterFace.IRate
    {
        private readonly dataContext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public RateRepo(dataContext dataContext , IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;

        }
        public async Task<CustomResult> GetAll(int storeid)
        {
            try
            {
                var listProperties = await _dataContext.Properties.ToListAsync();
                var list = await _dataContext.Rates.Include(e=>e.Order_Detail).ThenInclude(e=>e.Properties).Include(e=>e.Rate_Replies).ThenInclude(e=>e.Admin).Include(e=>e.User).Include(e=>e.Products).Where(e=>e.Order_Detail.Properties.StoreId== storeid).OrderByDescending(x => x.Create_at).Select(e=>new RateRes()
                {
                    Id = e.Id,
                    ProductId = e.ProductId,
                    Order_detailId = e.Order_detailId,
                    UserId = e.UserId,
                    Rating = e.Rating,
                    Content = e.Content,
                    Status = e.Status,
                    Like = e.Like,
                    Create_at = e.Create_at,
                    Update_at = e.Update_at,
                    ProductName = e.Products.Name,
                    Type = e.Order_Detail.Properties.Name,
                    UseName = e.User.FullName,
                    RepAt = e.Rate_Replies.Create_at,
                    AdminEmail = e.Rate_Replies.Admin.Email,
                    AdminName = e.Rate_Replies.Admin.FullName,
                    Rep_Content = e.Rate_Replies.Content,
                    Rep_Id = e.Rate_Replies.Id,
                    Image = e.Order_Detail.Properties.Image,
                    PropertiesId = e.Order_Detail.Properties.Id,
                    OrderId = e.Order_Detail.OrederId
                }).ToListAsync();
           
                foreach (var item in listProperties)
                {
                 
                        item.Image = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/PropertiesImage/{item.Image}";
                      
                }foreach (var item in list)
                {
                    foreach(var item2 in listProperties) {
                        if(item2.Id == item.PropertiesId)
                        {
                            item.Image = item2.Image;
                        }
                    }
                }
              
                return new CustomResult()
                {
                    Status = 200,
                    Message="Get Success!",
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
                var data = await _dataContext.Rate_Replies.SingleOrDefaultAsync(e => e.Id == id);
                if (data == null)
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Record Not Found!",

                    };
                }
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Get Discount Success!",
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

        public async Task<CustomResult> SendFeedBack(Rate_Reply r)
        {
            try {
                r.Status = true;
                r.Create_at = DateTime.Now;
                r.Update_at = DateTime.Now;
                r.Like = 0;
                _dataContext.Rate_Replies.Add(r);
                await _dataContext.SaveChangesAsync();
                return new CustomResult() { 
                    Status = 200,
                    Message="Send Success"
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

        public async Task<CustomResult> UpdateFeedBack(Rate_Reply r)
        {
            try { 
                var dataOld = await _dataContext.Rate_Replies.SingleOrDefaultAsync(e=>e.Id == r.Id);
                if(dataOld == null)
                {
                    return new CustomResult() { Status = 205 , Message = "Record Not Found!"};
                }
                dataOld.Create_at= DateTime.Now;
                dataOld.Update_at= DateTime.Now;
                dataOld.AdminId = r.AdminId;
                dataOld.Content = r.Content;
                _dataContext.Rate_Replies.Update(dataOld);
                await _dataContext.SaveChangesAsync();
                return new CustomResult() { 
                    Status= 200,
                    Message = "Update Success!"
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

        public class RateRes
        {
            public string OrderId { get; set; }
            public int Id { get; set; }

            public string ProductId { get; set; }

            public int Order_detailId { get; set; }


            public int UserId { get; set; }

            public int? Rating { get; set; }

            public string? Content { get; set; }

            public bool? Status { get; set; }
            public int? Like { get; set; }

            public DateTime? Create_at { get; set; }

            public DateTime? Update_at { get; set; }

            public string ProductName { get; set; }

            public string Type { get; set; }

            public string UseName { get; set; }

            public DateTime? RepAt { get; set; }

            public string? AdminName { get; set; }

            public string? AdminEmail { get; set; }

            public string? Rep_Content { get; set; }

            public int? Rep_Id { get; set; }

            public string Image {  get; set; }

            public int PropertiesId { get; set; }
         
        }
    }
}
