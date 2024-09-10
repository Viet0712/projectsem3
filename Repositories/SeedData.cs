using MailKit.Search;
using Microsoft.EntityFrameworkCore;
using Project_sem3.InterFace;
using Project_sem3.Models;

namespace Project_sem3.Repositories
{
    public static class SeedData
    {
      
        public static async Task<string> SeedDataFuction(IServiceProvider serviceProvider,int year , int month ,int day , int totalRecord)
        {
            try
            {
                using (var scope = serviceProvider.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<dataContext>();
                    // Kiểm tra nếu có dữ liệu rồi thì không thêm nữa


                    // Tạo danh sách 1000 bản ghi
                    var listStore = await dbContext.Stores.ToListAsync();
                    var listUser = await dbContext.Users.ToListAsync();
                    var listProperties = await dbContext.Properties.ToListAsync();
                    string[] ListatusOrder = new string[]
                    {
                         "packaged",
                         "delivery",
                         "completed"

                     };

                    var random = new Random();

                   

                    for (int i = 1; i <= 200; i++)
                    {
                        int randomIndexStatusorder = random.Next(2);
                       
                        int randomIndexStore = random.Next(listStore.Count);
                        int randomIndexUser = random.Next(listUser.Count);
                        int randomIndexProperties = random.Next(listProperties.Count);

                        var randomstatusOrder = ListatusOrder[randomIndexStatusorder];
                        var randomUser = listUser[randomIndexUser];
                        var randomStore = listStore[randomIndexStore];
                        var randomProperties = listProperties[randomIndexProperties];
                        int[] quantities = { 2, 5 };
                        var date = GetRandomDateTime(year,month,day);
                        var _quantity = random.Next(2, 100);
                        var _price = randomProperties.Price;
                        var _cost = randomProperties.CostPrice;
                       
                        dbContext.Order_Details.Add(new Order_Detail()
                            {
                                PropertiesId = randomProperties.Id,
                                OrederId = $"OD{totalRecord + i}",
                                Quantity = _quantity,
                                Price = _price,
                                Create_at = date,
                                Update_at = date,
                            });
                        dbContext.Orders.Add(new Order()
                        {
                            IdOrder = $"OD{totalRecord + i}",
                            Fee_Shipping = 2,
                            StoreId = randomProperties.StoreId,
                            UserID = randomUser.Id,
                            Status = randomstatusOrder,
                            Address = randomUser.Address,
                            Phone = randomUser.Phone,
                            Name = randomUser.FullName,
                            Distance = 5,
                            Create_at = date,
                            Update_at = date,
                            Total = _price * _quantity
                        });
                        dbContext.Payments.Add(new Payment()
                        {
                            OrderId = $"OD{totalRecord + i}",
                            Revenue = (_price - _cost) * _quantity,
                            Create_at = date,
                            Update_at = date,
                        });

                    }

                   
                    await dbContext.SaveChangesAsync();
                }
                return "Success";
            }
            catch (Exception ex) { 
                return ex.Message + " " + ex.InnerException.Message;
            }   
           
        }

        public static DateTime GetRandomDateTime(int year , int month , int day)
        {
            // Ngày bắt đầu: 24/06/2024
            DateTime startDate = new DateTime(year, month, 1);

            // Ngày hiện tại
            DateTime endDate = new DateTime(year, month, day);

            // Tổng số ngày giữa ngày bắt đầu và ngày hiện tại
            int totalDays = (endDate - startDate).Days;

            // Tạo một đối tượng Random
            Random random = new Random();

            // Tạo ra một số ngẫu nhiên trong khoảng từ 0 đến totalDays
            int randomDays = random.Next(totalDays + 1);

            // Tạo ra một số giây ngẫu nhiên trong ngày
            int randomSeconds = random.Next(86400); // 86400 là số giây trong một ngày

            // Trả về DateTime ngẫu nhiên
            return startDate.AddDays(randomDays).AddSeconds(randomSeconds);
        }
    }
}
