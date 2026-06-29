using ECommerceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WishlistController : ControllerBase
    {
        private readonly IWishlistService _wishlistService;
        public WishlistController(IWishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        [HttpGet]
        public IActionResult GetWishlist()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            return Ok(_wishlistService.GetWishlist(userId));
        }

        [HttpPost("{productId}")]
        public IActionResult AddToWishlist(int productId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            _wishlistService.AddToWishlist(userId, productId);
            return Ok("Added To Wishlist");
        }

        [HttpDelete("{productId}")]
        public IActionResult RemoveFromWishlist(int productId)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            _wishlistService.RemoveFromWishlist(userId,productId);
            return Ok("Removed From Wishlist");
        }
    }
}