using Microsoft.AspNetCore.Mvc;
using ShopSphereMVC.Services;

namespace ShopSphereMVC.Controllers
{
    public class AdminOrdersController : Controller
    {
        private readonly OrderService _orderService;

        public AdminOrdersController(OrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index","Home");
            }

            var orders = await _orderService.GetAllOrdersAsync();
            return View(orders);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateStatus(int id,string status)
        {
            if (!IsAdmin())
            {
                return RedirectToAction("Index","Home");
            }

            await _orderService.UpdateOrderStatusAsync(id, status);

            return RedirectToAction(nameof(Index));
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Role") == "Admin";
        }
    }
}