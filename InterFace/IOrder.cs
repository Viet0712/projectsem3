using Project_sem3.Models;

namespace Project_sem3.InterFace
{
    public interface IOrder
    {
        Task<CustomResult> GetByStore(int storeId,DateTime month);
        Task<CustomResult> OrderDetail(string OrderId);
        Task<CustomResult> ChangeStatus(string id);
    }
}
