using ECommerceAPI.Models;

namespace ECommerceAPI.Services
{
    public interface ICartService
    {
        List<CartItem> GetUserCart(int userId);

        CartItem AddToCart(CartItem cartItem);

        bool RemoveFromCart(int cartItemId);

        void ClearCart(int userId);
    }
}