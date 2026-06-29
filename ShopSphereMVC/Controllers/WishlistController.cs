using Microsoft.AspNetCore.Mvc;
using ShopSphereMVC.Services;

namespace ShopSphereMVC.Controllers
{
    public class WishlistController : Controller
    {
        private readonly WishlistService _wishlistService;
        private readonly CartService _cartService;

        public WishlistController(WishlistService wishlistService, CartService cartService)
        {
            _wishlistService = wishlistService;
            _cartService = cartService;
        }

        public async Task<IActionResult> Index()
        {
            var items = await _wishlistService.GetWishlistAsync();
            return View(items);
        }

        [HttpPost]
        public async Task<IActionResult> Add(int productId)
        {
            var items = await _wishlistService.GetWishlistAsync();

            bool exists = items.Any(x => x.ProductId == productId);

            if (exists)
            {
                TempData["Message"] = "Product already exists in wishlist";
                return RedirectToAction("Index", "Products");
            }

            await _wishlistService.AddToWishlistAsync(productId);
            TempData["Message"] = "Added to wishlist successfully";
            return RedirectToAction("Index", "Products");
        }

        public async Task<IActionResult> Remove(int productId)
        {
            await _wishlistService.RemoveFromWishlistAsync(productId);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> MoveToCart(int productId)
        {
            await _cartService.AddToCart(productId, 1);
            await _wishlistService.RemoveFromWishlistAsync(productId);

            TempData["Message"] = "Item moved to cart successfully";
            return RedirectToAction(nameof(Index));
        }
    }
}