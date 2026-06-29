using Microsoft.AspNetCore.Mvc;
using ShopSphereMVC.Models;
using ShopSphereMVC.Services;

namespace ShopSphereMVC.Controllers
{
    public class CartController : Controller
    {
        private readonly CartService _cartService;

        public CartController(CartService cartService)
        {
            _cartService = cartService;
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity)
        {
            var dto = new CartItemCreateDto
            {
                ProductId = productId,
                Quantity = quantity
            };

            await _cartService.AddToCartAsync(dto);

            return RedirectToAction("Index", "Products");
        }

        public async Task<IActionResult> Index()
        {
            var cartItems = await _cartService.GetCartAsync();
            return View(cartItems);
        }  

        [HttpPost]
        public async Task<IActionResult> Remove(int id)
        {
            await _cartService.RemoveFromCartAsync(id);
            return RedirectToAction(nameof(Index));
        } 

        [HttpPost]
        public async Task<IActionResult> Checkout()
        {
            var success = await _cartService.CheckoutAsync();

            if (success)
            {
                TempData["Message"] = "Order Created Successfully";
            }
            else
            {
                TempData["Message"] = "Checkout Failed";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}