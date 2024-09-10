using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Utilities.Collections;
using Project_sem3.InterFace;
using Project_sem3.Models;

namespace Project_sem3.Repositories
{
    public class StoreFERepo: IStoreFE
    {
        private readonly dataContext db;

        public StoreFERepo(dataContext db)
        {
            this.db = db;
        }

        public async Task<IEnumerable<Store>> Get()
        {
            try
            {
                var list = await db.Stores.Where(p=>p.Status == true).ToListAsync();
                return list;
            }catch (Exception ex)
            {
                return null;
            }
        }
    }
}
