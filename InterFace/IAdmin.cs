using Project_sem3.Models;

namespace Project_sem3.InterFace
{
    public interface IAdmin
    {
        Task<CustomResult> GetAllAdmin();

        Task<CustomResult> CreateAdmin(Admin e);

        Task<CustomResult> UpdateAdmin(Admin e);

        Task<CustomResult> Search(string? name , string? role , bool? status);

        Task<CustomResult> GetById(int id);

        Task<CustomResult> ChangeStatus(int id);
        Task<CustomResult> LogOut(string email);
        Task<CustomResult> Verify(string email,string timeCreate);

        Task<CustomResult> ForgotPassword(string email);
    }
}
