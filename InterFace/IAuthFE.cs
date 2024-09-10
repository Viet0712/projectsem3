using Project_sem3.Models;

namespace Project_sem3.InterFace
{

    public interface IAuthFE
    {
        Task<User> Login(AccountLogin acc);

        Task<CustomResult> LoginGoogle(User user);
        Task<int> Register(User user);
        Task<CustomResult> ForgotPassword(string email);
        Task<User> Get(string token);

        Task<int> Verify(string email,string timer);

    }
}
