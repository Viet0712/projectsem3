using Microsoft.EntityFrameworkCore;
using Project_sem3.InterFace;
using Project_sem3.Models;

namespace Project_sem3.Repositories
{
    public class DiscountRepo : IDiscount
    {
        private readonly dataContext _dataContext;
        public DiscountRepo(dataContext dataContext)
        {
            _dataContext = dataContext;
        }
        public async Task<CustomResult> ChangeStatus(int id)
        {
            try
            {
                var data = await _dataContext.Discounts.SingleOrDefaultAsync(e => e.Id == id);
                if (data == null)
                {

                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Record Not Found! ",

                    };
                }
                else
                {
                    data.Status = !data.Status;
                    data.Update_at = DateTime.Now;
                    _dataContext.Discounts.Update(data);
                    await _dataContext.SaveChangesAsync();

                    return new CustomResult()
                    {
                        Status = 200,
                        Message = "Change Status Success ",
                        data = data

                    };
                }
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

        public async Task<CustomResult> Create(Discount d)
        {
            try {
                var data = await _dataContext.Discounts.SingleOrDefaultAsync(e => e.Name.ToLower() == d.Name.ToLower());
                if (data != null)
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Duplicate Discount Name! "


                    };
                }
                d.Create_at = DateTime.Now;
                d.Status = false;
                _dataContext.Discounts.Add(d);
                await _dataContext.SaveChangesAsync();
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Create Discount Success!",
                    data = d
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
            try {
                var list = await _dataContext.Discounts.ToListAsync();
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Get Discount Success!",
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
        public async Task<CustomResult> GetAllTrue()
        {
            try
            {
                var list = await _dataContext.Discounts.Where(e=>e.Status==true).ToListAsync();
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Get Discount Success!",
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
                var data = await _dataContext.Discounts.SingleOrDefaultAsync(e=>e.Id == id);
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
                    Message = "Get Discount Success!",
                    data = data
                    
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

        public async Task<CustomResult> Search(string? name, bool? expired, bool? status)
        {
            try
            {
                var query = _dataContext.Discounts.AsQueryable();
                var now = DateTime.Now;
                if (!string.IsNullOrEmpty(name))
                    query = query.Where(s => s.Name.Contains(name));



                if (status.HasValue)
                    query = query.Where(s => s.Status == status.Value);

                if (expired.HasValue && expired == true)
                {


                    query = query.Where(s => s.End_date < now);
                }

                if (expired.HasValue && expired == false)
                {


                    query = query.Where(s => s.End_date > now);
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

        public async Task<CustomResult> Update(Discount d)
        {
            try { 
                var dataOld = await _dataContext.Discounts.SingleOrDefaultAsync(e=>e.Id==d.Id);
                if (dataOld == null)
                {
                    return new CustomResult()
                    {
                        Status = 205,
                        Message = "Record Not Found!",

                    };
                }
                if (dataOld.Name.ToLower() != d.Name.ToLower())
                {
                    var data = await _dataContext.Discounts.SingleOrDefaultAsync(e=>e.Name.ToLower()==d.Name.ToLower());
                    if(data != null)
                    {
                        return new CustomResult()
                        {
                            Status = 205,
                            Message = "Duplicate Discount Name! "

                        };
                    }    
                }
                dataOld.Update_at = DateTime.Now;
                dataOld.Start_date = d.Start_date;
                dataOld.End_date = d.End_date;
                dataOld.Name = d.Name;
                dataOld.Volume = d.Volume;
                _dataContext.Discounts.Update(dataOld);
                await _dataContext.SaveChangesAsync();
                return new CustomResult()
                {
                    Status = 200,
                    Message = "Update Success!",
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


        static void AutoNotifyAt8am()
        {
            
            int hour = DateTime.Now.Hour;

           
            if (hour == 8)
            {
              
            }
        }
        static void Run(string[] args)
        {
            
            while (true)
            {
                AutoNotifyAt8am();
                Thread.Sleep(60000); // Chờ 1 phút
            }
        }
        
    }
}
