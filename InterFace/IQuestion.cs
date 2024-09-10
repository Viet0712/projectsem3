using Project_sem3.Models;

namespace Project_sem3.InterFace
{
    public interface IQuestion
    {
        Task<CustomResult> GetAll();
        Task<CustomResult> GetById(int id);

        Task<CustomResult> SendFeedBack(Question_Reply q);

        Task<CustomResult> UpdateFeedBack(Question_Reply q);
    }
}
