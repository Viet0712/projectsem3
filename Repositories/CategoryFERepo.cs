using Microsoft.EntityFrameworkCore;
using Project_sem3.InterFace;
using Project_sem3.Models;

namespace Project_sem3.Repositories
{
    public class CategoryFERepo : ICategoryFE
    {
        private readonly dataContext db;

        public CategoryFERepo(dataContext db)
        {
            this.db = db;
        }

     

        public async Task<IEnumerable<Category>> GetAllCategory()
        {
            try
            {
                var list = await db.Categories
                 .Include(p => p.Subcategories)
                 .ThenInclude(p => p.Segments)
                 .ToListAsync();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public async Task<IEnumerable<Category>> GetCategory(int id)
        {
            try
            {
                var list = await db.Categories
                .Include(p => p.Products)
                  .Include(p => p.Subcategories)
                  .ThenInclude(p => p.Segments)
                  .Where(p => p.Id == id)
                  .ToListAsync();
                return list;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Subcategory>> GetSubCategory(int id)
        {
            try
            {
                var list = await db.subcategories
              .Include(p => p.Products)
              .Include(p => p.Segments)
                .Where(p => p.Id == id)
              .ToListAsync();
                return list;
            }
          
              catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<IEnumerable<Segment>> GetSegment(int id)
        {
            try
            {
                var list = await db.Segments
             .Include(p => p.Products)
             .Where(p => p.Id == id)
             .ToListAsync();
                return list;

            }
            catch (Exception ex)
            {
                return null ;
            }
         
        }

       
    }
}
