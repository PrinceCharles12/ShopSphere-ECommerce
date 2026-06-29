using Microsoft.AspNetCore.Mvc;
using ShopSphereMVC.Services;
using ShopSphereMVC.Models;

namespace ShopSphereMVC.Controllers
{
    public class PaymentsController : Controller
    {
        private readonly PaymentService _paymentService;

        public PaymentsController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

        public async Task<IActionResult> Pay(int orderId,decimal amount)
        {
            var payment = await _paymentService.CreateOrder(amount);
            ViewBag.OrderId = orderId;
            return View(payment);
        }

        [HttpPost]
        public async Task<IActionResult> Verify(VerifyPaymentDto dto)
        {
            var result = await _paymentService.VerifyPaymentAsync(dto);

            if (result)
            {
                TempData["Success"] = "Payment Successful";
                return RedirectToAction("Index","Orders");
            }

            return Content("Payment Verification Failed");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}