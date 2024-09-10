using Project_sem3.Models;

namespace Project_sem3.InterFace
{
    public interface IStore
    {
        Task<CustomResult> GetAllStore();

        Task<CustomResult> CreateStore(Store store);

        Task<CustomResult> UpdateStore(Store store);

        Task<CustomResult> GetById(int id);
        Task<CustomResult> Search(string? address , string? city , string? district , bool? status );
        Task<CustomResult> DeleteStore(int id);

        Task<CustomResult> ChangeStatus(int id);
    }
}
