using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
        Category? GetById(int id);
        void Add(Category category);
        void Update(Category category);
        bool Delete(int id);
    }
}