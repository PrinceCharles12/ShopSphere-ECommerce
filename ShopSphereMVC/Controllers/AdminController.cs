using Microsoft.AspNetCore.Mvc;
using ShopSphereMVC.Services;

namespace ShopSphereMVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        private readonly OrderService _orderService;

        public AdminController(ProductService productService, CategoryService categoryService,
            OrderService orderService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _orderService = orderService;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Role") == "Admin";
        }

        public async Task<IActionResult> Dashboard()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index", "Home");
            }

            var products = await _productService.GetProductsAsync();
            var categories = await _categoryService.GetCategoriesAsync();
            var orders = await _orderService.GetAllOrdersAsync();

            ViewBag.TotalProducts = products.Count;
            ViewBag.TotalCategories = categories.Count;
            ViewBag.TotalOrders = orders.Count;
            ViewBag.TotalRevenue = orders.Sum(x => x.TotalAmount);

            return View();
        }
    }
}