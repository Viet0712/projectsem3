using Project_sem3.Models;

namespace Project_sem3.InterFace
{
    public interface ICategory
    {
        Task<CustomResult> GetAllCategoryTrue();
        Task<CustomResult> GetAllCategory();
        Task<CustomResult> CreateCategory(Category category);

        Task<CustomResult> UpdateCategory(Category category);

        Task<CustomResult> GetById(int id);
        Task<CustomResult> Search(string? name,bool? status);

        Task<CustomResult> ChangeStatus(int id);
    }
}
