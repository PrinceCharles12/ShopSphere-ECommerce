using Microsoft.AspNetCore.Mvc;
using ShopSphereMVC.Services;

namespace ShopSphereMVC.Controllers
{
    public class OrdersController : Controller
    {
        private readonly OrderService _orderService;

        public OrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetOrdersAsync();
            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateDeliveryStatus(int orderId,string deliveryStatus)
        {
            await _orderService.UpdateDeliveryStatusAsync(orderId,deliveryStatus);

            return RedirectToAction(nameof(ManageOrders));
        }

        public async Task<IActionResult> ManageOrders()
        {
            var orders = await _orderService.GetAllOrdersAsync();

            return View(orders);
        }
    }
}