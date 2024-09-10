using Microsoft.EntityFrameworkCore;
using Project_sem3.InterFace;
using Project_sem3.Models;

namespace Project_sem3.Repositories
{
    public class EventRepo : IEvent
    {
        private readonly dataContext _dataContext;
        public EventRepo(dataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<CustomResult> Get()
        {
            try
            {
                var list = await _dataContext.Properties.Include(e => e.Product).ThenInclude(e => e.Brand).Include(e => e.Product).ThenInclude(e => e.Category).Include(e => e.Product).ThenInclude(e => e.Subcategory).Include(e => e.Product).ThenInclude(e => e.Segment).Include(e => e.Store).Include(e=>e.Discount).Include(e=>e.FlashSaleId).ToListAsync();

                return new CustomResult()
                {
                    Status = 200,
                    Message = "Get Success",
                    data = list
                };
            }
            catch (Exception ex)
            {
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error! " + ex.Message
                };
            }
        }

        public async Task<CustomResult> RemoveDiscount(int id)
        {
            try {
                var data = await _dataContext.Properties.SingleOrDefaultAsync(e=>e.Id == id);
                data.DiscountId = null;
                _dataContext.Properties.Update(data);
                await _dataContext.SaveChangesAsync();
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Remove Success!"
                };
                
            }
            catch (Exception ex)
            {
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error! " + ex.Message
                };
            }
        }

        public async Task<CustomResult> RemoveFlashSale(int id)
        {
            try
            {
                var data = await _dataContext.Properties.SingleOrDefaultAsync(e => e.Id == id);
                data.FlashSaleId = null;
                _dataContext.Properties.Update(data);
                await _dataContext.SaveChangesAsync();
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Remove Success!"
                };

            }
            catch (Exception ex)
            {
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error! " + ex.Message
                };
            }
        }

        public async Task<CustomResult> SetDiscount(List<int> PropertiesId, int DiscountId)
        {
            try {
                foreach (var property in PropertiesId)
                {
                    var data = await _dataContext.Properties.SingleOrDefaultAsync(e=>e.Id == property);
                    data.DiscountId = DiscountId;
                    _dataContext.Properties.Update(data);
                }
                await _dataContext.SaveChangesAsync();
                return new CustomResult() { 
                    Status = 200,
                    Message = "Set Discount Success!"

                };
            }
            catch (Exception ex)
            {
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error! " + ex.Message
                };
            }
        }

        public async Task<CustomResult> SetFlashSale(List<int> PropertiesId, int FlashSaleId)
        {
            try
            {
                foreach (var property in PropertiesId)
                {
                    var data = await _dataContext.Properties.SingleOrDefaultAsync(e => e.Id == property);
                    data.FlashSaleId = FlashSaleId;
                    _dataContext.Properties.Update(data);
                }
                await _dataContext.SaveChangesAsync();
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Set Flash Sale Success!"

                };
            }
            catch (Exception ex)
            {
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error! " + ex.Message
                };
            }
        }
    }
}
