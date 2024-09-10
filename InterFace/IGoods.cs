using Project_sem3.Models;

namespace Project_sem3.InterFace
{
    public interface IGoods
    {
        Task<CustomResult> GetAll(int id);

        Task<CustomResult> Create(Goods g);
        Task<CustomResult> Update(Goods g);

        Task<CustomResult> GetById(int id);

        Task<CustomResult> ChangeStatus(int id);
    }
}
