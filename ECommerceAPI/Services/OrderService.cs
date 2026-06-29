using ECommerceAPI.Models;
using ECommerceAPI.Repositories;

namespace ECommerceAPI.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;

        public OrderService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public Order CreateOrder(Order order)
        {
            return _orderRepository.CreateOrder(order);
        }

        public List<Order> GetOrdersByUser(int userId)
        {
            return _orderRepository.GetOrdersByUser(userId);
        }

        public List<Order> GetAllOrders()
        {
            return _orderRepository.GetAllOrders();
        }

        public void UpdateOrderStatus(int orderId,string status)
        {
            _orderRepository.UpdateOrderStatus(orderId,status);
        }
    }
}