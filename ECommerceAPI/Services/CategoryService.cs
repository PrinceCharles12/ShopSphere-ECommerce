using ECommerceAPI.Models;
using ECommerceAPI.Repositories;

namespace ECommerceAPI.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        public CategoryService(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public List<Category> GetAllCategories()
        {
            return _categoryRepository.GetAll();
        }

        public Category? GetCategoryById(int id)
        {
            return _categoryRepository.GetById(id);
        }

        public void AddCategory(Category category)
        {
            _categoryRepository.Add(category);
        }

        public void UpdateCategory(Category category)
        {
            _categoryRepository.Update(category);
        }

        public bool DeleteCategory(int id)
        {
            return _categoryRepository.Delete(id);
        }
    }
}