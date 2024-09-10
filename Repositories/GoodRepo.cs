using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using Project_sem3.InterFace;
using Project_sem3.Models;

namespace Project_sem3.Repositories
{
    public class GoodRepo : IGoods
    {
        private readonly dataContext _dataContext;
        public GoodRepo(dataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<CustomResult> ChangeStatus(int id)
        {
            try
            {
                var data = await _dataContext.Goods.SingleOrDefaultAsync(e => e.Id == id);
                if (data != null)
                {
                    data.Status = !data.Status;

                    _dataContext.Goods.Update(data);
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

        public async Task<CustomResult> Create(Goods g)
        {
            try
            {
              
                g.Status = true;
                _dataContext.Goods.Add(g);
                await _dataContext.SaveChangesAsync();
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Create  Success!",
                    data = g
                };
            }
            catch (Exception ex)
            {

                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error!! " + ex.Message
                };
            }
        }

        public async Task<CustomResult> GetAll(int id)
        {
            try
            {
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Get  Success!",
                    data = await _dataContext.Goods.Include(e=>e.Properties).ThenInclude(e=>e.Product).ThenInclude(e=>e.Brand).Where(e=>e.Properties.StoreId==id).Select(e=>new GoodRes()
                    {
                        Id= e.Id,
                        PropertiesId = e.PropertiesId,
                        Arrival_date = e.Arrival_date,
                        Expiry_date = e.Expiry_date,
                        Status = e.Status,
                        Stock = e.Stock,
                        Type = e.Properties.Name,
                        ProductName = e.Properties.Product.Name,
                        BrandId = e.Properties.Product.BrandId,
                        BrandName = e.Properties.Product.Brand.Name,
                        Cost = e.Properties.CostPrice,
                        Price = e.Properties.Price,
                    }).ToListAsync()
                };
            }
            catch (Exception ex)
            {
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error!! " + ex.Message
                };
            }
        }

        public async Task<CustomResult> GetById(int id)
        {
            try
            {
                var data = await _dataContext.Goods.SingleOrDefaultAsync(e => e.Id == id);
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
                    Message = "Get Success!",
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

        public async Task<CustomResult> Update(Goods e)
        {
            try
            {
                var dataOld = await _dataContext.Goods.SingleOrDefaultAsync(a => a.Id == e.Id);
                if (dataOld == null)
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Record Not Found!",
                    };
                }
                dataOld.Arrival_date = e.Arrival_date;
              
                dataOld.Expiry_date = e.Expiry_date;
                dataOld.Stock = e.Stock;
                dataOld.PropertiesId = e.PropertiesId;
              
                _dataContext.Goods.Update(dataOld);
                await _dataContext.SaveChangesAsync();
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Update Success!",
                    data = dataOld
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

        public class GoodRes
        {
            public float Cost { get; set; }

            public float Price { get; set; }
            public string BrandName { get; set; }
            public string ProductName { get; set; }

            public int? BrandId { get; set; }
            public string Type { get; set; }
            public int Id { get; set; }

            public int PropertiesId { get; set; }

            public DateTime Arrival_date { get; set; }

            public DateTime? Expiry_date { get; set; }

            public int Stock { get; set; }

            public bool? Status { get; set; }

           
        }
    }
}
