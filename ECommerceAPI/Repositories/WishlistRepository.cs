using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Repositories
{
    public class WishlistRepository : IWishlistRepository
    {
        private readonly AppDbContext _context;
        public WishlistRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Wishlist> GetWishlist(int userId)
        {
            return _context.Wishlists.Include(w => w.Product).Where(w => w.UserId == userId).ToList();
        }

        public void AddToWishlist(Wishlist wishlist)
        {
            _context.Wishlists.Add(wishlist);
            _context.SaveChanges();
        }

        public bool RemoveFromWishlist(int userId,int productId)
        {
            var item = _context.Wishlists.FirstOrDefault(w => w.UserId == userId && w.ProductId == productId);
 
            if(item == null)
                return false;

            _context.Wishlists.Remove(item);
            _context.SaveChanges();
            return true;
        }
    }
}