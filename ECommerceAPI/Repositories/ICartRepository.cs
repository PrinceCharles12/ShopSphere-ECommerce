using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories
{
    public interface ICartRepository
    {
        List<CartItem> GetUserCart(int userId);
        CartItem AddToCart(CartItem cartItem);
        bool RemoveFromCart(int cartItemId);
        void ClearCart(int userId);
    }
}