using Project_sem3.Models;

namespace Project_sem3.InterFace
{
    public interface IProduct
    {
        Task<CustomResult> GetAll();

        Task<CustomResult> GetAllTrue();

        Task<CustomResult> Create(Product e);

        Task<CustomResult> Update(Product e);

        Task<CustomResult> Search(string? name, bool? status , int? brandId , int? categoryId , int?subcategoryId , int? segmentId );

        Task<CustomResult> GetById(string id);

        Task<CustomResult> ChangeStatus(string id);
    }
}
