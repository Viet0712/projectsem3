using Project_sem3.Models;

namespace Project_sem3.InterFace
{
    public interface ICartFE
    {
        Task<IEnumerable<Cart>> GetCart(string email);
        Task<Cart> CreateCart(CartCreate c);
        Task<Cart> UpdateCart(Cart cart);

        Task<int> DeleteCart (int[] id);
    }
}
