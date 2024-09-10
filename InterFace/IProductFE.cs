using Microsoft.AspNetCore.Mvc;
using Project_sem3.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Project_sem3.InterFace
{
    public interface IProductFE
    {
        Task<IEnumerable<Product>> GetAllProducts(int storeid);
        Task<IEnumerable<Product>> GetProductsCategory(int storeid,string grant, int id);
        Task<Product> GetProductsById(int storeid,string id);
        Task<IEnumerable<Product>> GetFlashSale(int storeid);

        Task<IEnumerable<Product>> GetProductSearch();

    }
}