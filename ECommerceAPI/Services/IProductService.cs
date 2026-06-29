using ECommerceAPI.Models;

namespace ECommerceAPI.Services
{
    public interface IProductService
    {
        List<Product> GetAllProducts();

        Product? GetProductById(int id);

        Product AddProduct(Product product);

        Product? UpdateProduct(Product product);

        bool DeleteProduct(int id);
    }
}