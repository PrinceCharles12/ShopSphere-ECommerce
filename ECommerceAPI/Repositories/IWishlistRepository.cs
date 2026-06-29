using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories
{
    public interface IWishlistRepository
    {
        List<Wishlist> GetWishlist(int userId);
        void AddToWishlist(Wishlist wishlist);
        bool RemoveFromWishlist(int userId,int productId);
    }
}