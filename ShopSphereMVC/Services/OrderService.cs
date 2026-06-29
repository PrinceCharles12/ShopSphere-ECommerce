using Newtonsoft.Json;
using ShopSphereMVC.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ShopSphereMVC.Services
{
    public class OrderService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public OrderService(HttpClient httpClient,IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;

            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer",token);
            }
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            var response = await _httpClient.GetAsync("Orders");

            if (!response.IsSuccessStatusCode)
                return new List<Order>();

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Order>>(json) ?? new List<Order>();
        }

        public async Task<List<AdminOrder>> GetAllOrdersAsync()
        {
            var response = await _httpClient.GetAsync("Orders/all");

            if (!response.IsSuccessStatusCode)
                return new List<AdminOrder>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<AdminOrder>>(json) ?? new List<AdminOrder>();
        }

        public async Task<bool> UpdateOrderStatusAsync(int orderId,string status)
        {
            var json = JsonConvert.SerializeObject(status);
            var content = new StringContent(json,Encoding.UTF8,"application/json");
            var response = await _httpClient.PutAsync($"Orders/{orderId}/status",content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> UpdateDeliveryStatusAsync(int orderId,string deliveryStatus)
        {
            var response = await _httpClient.PutAsync(
                $"Orders/delivery-status?orderId={orderId}&deliveryStatus={deliveryStatus}",
                null);

            return response.IsSuccessStatusCode;
        }
    }
}