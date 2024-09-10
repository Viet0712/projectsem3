using Project_sem3.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project_sem3.InterFace
{
  

    public interface IUserFE
    {
        Task<int> UpdateInfo(User user);
        Task<int> UpdateCard(User user);
        Task<int> UpdatePassword(User user);
    }
}
