using Project_sem3.Models;

namespace Project_sem3.InterFace
{
    public interface ICategoryFE
    {
        Task<IEnumerable<Category>> GetAllCategory();
        Task<IEnumerable<Category>> GetCategory(int id);
        Task<IEnumerable<Subcategory>> GetSubCategory(int id);
        Task<IEnumerable<Segment>> GetSegment(int id);
    }
}
