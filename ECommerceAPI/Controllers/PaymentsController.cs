using AutoMapper;
using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using ECommerceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ECommerceAPI.Data;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private readonly AppDbContext _context;
        public PaymentsController(IPaymentService paymentService, IMapper mapper,
            IConfiguration configuration,AppDbContext context)
        {
            _paymentService = paymentService;
            _mapper = mapper;
            _configuration = configuration;
            _context = context;
        }

        [HttpPost]
        public IActionResult CreatePayment(int orderId, decimal amount)
        {
            var payment = new ECommerceAPI.Models.Payment
            {
                OrderId = orderId,
                Amount = amount,
                PaymentMethod = "Demo Payment",
                Status = "Success",
                PaymentDate = DateTime.Now
            };

            _paymentService.CreatePayment(payment);

            return Ok(payment);
        }

        [HttpGet]
        public IActionResult GetMyPayments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var payments = _paymentService.GetPaymentsByUser(int.Parse(userId));
            var result = _mapper.Map<List<PaymentResponseDto>>(payments);

            return Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetPaymentById(int id)
        {
            var payment = _paymentService.GetPaymentById(id);

            if (payment == null)
            {
                return NotFound();
            }

            return Ok(payment);
        }

        [AllowAnonymous]
        [HttpPost("create-order")]
        public IActionResult CreateRazorpayOrder(CreatePaymentDto request)
        {
            var key = _configuration["Razorpay:Key"];
            var secret = _configuration["Razorpay:Secret"];

            Razorpay.Api.RazorpayClient client = new Razorpay.Api.RazorpayClient(key, secret);

            Dictionary<string, object> options = new Dictionary<string, object>();

            options.Add("amount", request.Amount * 100);
            options.Add("currency", "INR");
            options.Add("receipt", Guid.NewGuid().ToString());

            Razorpay.Api.Order order = client.Order.Create(options);

            return Ok(new
            {
                OrderId = order.Attributes["id"]?.ToString(),
                Amount = order.Attributes["amount"]?.ToString(),
                Currency = order.Attributes["currency"]?.ToString(),
                Receipt = order.Attributes["receipt"]?.ToString(),
                Status = order.Attributes["status"]?.ToString()
            });
        }

        [HttpPost("verify")]
        public IActionResult VerifyPayment(VerifyPaymentDto dto)
        {   
            var secret = _configuration["Razorpay:Secret"];
            var payload = dto.RazorpayOrderId + "|" + dto.RazorpayPaymentId;
            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(secret!));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
            var generatedSignature = BitConverter.ToString(hash).Replace("-", "").ToLower();

            if (generatedSignature != dto.RazorpaySignature)
            {
                return BadRequest("Payment Verification Failed");
            }

            var payment = new Payment
            {
                OrderId = dto.OrderId,
                Amount = dto.Amount,
                PaymentMethod = "Razorpay",
                Status = "Success",
                PaymentDate = DateTime.Now,
                RazorpayOrderId = dto.RazorpayOrderId,
                RazorpayPaymentId = dto.RazorpayPaymentId
            };

            _paymentService.CreatePayment(payment);
            var order = _context.Orders.Find(dto.OrderId);

            if (order != null)
            {
                order.Status = "Paid";
                _context.SaveChanges();
            }

            return Ok(new
            {
                Message = "Payment Verified Successfully",
                OrderStatus = "Paid"
            });
        }
    }
}