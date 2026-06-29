using ECommerceAPI.Models;
using ECommerceAPI.Repositories;

namespace ECommerceAPI.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public List<CartItem> GetUserCart(int userId)
        {
            return _cartRepository.GetUserCart(userId);
        }

        public CartItem AddToCart(CartItem cartItem)
        {
            return _cartRepository.AddToCart(cartItem);
        }

        public bool RemoveFromCart(int cartItemId)
        {
            return _cartRepository.RemoveFromCart(cartItemId);
        }

        public void ClearCart(int userId)
        {
            _cartRepository.ClearCart(userId);
        }
    }
}