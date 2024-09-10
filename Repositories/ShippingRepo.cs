using Microsoft.EntityFrameworkCore;
using Project_sem3.InterFace;
using Project_sem3.Models;

namespace Project_sem3.Repositories
{
    public class ShippingRepo : IShipping
    {
        private readonly dataContext _dataContext;
        public ShippingRepo(dataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<CustomResult> ChangeStatus(int id)
        {
            try
            {
                var data = await _dataContext.Shippings.SingleOrDefaultAsync(e=>e.Id==id);
                if (data!=null)
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Shipping Record Not Found!!"
                    };
                }
                else
                {
                    data.Status = ! data.Status;
                    _dataContext.Shippings.Update(data);
                    await _dataContext.SaveChangesAsync();
                    return new CustomResult()
                    {
                        Status = 200,
                        Message = "Change Status Shipping Success!",
                        data = data
                    };
                }
            }
            catch (Exception ex) {

                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error!! " + ex.Message
                };
            }
        }

        public async Task<CustomResult> CreateShipping(Shipping shipping)
        {
            try { 
                shipping.Create_at = DateTime.Now;
                shipping.Status = false;
                _dataContext.Shippings.Add(shipping);
                await _dataContext.SaveChangesAsync();
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Create Shipping Success!",
                    data = shipping
                };
            }
            catch (Exception ex) {

                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error!! " + ex.Message
                };
            }
        }

        public async Task<CustomResult> GetAllShipping()
        {
            try {
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Get Shipping Success!",
                    data = await _dataContext.Shippings.ToListAsync()
                };
            }
            catch (Exception ex) {
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error!! " + ex.Message
                };
            }
        }

        public async Task<CustomResult> SearchByName(string name)
        {
            try
            {
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Search Shipping Success!",
                    data = await _dataContext.Shippings.Where(e=>e.Name.Contains(name)).ToListAsync()
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

        public async Task<CustomResult> UpdateShipping(Shipping shipping)
        {
            try { 
                var dataOld = await _dataContext.Shippings.SingleOrDefaultAsync(e=>e.Id==shipping.Id);
                if (dataOld == null)
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Update Shipping Fail , Record Not Found!",
                        data = shipping
                    };
                }
                else
                {
                    dataOld.Update_at = DateTime.Now;
                   
                    dataOld.Description = shipping.Description;
                    dataOld.Name = shipping.Name;
                    dataOld.Price = shipping.Price;
                    _dataContext.Shippings.Update(dataOld);
                    await _dataContext.SaveChangesAsync();
                    return new CustomResult()
                    {
                        Status = 200,
                        Message = "Update Shipping Success!",
                        data = dataOld
                    };
                }
            }
            catch (Exception ex) {
                return new CustomResult()
                {
                    Status = 400,
                    Message = "Server Error!! " + ex.Message
                };
            }
        }
    }
}
