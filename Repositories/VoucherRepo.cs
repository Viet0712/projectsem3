using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.EntityFrameworkCore;
using Project_sem3.InterFace;
using Project_sem3.Models;

namespace Project_sem3.Repositories
{
    public class VoucherRepo : IVoucher
    {
        private readonly dataContext _dataContext;
        public VoucherRepo(dataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<CustomResult> ChangeStatus(int id)
        {
            try
            {
                var data = await _dataContext.Vouchers.SingleOrDefaultAsync(e => e.Id == id);
                if (data != null)
                {
                    data.Status = !data.Status;
                    data.Update_at = DateTime.Now;
                    _dataContext.Vouchers.Update(data);
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

        public async Task<CustomResult> Create(Voucher e)
        {
            try
            {
                var data = await _dataContext.Vouchers.SingleOrDefaultAsync(a=>a.Name.ToLower() == e.Name.ToLower());
                if (data != null)
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Duplicate Name!",
                        data = e

                    };
                }
                e.Create_at = DateTime.Now;
                e.Status = true;
                _dataContext.Vouchers.Add(e);
                await _dataContext.SaveChangesAsync();
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Create Success!",
                    data = e

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

        public async Task<CustomResult> GetAll()
        {
            try
            {
                var list = await _dataContext.Vouchers.ToListAsync();
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Get  Success!",
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
        public async Task<CustomResult> GetAllTrue()
        {
            try
            {
                var list = await _dataContext.Vouchers.Where(e=>e.Status==true).ToListAsync();
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Get  Success!",
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
                var data = await _dataContext.Vouchers.SingleOrDefaultAsync(e => e.Id == id);
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
                    Message = "Get  Success!",
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

        public async Task<CustomResult> Search(string? name, bool? expired, bool? status, string? Type)
        {
            try
            {
                var query = _dataContext.Vouchers.AsQueryable();
                var now = DateTime.Now;
                if (!string.IsNullOrEmpty(name))
                    query = query.Where(s => s.Name.Contains(name));

                if (!string.IsNullOrEmpty(Type))
                    query = query.Where(s => s.Type == Type);

                if (status.HasValue)
                    query = query.Where(s => s.Status == status.Value);

                if (expired.HasValue && expired == true)
                {


                    query = query.Where(s => s.Expiry_date < now);
                }

                if (expired.HasValue && expired == false)
                {


                    query = query.Where(s => s.Expiry_date > now);
                }



                var result = await query.ToListAsync();


                return new CustomResult()
                {
                    Status = 200,
                    Message = "Search Success!",
                    data = result
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

        public async Task<CustomResult> Update(Voucher e)
        {
            try
            {
                var dataOld = await _dataContext.Vouchers.SingleOrDefaultAsync(a => a.Id == e.Id);
                if (dataOld == null)
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Record Not Found!",
                    };
                }
                if (dataOld.Name.ToLower() != e.Name.ToLower())
                {
                    var data = await _dataContext.Vouchers.SingleOrDefaultAsync(a=>a.Name.ToLower()==e.Name.ToLower());
                    if(data != null)
                    {
                        return new CustomResult()
                        {
                            Status = 205,
                            Message = "Duplicate Name!",
                        };
                    }
                }
                dataOld.Start_at = e.Start_at;
                dataOld.Update_at = DateTime.Now;
                dataOld.Expiry_date = e.Expiry_date;
                dataOld.Name = e.Name;
                dataOld.Type = e.Type;
                dataOld.Volume = e.Volume;
                dataOld.Quantity = e.Quantity;
                _dataContext.Vouchers.Update(dataOld);
                await _dataContext.SaveChangesAsync();
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Update Flash Sale Success!",
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
    }
}
