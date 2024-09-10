using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project_sem3.InterFace;
using Project_sem3.Model;
using Project_sem3.Models;
using Project_sem3.SendMail;
using System.Numerics;
using System.Runtime.Intrinsics.X86;

namespace Project_sem3.Repositories
{
    public class OrderFERepo  : IOrderFE
    {
        private readonly dataContext db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IEmailService _emailService;

        public OrderFERepo(dataContext db, IHttpContextAccessor httpContextAccessor, IEmailService emailSender)
        {
            this.db = db;
            _httpContextAccessor = httpContextAccessor;
            _emailService = emailSender;
        }
        public async Task<CustomResult> Create(string email, PaymentRequest a)
        {
            try
            {
                var user = await db.Users.SingleOrDefaultAsync(p => p.Email == email);
                if (user == null)
                {
                    return new CustomResult()
                    {
                        Status = 201,
                        Message = "user null",
                    };
                }

                var ListItemCart = a.ListCart;
                var voucher = await db.VoucherUsers.Include(p => p.Voucher).SingleOrDefaultAsync(p => p.Id == a.Voucher);
                var cartstock = await db.Carts.Include(p => p.Properties).ThenInclude(c => c.Goods)
                                              .Where(p => p.UserId == user.Id).ToListAsync();

                // Group items by StoreId
                var groupedItems = ListItemCart.GroupBy(itemId => cartstock.FirstOrDefault(c => c.Id == itemId)?.Properties.StoreId);
                foreach (var group in groupedItems)
                {
                    var storeId = group.Key;
                    var itemsInStore = group.ToList();

                    // Check stock for items in this store
                    foreach (var itemId in itemsInStore)
                    {
                        var cartItem = cartstock.FirstOrDefault(c => c.Id == itemId);
                        if (cartItem == null)
                        {
                            continue; // Handle if cart item not found
                        }
                        var stock = cartItem.Properties.Goods.Sum(g => g.Stock);
                        if (cartItem.Quantity > stock)
                        {
                            return new CustomResult()
                            {
                                Status = 400,
                                Message = "out of stock",
                            };
                        }
                    }

                    // Calculate total price for items in this store
                    float storeTotal = 0;
                    foreach (var itemId in itemsInStore)
                    {
                        var cart = await db.Carts.SingleOrDefaultAsync(p => p.Id == itemId);
                        if (cart != null)
                        {
                            var pro = await db.Properties.Include(p => p.Discount)
                                                         .Include(p => p.Flash_Sale)
                                                         .SingleOrDefaultAsync(p => p.Id == cart.PropertiesId);
                            if (pro != null)
                            {
                                var currentDate = DateTime.Now;
                                float discount = 0;

                                if (pro.Flash_Sale != null && pro.Flash_Sale.Start_Date <= currentDate &&
                                    pro.Flash_Sale.End_Date >= currentDate && pro.Flash_Sale.Status)
                                {
                                    discount = pro.Flash_Sale.Volume;
                                }
                                else if (pro.Discount != null && pro.Discount.Start_date <= currentDate &&
                                         pro.Discount.End_date >= currentDate && pro.Discount.Status)
                                {
                                    discount = pro.Discount.Volume;
                                }
                                var price = pro.Price;
                                var quantity = cart.Quantity;
                                storeTotal += price * quantity * (100 - discount) / 100;
                            }
                        }
                    }

                    string userIdString = user.Id.ToString();
                    string currentDateTimeString = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string idOrder = $"{userIdString}_{currentDateTimeString}_{storeId}";
                    if (voucher?.Voucher?.Type == "percent")
                    {
                        storeTotal = storeTotal * (100 - voucher.Voucher.Volume) / 100;
                    }
                    else if (voucher?.Voucher?.Type == "amount")
                    {
                        int numberOfGroups = groupedItems.Count();
                        storeTotal = storeTotal - (voucher.Voucher.Volume / numberOfGroups);
                    }

                    var Order = new Order
                    {
                        IdOrder = idOrder,
                        UserID = user.Id,
                        Fee_Shipping = a.Fee_Shipping,
                        StoreId = Convert.ToInt32(storeId), // Set StoreId for this order
                        TypeVoucher = voucher?.Voucher?.Type ?? string.Empty,
                        Address = a.Address,
                        Phone = a.Phone,
                        Name = a.Name,
                        VolumeVoucher = voucher?.Voucher?.Volume ?? 0,
                        Status = "packaged",
                        Status_Rating = "unevaluated",
                        Distance = a.Distance,
                        Total = storeTotal + a.Fee_Shipping,
                        Create_at = DateTime.Now,
                        Update_at = DateTime.Now
                    };
                    db.Orders.Add(Order);

                    var rs = await db.SaveChangesAsync();

                    MailRequest mailRequest = new MailRequest() { 
                         Subject = "Confirm Order.",
                         UserName = user.FullName,
                         ToEmail = user.Email,
                         Body = $"<h4>Thank you for ordering at SwiftMart</h4>Your order has been recorded on the system and is ready to be delivered to you in the next few hours.<p>Please pay attention to tracking your order status.</p><h4>Your Order Id : {idOrder}</h4><h4>Address : {a.Address}</h4><h4>Phone : {a.Phone}</h4>"

                    };
                    await _emailService.ConfirmOrder(mailRequest);
                     



                    var data = await db.Orders.SingleOrDefaultAsync(p => p.IdOrder == idOrder);
                    float totalCost = 0;
                    if (rs > 0)
                    {
                        user.RewardPoints += Convert.ToInt32(a.Total * 10 / 100);

                        foreach (var itemId in itemsInStore)
                        {
                            var cart = await db.Carts.Include(c => c.Properties).SingleOrDefaultAsync(p => p.Id == itemId);
                            if (cart != null)
                            {
                                var pro = await db.Properties.Include(p => p.Discount)
                                                             .Include(p => p.Flash_Sale)
                                                             .Include(p => p.Goods)
                                                             .SingleOrDefaultAsync(p => p.Id == cart.PropertiesId);
                                if (pro != null)
                                {
                                    var currentDate = DateTime.Now;
                                    float discount = 0;

                                    if (pro.Flash_Sale != null && pro.Flash_Sale.Start_Date <= currentDate &&
                                        pro.Flash_Sale.End_Date >= currentDate && pro.Flash_Sale.Status)
                                    {
                                        discount = pro.Flash_Sale.Volume;
                                    }
                                    else if (pro.Discount != null && pro.Discount.Start_date <= currentDate &&
                                             pro.Discount.End_date >= currentDate && pro.Discount.Status)
                                    {
                                        discount = pro.Discount.Volume;
                                    }
                                    var sortedGoods = pro.Goods.OrderBy(g => g.Expiry_date).ToList();
                                    int remainingQuantity = Convert.ToInt32(cart.Quantity);

                                    foreach (var good in sortedGoods)
                                    {
                                        if (remainingQuantity <= 0)
                                            break;

                                        if (good.Stock >= remainingQuantity)
                                        {
                                            good.Stock -= remainingQuantity;
                                            remainingQuantity = 0;
                                        }
                                        else
                                        {
                                            remainingQuantity -= good.Stock;
                                            good.Stock = 0;
                                        }
                                    }
                                    if (remainingQuantity > 0)
                                    {
                                        return new CustomResult()
                                        {
                                            Status = 401,
                                            Message = "Stock thieu",
                                        };
                                    }

                                    var orderDetail = new Order_Detail
                                    {
                                        OrederId = idOrder,
                                        PropertiesId = cart.PropertiesId,
                                        Quantity = cart.Quantity,
                                        Price = pro.Price,
                                        Volume_Discout = discount,
                                        Create_at = DateTime.Now,
                                        Update_at = DateTime.Now,
                                        Properties = pro // Include the Properties object to get the cost price
                                    };

                                    db.Order_Details.Add(orderDetail);
                                    db.Carts.Remove(cart);
                                    await db.SaveChangesAsync();

                                    totalCost += pro.CostPrice * cart.Quantity;
                                }
                            }
                        }
                    }

                    var payment = new Payment
                    {
                        OrderId = idOrder,
                        Revenue = storeTotal - totalCost,
                        Create_at = DateTime.Now,
                        Update_at = DateTime.Now
                    };
                    db.Payments.Add(payment);
                    await db.SaveChangesAsync();
                }

                if (voucher != null)
                {
                    db.VoucherUsers.Remove(voucher);
                    await db.SaveChangesAsync();
                }

                return new CustomResult()
                {
                    Status = 200,
                    Message = "ok",
                };
            }
            catch (Exception ex)
            {
                return new CustomResult()
                {
                    Status = 500,
                    Message = "eorr catch " + ex.Message + " " + ex?.InnerException?.Message,
                };
            }
        }


