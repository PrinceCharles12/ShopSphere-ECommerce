using ECommerceAPI.Data;
using ECommerceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public Order CreateOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();

            return order;
        }

        public List<Order> GetOrdersByUser(int userId)
        {
            return _context.Orders.Include(o => o.OrderItems).Where(o => o.UserId == userId).ToList();
        }

        public List<Order> GetAllOrders()
        {
            return _context.Orders.Include(o => o.OrderItems).ToList();
        }

        public void UpdateOrderStatus(int orderId,string status)
        {
            var order = _context.Orders.FirstOrDefault(o => o.Id == orderId);

            if (order != null)
            {
                order.Status = status;
                _context.SaveChanges();
            }
        }
    }
}