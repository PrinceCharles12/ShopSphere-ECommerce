using ECommerceAPI.Models;
using ECommerceAPI.Repositories;

namespace ECommerceAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;

        public ProductService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public List<Product> GetAllProducts()
        {
            return _productRepository.GetAll();
        }

        public Product? GetProductById(int id)
        {
            return _productRepository.GetById(id);
        }

        public Product AddProduct(Product product)
        {
            return _productRepository.Add(product);
        }

        public Product? UpdateProduct(Product product)
        {
            return _productRepository.Update(product);
        }

        public bool DeleteProduct(int id)
        {
            return _productRepository.Delete(id);
        }
    }
}