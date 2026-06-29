using ECommerceAPI.Models;
using ECommerceAPI.Repositories;

namespace ECommerceAPI.Services
{
    public class WishlistService : IWishlistService
    {
        private readonly IWishlistRepository _wishlistRepository;

        public WishlistService(IWishlistRepository wishlistRepository)
        {
            _wishlistRepository = wishlistRepository;
        }

        public List<Wishlist> GetWishlist(int userId)
        {
            return _wishlistRepository.GetWishlist(userId);
        }

        public void AddToWishlist(int userId, int productId)
        {
            _wishlistRepository.AddToWishlist(new Wishlist
            {
                UserId = userId,
                ProductId = productId
            });
        }

        public bool RemoveFromWishlist(int userId, int productId)
        {
            return _wishlistRepository.RemoveFromWishlist(userId, productId);
        }
    }
}