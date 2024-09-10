using Project_sem3.Models;

namespace Project_sem3.InterFace
{
    public interface IShipping
    {
        Task<CustomResult> GetAllShipping();

        Task<CustomResult> CreateShipping(Shipping shipping);

        Task<CustomResult> UpdateShipping(Shipping shipping);


        Task<CustomResult> SearchByName(string name);

        Task<CustomResult> ChangeStatus(int id);
    }
}
