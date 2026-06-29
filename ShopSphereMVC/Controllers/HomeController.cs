using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ShopSphereMVC.Models;
using ShopSphereMVC.Services;

namespace ShopSphereMVC.Controllers
{
    public class HomeController : BaseController
    {
        private readonly ProductService _productService;

        public HomeController(ProductService productService, WishlistService wishlistService)
            : base(wishlistService)
        {
            _productService = productService;
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetProductsAsync();
            var topProducts = products.OrderByDescending(p => p.AverageRating).Take(4).ToList();
            return View(topProducts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel
            {
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier
            });
        }
    }
}