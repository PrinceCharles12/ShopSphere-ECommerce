using ECommerceAPI.Models;

namespace ECommerceAPI.Services
{
    public interface IWishlistService
    {
        List<Wishlist> GetWishlist(int userId);

        void AddToWishlist(int userId,int productId);

        bool RemoveFromWishlist(int userId,int productId);
    }
}