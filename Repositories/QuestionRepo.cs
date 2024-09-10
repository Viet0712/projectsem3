using Microsoft.EntityFrameworkCore;
using Project_sem3.InterFace;
using Project_sem3.Models;
using static System.Net.Mime.MediaTypeNames;

namespace Project_sem3.Repositories
{
    public class QuestionRepo : IQuestion
    {
        private readonly dataContext _dataContext;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public QuestionRepo(dataContext dataContext, IHttpContextAccessor httpContextAccessor)
        {
            _dataContext = dataContext;
            _httpContextAccessor = httpContextAccessor;

        }
        public async Task<CustomResult> GetAll()
        {
            try
            {
                var listProduct = await _dataContext.Products.ToListAsync();
                foreach (var item in listProduct)
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
                var list = await _dataContext.Questions.Include(e=>e.Products).Include(e=>e.Question_Replies).ThenInclude(e=>e.Admin).Include(e=>e.User).OrderByDescending(e=>e.Create_at).Select(e => new ListQuest()
                {
                    Id = e.Id,
                    ProductId = e.ProductId,
                    UserId = e.UserId,
                    Content = e.Content,
                    Like = e.Like,
                    Create_at = e.Create_at,
                    Update_at = e.Update_at,
                    UserName = e.User.FullName,
                    ProductName = e.Products.Name,
                    Question_RepliesContent = e.Question_Replies.Content,
                    EmailAdmin = e.Question_Replies.Admin.Email,
                    AdminName = e.Question_Replies.Admin.FullName,
                    RepAt = e.Question_Replies.Create_at,
                    Image = e.Products.Image,
                    Question_RepliesId = e.Question_Replies.Id,
                    Status = e.Status

                }).ToListAsync();
               

                foreach (var item in list)
                {

                   foreach(var item2 in listProduct)
                    {
                        if (item2.ProductId == item.ProductId)
                        {
                            item.Image=item2.Image;
                        }
                    }
                }
                //foreach (var item in list)
                //{
                //    foreach (var item2 in listProperties)
                //    {
                //        if (item2.Id == item.Order_Detail.PropertiesId)
                //        {
                //            item.Order_Detail.Properties.Image = item2.Image;
                //        }
                //    }
                //}
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Get Success!",
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
                var data = await _dataContext.Question_Replies.SingleOrDefaultAsync(e => e.Id == id);
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

        public async Task<CustomResult> SendFeedBack(Question_Reply r)
        {
            try
            {
                r.Status = true;
                r.Create_at = DateTime.Now;
                r.Update_at = DateTime.Now;
                r.Like = 0;
                _dataContext.Question_Replies.Add(r);
                await _dataContext.SaveChangesAsync();
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Send Success"
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

        public async Task<CustomResult> UpdateFeedBack(Question_Reply r)
        {
            try
            {
                var dataOld = await _dataContext.Question_Replies.SingleOrDefaultAsync(e => e.Id == r.Id);
                if (dataOld == null)
                {
                    return new CustomResult() { Status = 205, Message = "Record Not Found!" };
                }
                dataOld.Create_at = DateTime.Now;
                dataOld.Update_at = DateTime.Now;
                dataOld.AdminId = r.AdminId;
                dataOld.Content = r.Content;
                _dataContext.Question_Replies.Update(dataOld);
                await _dataContext.SaveChangesAsync();
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Update Success!"
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

        public class ListQuest
        {
            public int? Question_RepliesId { get; set; }
            public string Image { get; set; }
            public string ProductName { get; set; }
            public string UserName { get; set; }

            public int Id { get; set; }

            public string ProductId { get; set; }

            public int UserId { get; set; }

            public string Content { get; set; }

            public int Like { get; set; }

            public bool? Status { get; set; }
            public DateTime? Create_at { get; set; }

            public DateTime? Update_at { get; set; }

            public string? Question_RepliesContent { get; set; }

            public string? EmailAdmin { get; set; }

            public string? AdminName { get; set; }

            public DateTime ? RepAt { get; set; }


        }
    }
}
