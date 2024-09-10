using Project_sem3.Models;

namespace Project_sem3.InterFace
{
    public interface IInteractFE
    {
        Task<int> CreateQuestion(Question question);

        Task<int> CreateRate(ListRate list);
    }
}
