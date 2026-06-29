using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly AppDbContext _context;

        public ProductRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Product> GetAll()
        {
            return _context.Products.Include(p => p.Reviews).ToList();
        }

        public Product? GetById(int id)
        {
            return _context.Products.Find(id);
        }

        public Product Add(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();

            return product;
        }

        public Product? Update(Product product)
        {
            _context.Products.Update(product);
            _context.SaveChanges();

            return product;
        }

        public bool Delete(int id)
        {
            var product = _context.Products.Find(id);

            if (product == null)
            {
                return false;
            }

            _context.Products.Remove(product);
            _context.SaveChanges();

            return true;
        }
    }
}