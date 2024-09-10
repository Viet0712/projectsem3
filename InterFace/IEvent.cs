using Project_sem3.Models;

namespace Project_sem3.InterFace
{
    public interface IEvent
    {
        Task<CustomResult> Get();
        Task<CustomResult> RemoveDiscount(int id);

        Task<CustomResult> SetDiscount(List<int> PropertiesId,int DiscountId);

        Task<CustomResult> RemoveFlashSale(int id);

        Task<CustomResult> SetFlashSale(List<int> PropertiesId, int FlashSaleId);
    }
}
