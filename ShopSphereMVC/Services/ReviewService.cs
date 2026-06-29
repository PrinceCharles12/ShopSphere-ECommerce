using Newtonsoft.Json;
using ShopSphereMVC.Models;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text;

namespace ShopSphereMVC.Services
{
    public class ReviewService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ReviewService(HttpClient httpClient,IHttpContextAccessor httpContextAccessor)
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

        public async Task<List<Review>> GetReviewsByProductAsync(int productId)
        {
            var response = await _httpClient.GetAsync($"Reviews/{productId}");

            if (!response.IsSuccessStatusCode)
                return new List<Review>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Review>>(json) ?? new List<Review>();
        }

        public async Task<bool> AddReviewAsync(Review review)
        {
            var json = JsonConvert.SerializeObject(review);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Reviews", content);

            return response.IsSuccessStatusCode;
        }

        public async Task<List<ReviewHistory>> GetMyReviewsAsync()
        {
            var response = await _httpClient.GetAsync("Reviews/myreviews");

            if (!response.IsSuccessStatusCode)
                return new List<ReviewHistory>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<ReviewHistory>>(json) ?? new List<ReviewHistory>();
        }
    }
}