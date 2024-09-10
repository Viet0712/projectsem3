using Microsoft.EntityFrameworkCore;
using Project_sem3.InterFace;
using Project_sem3.Models;
using System.Security;

namespace Project_sem3.Repositories
{
    public class PermissionRepo : InterFace.IPermission
    {
        private readonly dataContext _context;

        public PermissionRepo(dataContext dataContext)
        {
            _context = dataContext;
        }

        public async Task<CustomResult> getAll()
        {
            try {
                var data = await _context.Permissions.Include(e=>e.Admin).ToListAsync();
                var result = new CustomResult()
                {
                    Status = 200,
                    Message = "OK",
                    data = data
                };
                return result;
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

        public async Task<CustomResult> getByIdAdmin(int id)
        {
            try
            {
                var data = await _context.Permissions.SingleOrDefaultAsync(x => x.AdminId == id);

                return new CustomResult() { Status = 200, Message = "OK", data = data };
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

        public async Task<CustomResult> Update(int id , string type)
        {
            try {
                var data = await _context.Permissions.SingleOrDefaultAsync(x => x.Id == id);
                if(data != null)
                {
                    if (type == "addProperties")
                    {
                        data.AddProperties = !data.AddProperties;
                    }
                    if (type == "addGoods")
                    {
                        data.AddGoods = !data.AddGoods;
                    }
                    if (type == "setEvent")
                    {
                        data.SetEven = !data.SetEven;
                    }
                    _context.Permissions.Update(data);
                    await _context.SaveChangesAsync();
                }
              

                return new CustomResult() { Status = 200, Message = "OK", data = data };

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
