using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly AppDbContext _context;
        public CartRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<CartItem> GetUserCart(int userId)
        {
            return _context.CartItems
                .Include(c => c.Product)
                .Where(c => c.UserId == userId)
                .ToList();
        }

        public CartItem AddToCart(CartItem cartItem)
        {
            _context.CartItems.Add(cartItem);
            _context.SaveChanges();

            return cartItem;
        }

        public bool RemoveFromCart(int cartItemId)
        {
            var item = _context.CartItems.Find(cartItemId);

            if (item == null)
            {
                return false;
            }

            _context.CartItems.Remove(item);
            _context.SaveChanges();

            return true;
        }
        public void ClearCart(int userId)
        {
            var items = _context.CartItems.Where(c => c.UserId == userId).ToList();
            _context.CartItems.RemoveRange(items);
            _context.SaveChanges();
        }
    }
}