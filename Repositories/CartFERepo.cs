using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Project_sem3.InterFace;
using Project_sem3.Models;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net.WebSockets;
using System.Security.Claims;

namespace Project_sem3.Repositories
{
    public class CartFERepo : ICartFE
    {
        private readonly dataContext db;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartFERepo(dataContext _dataContext, IHttpContextAccessor httpContextAccessor)
        {
            this.db = _dataContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<Cart> CreateCart(CartCreate c)
        {
            try
            {
              //  var email = GetEmailFromToken(c.Token);

                var user = await db.Users.SingleOrDefaultAsync(p => p.Email == c.Email);
                if (user == null)
                {
                    return null;
                }
                var cartItem = await db.Carts.SingleOrDefaultAsync(p=>p.UserId == user.Id&& p.PropertiesId == c.PropertiesId);
                if(cartItem == null)
                {
                    Cart cart = new Cart
                    {
                        UserId = user.Id,
                        PropertiesId = c.PropertiesId,
                        Quantity = c.Quantity,
                        Create_at = DateTime.UtcNow,
                        Update_at = DateTime.UtcNow
                    };
                    db.Carts.Add(cart);
                    var rs = await db.SaveChangesAsync();
                    if (rs > 0)
                    {
                        return cart;
                    }
                }
                else
                {
                    cartItem.Quantity += c.Quantity;
                    await db.SaveChangesAsync();
                    return cartItem;
                }

                return null;
            }catch (Exception ex)
            {
                return null;
            }
        }

       

        public async Task<int> DeleteCart(int[] ids)
        {
            try
            {
                foreach (var id in ids)
                {
                    var cartItem = await db.Carts.SingleOrDefaultAsync(p => p.Id == id);

                    if (cartItem != null)
                    {
                        db.Carts.Remove(cartItem);
                    }
                }

                await db.SaveChangesAsync();

                return ids.Length;
            }catch (Exception ex)
            {
                return 0;
            }
        }
        public async Task<IEnumerable<Cart>> GetCart(string email)
        {
            try
            {
                var currentDate = DateTime.Now;
                var cartItems = await db.Carts
                    .Include(u=>u.User)
                    .Where(c => c.User.Email == email) // Filter by email
                    .Include(c => c.Properties)
                        .ThenInclude(prop => prop.Discount)
                           .Include(c => c.Properties)
                        .ThenInclude(prop => prop.Store)
                    .Include(c => c.Properties)
                        .ThenInclude(prop => prop.Flash_Sale)
                         .Include(c => c.Properties)
                        .ThenInclude(prop => prop.Goods)
                       .Include(c => c.Properties)
                        .ThenInclude(prop => prop.Product)
                    .Select(c => new Cart
                    {
                        Id = c.Id,
                        Quantity = c.Quantity,
                        Properties = c.Properties != null && c.Properties.Status ? new Properties
                        {
                            Id = c.Properties.Id,
                            Name = c.Properties.Name,
                            Price = c.Properties.Price,
                            StoreId = c.Properties.StoreId,
                            DiscountId = c.Properties.DiscountId,
                            FlashSaleId = c.Properties.FlashSaleId,
                           
                            Image = c.Properties.Image,
                            Status = c.Properties.Status,

                            Discount = c.Properties.Discount != null && c.Properties.Discount.Status && c.Properties.Discount.Start_date <= currentDate && c.Properties.Discount.End_date >= currentDate ? new Discount
                            {
                                Id = c.Properties.Discount.Id,
                                Name = c.Properties.Discount.Name,
                                Volume = c.Properties.Discount.Volume,
                                Status = c.Properties.Discount.Status,
                                Start_date = c.Properties.Discount.Start_date,
                                End_date = c.Properties.Discount.End_date
                            } : null,
                            Product = c.Properties.Product != null && c.Properties.Product.Status ? new Product
                            {
                                ProductId = c.Properties.Product.ProductId,
                                Name = c.Properties.Product.Name,
                                Image = c.Properties.Product.Image
                                
                            } : null,
                            Store = c.Properties.Store != null && c.Properties.Store.Status ? new Store
                            {
                                Id = c.Properties.Store.Id,
                                District = c.Properties.Store.District,
                                City = c.Properties.Store.City

                            } : null,
                            Flash_Sale = c.Properties.Flash_Sale != null && c.Properties.Flash_Sale.Status && c.Properties.Flash_Sale.Start_Date <= currentDate && c.Properties.Flash_Sale.End_Date >= currentDate ? new Flash_Sale
                            {
                                Id = c.Properties.Flash_Sale.Id,
                                Name = c.Properties.Flash_Sale.Name,
                                Volume = c.Properties.Flash_Sale.Volume,
                                End_Date = c.Properties.Flash_Sale.End_Date,
                                Start_Date = c.Properties.Flash_Sale.Start_Date,
                                Status = c.Properties.Flash_Sale.Status
                            } : null,
                            Goods = c.Properties.Goods.Select(or => new Goods
                            {
                                Id = or.Id,
                                Arrival_date = or.Arrival_date,
                                Expiry_date = or.Expiry_date,
                                Stock = or.Stock
                            }).ToList(),

                        } : null
                    })
                    .ToListAsync();
                foreach (var iteam in cartItems)
                {
                    iteam.Properties.Image = $"{_httpContextAccessor.HttpContext.Request.Scheme}://{_httpContextAccessor.HttpContext.Request.Host}{_httpContextAccessor.HttpContext.Request.PathBase}/PropertiesImage/{iteam.Properties.Image}";
                }

                return cartItems;
            }
            catch (Exception ex)
            {
                // Handle exceptions (log, notify, etc.)
                return null;
            }
        }

        public async Task<Cart> UpdateCart(Cart cart)
        {
            try
            {
                var cartItems = await db.Carts.SingleOrDefaultAsync(c=>c.Id == cart.Id);
                if(cartItems == null)
                {
                    return null;
                }
                else
                {
                    var property = await db.Properties.Include(p=>p.Goods).SingleOrDefaultAsync(p => p.Id == cartItems.PropertiesId);
                    if(property == null)
                    {
                        return null;
                    }
                    //if(property.Goods[ < cart.Quantity)
                    //{
                    //    return null;
                    //}
                    cartItems.Quantity = cart.Quantity;
                    var rs = await db.SaveChangesAsync();
                    if (rs > 0)
                    {
                        return cartItems;
                    }
                    return null;
                } 
            }catch(Exception ex)
            {
                return null;
            }
        }
    }
}
