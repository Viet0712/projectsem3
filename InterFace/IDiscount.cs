using Project_sem3.Models;

namespace Project_sem3.InterFace
{
    public interface IDiscount
    {
        Task<CustomResult> GetAll();
        Task<CustomResult> GetAllTrue();
        Task<CustomResult> Create(Discount d);

        Task<CustomResult> Update(Discount d);

        Task<CustomResult> Search(string? name, bool? expired, bool? status);

        Task<CustomResult> GetById(int id);

        Task<CustomResult> ChangeStatus(int id);
    }
}
