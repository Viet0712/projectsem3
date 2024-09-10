using Project_sem3.Models;

namespace Project_sem3.InterFace
{
    public interface IRate
    {
        Task<CustomResult> GetAll(int storeid);
        Task<CustomResult> GetById(int id);

        Task<CustomResult> SendFeedBack(Rate_Reply r);

        Task<CustomResult> UpdateFeedBack(Rate_Reply r);
    }
}