        // public async Task<int> Create(string email,PaymentRequest a)
        //{
        //     try
        //     {
        //         var ListItemCart = a.ListCart;

        //         var voucher = a.Voucher != null
        //       ? await db.VoucherUsers.Include(p => p.Voucher).SingleOrDefaultAsync(p => p.Id == a.Voucher)
        //       : null;

        //         var user = await db.Users.SingleOrDefaultAsync(p => p.Email == email);
        //         if (user != null)
        //         {
        //             var cartstock = await db.Carts.Include(p=>p.Properties).ThenInclude(c=>c.Goods).Where(p => p.UserId == user.Id).ToListAsync();
        //             foreach (var item in cartstock)
        //             {
        //                 foreach (var item1 in ListItemCart)
        //                 {
        //                     if(item.Id == item1)
        //                     {
        //                         var stock = 0;
        //                         foreach (var item2 in item.Properties.Goods)
        //                         {
        //                             stock += item2.Stock;
        //                         }
        //                         if (item.Quantity > stock)
        //                         {
        //                             return 400;
        //                         }
        //                     }
        //                 }
        //             }
        //             string userIdString = user.Id.ToString();
        //             string currentDateTimeString = DateTime.Now.ToString("yyyyMMddHHmm");
        //             string idOrder = $"{userIdString}_{currentDateTimeString}";

