using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.EntityFrameworkCore;
using Project_sem3.InterFace;
using Project_sem3.Model;
using Project_sem3.Models;
using Project_sem3.SendMail;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Project_sem3.Repositories
{
    public class OrderRepo : IOrder
    {
        private readonly dataContext _dataContext;
        private readonly IEmailService _emailService;
        public OrderRepo(dataContext dataContext, IEmailService emailService)

        {
            _dataContext = dataContext;
            _emailService = emailService;
        }
        public async Task<CustomResult> ChangeStatus(string id)
        {
            try
            {
                var data = await _dataContext.Orders.Include(e=>e.User).SingleOrDefaultAsync(e => e.IdOrder == id);
                if (data != null)
                {
                    if(data.Status == "packaged")
                    {
                        data.Status = "delivery";
                        MailRequest mailRequest = new MailRequest()
                        {
                            Subject = "Notification Order Status.",
                            UserName = data.User.FullName,
                            ToEmail = data.User.Email,
                            Body = $"<h4>Thank you for ordering at SwiftMart</h4>Your order has been delivered.<p>Please pay attention to tracking your order status.</p><h4>Your Order Id : {data.IdOrder}</h4><h4>Address : {data.Address}</h4><h4>Phone : {data.Phone}</h4>"

                        };
                        await _emailService.ConfirmOrder(mailRequest);
                    }
                    else
                    {
                        data.Status = "completed";
                        MailRequest mailRequest = new MailRequest()
                        {
                            Subject = "Notification Order Status.",
                            UserName = data.User.FullName,
                            ToEmail = data.User.Email,
                            Body = $"<h4>Thank you for ordering at SwiftMart</h4>Your order has been completed.<p>Thank you for trusting our service.</p><h4>Your Order Id : {data.IdOrder}</h4><h4>Address : {data.Address}</h4><h4>Phone : {data.Phone}</h4>"

                        };
                        await _emailService.ConfirmOrder(mailRequest);
                    }

                    _dataContext.Orders.Update(data);
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

        public async Task<CustomResult> GetByStore(int storeId,DateTime month)
        {
            try {
              
               
                var list = await _dataContext.Orders.Where(e=>e.StoreId==storeId ).Where(e=> EF.Functions.DateDiffMonth(month, e.Create_at) == 0).Include(e=>e.User).OrderByDescending(e=>e.Create_at).ToListAsync();
                return new CustomResult() {Status=200,Message="Get Success!",data = list };
            }
            catch (Exception ex) {
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error! " + ex.Message,


                };
            }
        }

        public async Task<CustomResult> OrderDetail(string OrderId)
        {
            try { 
                var data = await _dataContext.Order_Details.Include(e=>e.Properties).ThenInclude(e=>e.Store).Include(e=>e.Properties).ThenInclude(e=>e.Product).ThenInclude(e=>e.Brand).Where(e=>e.OrederId==OrderId).ToListAsync();
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Success!",
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
    }
}
