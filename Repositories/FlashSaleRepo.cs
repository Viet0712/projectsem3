using Microsoft.EntityFrameworkCore;
using Project_sem3.InterFace;
using Project_sem3.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Project_sem3.Repositories
{
    public class FlashSaleRepo : IFlashSale
    {
        private readonly dataContext _dataContext;
        public FlashSaleRepo(dataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<CustomResult> ChangeStatus(int id)
        {
            try {
                var data = await _dataContext.Flash_Sales.SingleOrDefaultAsync(e=>e.Id==id);
                if (data != null)
                {
                    data.Status = !data.Status;
                    data.Update_at = DateTime.Now;
                    _dataContext.Flash_Sales.Update(data);
                    await _dataContext.SaveChangesAsync();
                    return new CustomResult()
                    {
                        Status = 200,
                        Message = "Change Status Flash Sale Success",
                        
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

        public async Task<CustomResult> Create(Flash_Sale e)
        {
            try
            {
                var data = await _dataContext.Flash_Sales.SingleOrDefaultAsync(a => a.Name.ToLower() == e.Name.ToLower());
                if(data != null)
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Duplicate Flash Sale Name!",
                       

                    };
                }
                e.Create_at = DateTime.Now;
                e.Status = false;
                _dataContext.Flash_Sales.Add(e);
                await _dataContext.SaveChangesAsync();
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Create Flash Sale Success!",
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

        public async Task<CustomResult> GetAllTrue()
        {
            try {
                var list = await _dataContext.Flash_Sales.Where(e=>e.Status==true).ToListAsync();
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Get Flash Sale Success!",
                    data = list
                };
            }
            catch (Exception ex) {
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
                var list = await _dataContext.Flash_Sales.ToListAsync();
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Get Flash Sale Success!",
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
            try {
                var data = await _dataContext.Flash_Sales.SingleOrDefaultAsync(e=>e.Id == id);
                if(data == null)
                {
                    return new CustomResult()
                    {
                        Status= 205,
                        Message = "Record Not Found!",
                    };
                }
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Get Flash Sale Success!",
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

        public async Task<CustomResult> Search(string? name, bool? expired, bool? status)
        {
            try
            {
                var query = _dataContext.Flash_Sales.AsQueryable();
                var now = DateTime.Now;
                if (!string.IsNullOrEmpty(name))
                    query = query.Where(s => s.Name.Contains(name));



                if (status.HasValue)
                    query = query.Where(s => s.Status == status.Value);

                if (expired.HasValue && expired == true)
                {
                   

                    query = query.Where(s => s.End_Date<now);
                }

                if (expired.HasValue && expired == false)
                {
                  

                    query = query.Where(s => s.End_Date > now);
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

        public async Task<CustomResult> Update(Flash_Sale e)
        {
            try {
                var dataOld = await _dataContext.Flash_Sales.SingleOrDefaultAsync(a=>a.Id==e.Id);
                if(dataOld == null)
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Record Not Found!",
                    };
                }
                if (dataOld.Name.ToLower() != e.Name.ToLower())
                {
                    var data = await _dataContext.Flash_Sales.SingleOrDefaultAsync(a=>a.Name.ToLower()==e.Name.ToLower());
                    if(data != null)
                    {
                        return new CustomResult()
                        {
                            Status = 205,
                            Message = "Duplicate Flash Sale Name!",


                        };
                    }
                }
                dataOld.Start_Date = e.Start_Date;
                dataOld.Update_at = DateTime.Now;
                dataOld.End_Date = e.End_Date;
                dataOld.Name = e.Name;
                dataOld.Description = e.Description;
                dataOld.Volume = e.Volume;
                _dataContext.Flash_Sales.Update(dataOld);
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
