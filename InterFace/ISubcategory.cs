using Project_sem3.Models;

namespace Project_sem3.InterFace
{
    public interface ISubcategory
    {
        Task<CustomResult> GetAllSubcategoryTrue();
        Task<CustomResult> GetAllSubcategory();
        Task<CustomResult> CreateCategory(Subcategory subcategory);

        Task<CustomResult> UpdateSubCategory(Subcategory subcategory);

        Task<CustomResult> GetById(int id);
        Task<CustomResult> Search(string? name,bool? status,int? categoryId);

        Task<CustomResult> ChangeStatus(int id);
    }
}
