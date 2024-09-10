using Project_sem3.Models;

namespace Project_sem3.InterFace
{
    public interface IStoreFE
    {

        Task<IEnumerable<Store>> Get();
    }
}
