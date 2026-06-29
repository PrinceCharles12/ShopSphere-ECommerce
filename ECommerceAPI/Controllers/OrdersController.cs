using AutoMapper;
using ECommerceAPI.Data;
using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using ECommerceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;
        private readonly AppDbContext _context;

        public OrdersController(IOrderService orderService,ICartService cartService,IMapper mapper, AppDbContext context)
        {
            _orderService = orderService;
            _cartService = cartService;
            _mapper = mapper;
            _context = context;
        }

        [HttpPost("checkout")]
        public IActionResult Checkout()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);;
            var cartItems = _cartService.GetUserCart(userId);

            if (!cartItems.Any())
            {
                return BadRequest("Cart is empty");
            }

            foreach (var item in cartItems)
            {
                if (item.Product != null)
                {
                    item.Product.Stock -= item.Quantity;
                }
            }

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                Status = "Placed",

                TotalAmount = cartItems.Sum(x => x.Product!.Price * x.Quantity),

                OrderItems = cartItems.Select(x => new OrderItem
                    {
                        ProductId = x.ProductId,
                        Quantity = x.Quantity,
                        Price = x.Product!.Price
                    }).ToList()
            };

            _orderService.CreateOrder(order);
            _cartService.ClearCart(userId);

            return Ok(new
            {
                order.Id, order.UserId, order.OrderDate, order.TotalAmount, order.Status
            });
        }

        [HttpGet]
        public IActionResult GetMyOrders()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var orders = _orderService.GetOrdersByUser(userId);

            return Ok(orders.Select(o => new
            {
                o.Id,
                o.OrderDate,
                o.TotalAmount,
                o.Status,
                o.DeliveryStatus
            }));
        }

        [HttpGet("{id}")]
        public IActionResult GetOrderById(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);;
            var orders = _orderService.GetOrdersByUser(userId);
            var order = orders.FirstOrDefault(o => o.Id == id);

            if (order == null)
            {
                return NotFound();
            }

            return Ok(new
            {
                order.Id, order.UserId, order.OrderDate, order.TotalAmount, order.Status
            });
        }

        [HttpGet("all")]
        public IActionResult GetAllOrders()
        {
            var orders = _context.Orders.Select(o => new
            {
                o.Id,
                o.UserId,
                CustomerName = o.User != null ? o.User.Name : "",
                CustomerEmail = o.User != null ? o.User.Email : "",
                o.OrderDate,
                o.TotalAmount,
                o.Status,
                o.DeliveryStatus
            }).ToList();

            return Ok(orders);
        }

        [HttpPut("{id}/status")]
        public IActionResult UpdateStatus(int id,[FromBody] string status)
        {
            _orderService.UpdateOrderStatus(id,status);

            return Ok(new
            {
                Message = "Status Updated"
            });
        }

        [HttpPut("delivery-status")]
        public IActionResult UpdateDeliveryStatus(int orderId,string deliveryStatus)
        {
            var order = _context.Orders.Find(orderId);

            if(order == null)
                return NotFound();

            order.DeliveryStatus = deliveryStatus;

            _context.SaveChanges();

            return Ok(order);
        }
    }
}