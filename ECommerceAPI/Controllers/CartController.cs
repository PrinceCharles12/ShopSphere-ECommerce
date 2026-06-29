using AutoMapper;
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
    [Authorize]
    public class CartController : ControllerBase
    {
        private readonly ICartService _cartService;
        private readonly IMapper _mapper;

        public CartController(ICartService cartService, IMapper mapper)
        {
            _cartService = cartService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetCart()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);;
            var cartItems = _cartService.GetUserCart(userId);
            var result = _mapper.Map<List<CartItemResponseDto>>(cartItems);
            return Ok(result);
        }

        [HttpPost]
        public IActionResult AddToCart(CartItemCreateDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);;
            var cartItem = new CartItem
            {
                UserId = userId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            };
            _cartService.AddToCart(cartItem);

            return Ok(cartItem);
        }

        [HttpDelete("{id}")]
        public IActionResult RemoveFromCart(int id)
        {
            var deleted = _cartService.RemoveFromCart(id);
            if (!deleted)
            {
                return NotFound();
            }

            return Ok("Item removed from cart");
        }
    }
}