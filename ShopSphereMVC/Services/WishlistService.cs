using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ShopSphereMVC.Models;
using System.Net.Http.Headers;

namespace ShopSphereMVC.Services
{
    public class WishlistService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WishlistService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;

            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<List<WishlistItem>> GetWishlistAsync()
        {
            var response = await _httpClient.GetAsync("Wishlist");

            if (!response.IsSuccessStatusCode)
                return new List<WishlistItem>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<WishlistItem>>(json) ?? new List<WishlistItem>();
        }

        public async Task<bool> AddToWishlistAsync(int productId)
        {
            var response = await _httpClient.PostAsync($"Wishlist/{productId}", null);
            return response.IsSuccessStatusCode;
        }

        public async Task<bool> RemoveFromWishlistAsync(int productId)
        {
            var response = await _httpClient.DeleteAsync($"Wishlist/{productId}");
            return response.IsSuccessStatusCode;
        }
    }
}