        //             var Order = new Order
        //             {
        //                 IdOrder = idOrder,
        //                 UserID = user.Id,
        //              Fee_Shipping = a.Fee_Shipping,
        //                 StoreId = a.StoreId,
        //                 TypeVoucher = voucher?.Voucher?.Type ?? string.Empty,
        //                 Address = a.Address,
        //                 Phone = a.Phone,
        //                 Name = a.Name,
        //                 VolumeVoucher = voucher?.Voucher?.Volume ?? 0,
        //                 Status = "packaged",
        //                 Status_Rating = "Evaluated",
        //                 Distance = a.Distance,
        //                 Total = a.Total,
        //                 Create_at = DateTime.Now,
        //                 Update_at = DateTime.Now

        //             };
        //             db.Orders.Add(Order);
        //             var rs = await db.SaveChangesAsync();
        //             if (rs > 0)
        //             {
        //                 if(voucher != null)
        //                 {
        //                     db.VoucherUsers.Remove(voucher);
        //                     await db.SaveChangesAsync();
        //                 }
        //                 int intValue = Convert.ToInt32(a.Total);
        //                 user.RewardPoints = user.RewardPoints + (10*intValue/ 100);
        //                 await db.SaveChangesAsync();
        //                 foreach (var item in ListItemCart)
        //                 {
        //                     var cart = await db.Carts.SingleOrDefaultAsync(p => p.Id == item);
        //                     if (cart != null)
        //                     {
        //                         var pro = await db.Properties.Include(p => p.Discount)
        //                                                      .Include(p => p.Flash_Sale)
        //                                                      .Include(p => p.Goods) // Bao gồm danh sách Goods
        //                                                      .SingleOrDefaultAsync(pro => pro.Id == cart.PropertiesId);
        //                         if (pro != null)
        //                         {
        //                             var currentDate = DateTime.Now;
        //                             float discount = 0;

