using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_sem3.InterFace;
using Project_sem3.Models;
using System.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Project_sem3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase, IHostedService
    {
        private readonly IHostApplicationLifetime _hostApplicationLifetime;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly IServiceProvider _serviceProvider;
      

        public NotificationController(IHostApplicationLifetime hostApplicationLifetime, IServiceProvider IServiceProvider)
        {
            _hostApplicationLifetime = hostApplicationLifetime;
            _serviceProvider = IServiceProvider;
            _cancellationTokenSource = new CancellationTokenSource();

        }
        //[HttpGet("check-and-notify/{hours}/{minute}")]
        //public async Task<ActionResult> CheckAndNotifyAt8am(int hours , int minute)
        //{
        //    _hours = hours;
        //    _minutes = minute;
          
           

        //      await AutoNotifyAt8am(_cancellationTokenSource.Token);

        //       return Ok("Thanh Cong");
        //}
     
        [NonAction]
        private async Task AutoNotifyAt8am(CancellationToken cancellationToken)
        {
           
            int _hour = DateTime.Now.Hour;
            int _minute = DateTime.Now.Minute;

          
            if (_hour > 0)
            {
              
                using (var scope = _serviceProvider.CreateScope())
                {
                    var date = DateTime.Now;
                    var dbContext = scope.ServiceProvider.GetRequiredService<dataContext>();
                    var dataFlashSale = await dbContext.Flash_Sales.Where(e => EF.Functions.DateDiffDay(date, e.End_Date) == 0).ToListAsync();
                    var listProperties = await dbContext.Properties.ToListAsync();
                    foreach (var property in listProperties)
                    {
                        foreach(var value in dataFlashSale) {
                            if(value.Id == property.FlashSaleId)
                            {
                                property.FlashSaleId = null;
                                dbContext.Properties.Update(property);
                              
                            }
                            value.Status = false;
                            dbContext.Flash_Sales.Update(value);
                        }
                    }
                    await dbContext.SaveChangesAsync();
                    //var data = await dbContext.Discounts.ToListAsync();
                    //    foreach (var item in data)
                    //{
                    //    item.Status = false;
                    //    dbContext.Discounts.Update(item);
                    //}
                    //   await dbContext.SaveChangesAsync();


                }
               
            }

            await Task.Delay(3600000, cancellationToken);
            await AutoNotifyAt8am(cancellationToken);
        }
        [NonAction]
        public Task StartAsync(CancellationToken cancellationToken)
        {

            // Chạy hàm AutoNotifyAt8am() khi ứng dụng khởi động
            AutoNotifyAt8am(_cancellationTokenSource.Token).ConfigureAwait(false);

            return Task.CompletedTask;
        }
        [NonAction]
        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // Huỷ CancellationTokenSource để dừng hàm AutoNotifyAt8am()
            _cancellationTokenSource.Cancel();
            await Task.Delay(Timeout.Infinite, cancellationToken);
        }
    }
}
