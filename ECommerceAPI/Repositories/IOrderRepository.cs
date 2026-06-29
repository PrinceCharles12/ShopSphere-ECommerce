using ECommerceAPI.Models;

namespace ECommerceAPI.Repositories
{
    public interface IOrderRepository
    {
        Order CreateOrder(Order order);
        List<Order> GetOrdersByUser(int userId);
        List<Order> GetAllOrders();
        void UpdateOrderStatus(int orderId, string status);
    }
}