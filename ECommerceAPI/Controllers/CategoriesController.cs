using ECommerceAPI.Models;
using ECommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(_categoryService.GetAllCategories());
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var category = _categoryService.GetCategoryById(id);

            if (category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            category.Id=0;
            _categoryService.AddCategory(category);

            return Ok(category);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id,Category category)
        {
            category.Id = id;
            _categoryService.UpdateCategory(category);

            return Ok(category);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var deleted = _categoryService.DeleteCategory(id);

            if (!deleted)
            {
                return NotFound();
            }

            return Ok("Category Deleted Successfully");
        }
    }
}