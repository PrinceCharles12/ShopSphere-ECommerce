using Newtonsoft.Json;
using ShopSphereMVC.Models;
using System.Text;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;

namespace ShopSphereMVC.Services
{
    public class CartService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public CartService(HttpClient httpClient,IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;

            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
            }
        }

        public async Task<bool> AddToCartAsync(CartItemCreateDto dto)
        {
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Cart", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddToCart(int productId, int quantity)
        {
            var dto = new CartItemCreateDto
            {
                ProductId = productId,
                Quantity = quantity
            };

            return await AddToCartAsync(dto);
        }
        public async Task<List<CartItem>> GetCartAsync()
        {
            var response = await _httpClient.GetAsync("Cart");

            if (!response.IsSuccessStatusCode)
                return new List<CartItem>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<CartItem>>(json) ?? new List<CartItem>();
        }

        public async Task<bool> RemoveFromCartAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"Cart/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> CheckoutAsync()
        {
            var response = await _httpClient.PostAsync("Orders/checkout", null);
            return response.IsSuccessStatusCode;
        }
    }
    
}