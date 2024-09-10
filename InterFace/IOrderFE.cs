using Microsoft.AspNetCore.Mvc;
using Project_sem3.Models;

namespace Project_sem3.InterFace
{
    public interface IOrderFE
    {
        Task<CustomResult> Create(string email,PaymentRequest a);

        Task<IEnumerable<Order>> Get(string email);

        Task<Order> GetOrderDetails(string idOrder);


        Task<Order> UpdateStatusOrder(string idOrder);
    }
}
