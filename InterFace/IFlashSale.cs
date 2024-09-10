using Project_sem3.Models;

namespace Project_sem3.InterFace
{
    public interface IFlashSale
    {
        Task<CustomResult> GetAll();

        Task<CustomResult> GetAllTrue();

        Task<CustomResult> Create(Flash_Sale e);

        Task<CustomResult> Update(Flash_Sale e);

        Task<CustomResult> Search(string? name, bool? expired, bool? status);

        Task<CustomResult> GetById(int id);

        Task<CustomResult> ChangeStatus(int id);

       
    }
}