        //                             if (pro.Flash_Sale != null && pro.Flash_Sale.Start_Date <= currentDate && pro.Flash_Sale.End_Date >= currentDate && pro.Flash_Sale.Status)
        //                             {
        //                                 discount = pro.Flash_Sale.Volume;
        //                             }
        //                             else if (pro.Discount != null && pro.Discount.Start_date <= currentDate && pro.Discount.End_date >= currentDate && pro.Discount.Status)
        //                             {
        //                                 discount = pro.Discount.Volume;
        //                             }

        //                             // Lấy danh sách Goods và sắp xếp theo Expiry_date tăng dần
        //                             var sortedGoods = pro.Goods.OrderBy(g => g.Expiry_date).ToList();

        //                             int remainingQuantity = Convert.ToInt32(cart.Quantity);

        //                             foreach (var good in sortedGoods)
        //                             {
        //                                 if (remainingQuantity <= 0)
        //                                     break;

        //                                 if (good.Stock >= remainingQuantity)
        //                                 {
        //                                     good.Stock -= remainingQuantity;
        //                                     remainingQuantity = 0;
        //                                 }
        //                                 else
        //                                 {
        //                                     remainingQuantity -= good.Stock;
        //                                     good.Stock = 0;
        //                                 }
        //                             }

        //                             if (remainingQuantity > 0)
        //                             {

        //                                 return 400;
        //                             }

        //                             var orderDetail = new Order_Detail
        //                             {
        //                                 OrederId = idOrder,
        //                                 PropertiesId = cart.PropertiesId,
        //                                 Quantity = cart.Quantity,
        //                                 Price = pro.Price,
        //                                 Volume_Discout = discount,
        //                                 Create_at = DateTime.Now,
        //                                 Update_at = DateTime.Now
        //                             };

        //                             db.Order_Details.Add(orderDetail);
        //                             var result = await db.SaveChangesAsync();
        //                             if (result > 0)
        //                             {
        //                                 db.Carts.Remove(cart);
        //                                 await db.SaveChangesAsync();
        //                             }
        //                         }
        //                     }
        //                 }
        //             }

        //         }
        //         return 1;
        //     }
        //     catch (Exception ex)
        //     {
        //         return 0;
        //     }


        // }

       

        public async Task<IEnumerable<Order>> Get(string email)
        {
            try
            {
                var user = await db.Users.SingleOrDefaultAsync(u=>u.Email == email);
                if(user != null)
                {
                    var list = await db.Orders.Include(o=>o.OrderDetails).Where(o =>o.UserID == user.Id).ToListAsync();
                    return list;
                }
                return null;
                
            }catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Order> GetOrderDetails(string idOrder)
        {
            try
            {
                var or = await db.Orders.Include(o=>o.OrderDetails).ThenInclude(p=>p.Properties).ThenInclude(p=>p.Product).SingleOrDefaultAsync(or=>or.IdOrder == idOrder);
                foreach (var iteam in or.OrderDetails)
                {
                    iteam.Properties.Image = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/PropertiesImage/{iteam.Properties.Image}";
                }
                return or;
            }catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<Order> UpdateStatusOrder(string idOrder)
        {
            try
            {
                var or = await db.Orders.SingleOrDefaultAsync(o=>o.IdOrder == idOrder);
                if(or != null)
                {
                    or.Status_Rating = "evaluated";
                   var rs = await db.SaveChangesAsync();
                    if (rs > 0)
                    {
                        return or;
                    }

                }
                return null;
            }catch (Exception ex)
            {
                return null;
            }
        }

        //public class OrderRes
        //{
        //    public string IdOrder { get; set; }

        //    public int UserID { get; set; }

        //    public float Fee_Shipping { get; set; }

        //    public int StoreId { get; set; }

        //    public string? TypeVoucher { get; set; }

        //    public float? VolumeVoucher { get; set; }

        //    public string Status { get; set; }

        //    public string Address { get; set; }
        //    public string Phone { get; set; }

        //    public string Name { get; set; }
        //    public float Distance { get; set; }

        //    public string? Status_Rating { get; set; }
        //    public float Total { get; set; }

        //    public DateTime? Create_at { get; set; }

        //    public DateTime? Update_at { get; set; }

        //    public virtual User? User { get; set; }


           

        

         

          
        //}
    }
}
