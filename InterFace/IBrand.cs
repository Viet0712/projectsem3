using Project_sem3.Models;

namespace Project_sem3.InterFace
{
    public interface IBrand
    {
        Task<CustomResult> GetAllBrandTrue();

        Task<CustomResult> GetAllBrand();

        Task<CustomResult> CreateBrand(Brand brand);

        Task<CustomResult> UpdateBrand(Brand brand);

        Task<CustomResult> GetById(int id);
        Task<CustomResult> SearchByName(string name);

        Task<CustomResult> ChangeStatus(int id);
    }
}
