using Project_sem3.Models;
namespace Project_sem3.InterFace
{
    public interface IPermission
    {
        Task<CustomResult> getAll();

        Task<CustomResult> getByIdAdmin(int id);

        Task<CustomResult> Update(int Id , string type);
    }
}
