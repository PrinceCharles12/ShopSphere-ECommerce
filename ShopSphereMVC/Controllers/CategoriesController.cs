using Microsoft.AspNetCore.Mvc;
using ShopSphereMVC.Models;
using ShopSphereMVC.Services;

namespace ShopSphereMVC.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly CategoryService _categoryService;

        public CategoriesController(CategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetCategoriesAsync();

            return View(categories);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            var success = await _categoryService.CreateCategoryAsync(category);

            if(success)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(category);
        }

        public async Task<IActionResult> Delete(int id)
        {
            await _categoryService.DeleteCategoryAsync(id);

            return RedirectToAction(nameof(Index));
        }
    }
}