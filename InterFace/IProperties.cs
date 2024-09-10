using Project_sem3.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Project_sem3.InterFace
{
    public interface IProperties
    {
        Task<CustomResult> GetAll(int id);
        Task<CustomResult> Create(Product_Properties e);

        Task<CustomResult> Update(Product_Properties e);
        Task<CustomResult> GetAll();
        Task<CustomResult> GetAllTrue();

        Task<CustomResult> Create(Properties e);
        //Task<CustomResult> Update(Properties e);

        Task<CustomResult> Search(string? name, bool? status, int? brandId, int? categoryId, int? subcategoryId, int? segmentId ,int? storeId,bool? expiried);

        Task<CustomResult> GetById(int id);

        Task<CustomResult> GetByIdAdmin(int id);

        Task<CustomResult> ChangeStatus(int id);

        Task<CustomResult> ChangeStatusSadmin(int id);
    }
}
