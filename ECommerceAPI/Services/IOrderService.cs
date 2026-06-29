using ECommerceAPI.Models;

namespace ECommerceAPI.Services
{
    public interface IOrderService
    {
        Order CreateOrder(Order order);
        List<Order> GetOrdersByUser(int userId);
        List<Order> GetAllOrders();
        void UpdateOrderStatus(int orderId, string status);

    }
    
}