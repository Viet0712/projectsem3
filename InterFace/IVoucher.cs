using Project_sem3.Models;

namespace Project_sem3.InterFace
{
    public interface IVoucher
    {
        Task<CustomResult> GetAll();
        Task<CustomResult> GetAllTrue();
        Task<CustomResult> Create(Voucher e);

        Task<CustomResult> Update(Voucher e);

        Task<CustomResult> Search(string? name, bool? expired, bool? status , string? Type);

        Task<CustomResult> GetById(int id);

        Task<CustomResult> ChangeStatus(int id);
    }
}
