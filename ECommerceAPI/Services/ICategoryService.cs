using ECommerceAPI.Models;

namespace ECommerceAPI.Services
{
    public interface ICategoryService
    {
        List<Category> GetAllCategories();

        Category? GetCategoryById(int id);

        void AddCategory(Category category);

        void UpdateCategory(Category category);

        bool DeleteCategory(int id);
    }
}