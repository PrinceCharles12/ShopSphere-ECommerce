using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using ShopSphereMVC.Models;
using System.Net.Http.Headers;
using System.Text;

namespace ShopSphereMVC.Services
{
    public class UserService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
        }

        private void AddToken()
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");

            if (!string.IsNullOrEmpty(token))
            {
                _httpClient.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue("Bearer", token);
            }
        }

        public async Task<UserProfile?> GetProfileAsync(string email)
        {
            var response = await _httpClient.GetAsync($"Auth/profile/{email}");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<UserProfile>(json);
        }

        public async Task<bool> UpdateProfileAsync(string email, EditProfileViewModel model)
        {
            var json = JsonConvert.SerializeObject(model);

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PutAsync($"Auth/profile/{email}",content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> ChangePasswordAsync(string email, ChangePasswordViewModel model)
        {
            var json = JsonConvert.SerializeObject(new
            {
                CurrentPassword = model.CurrentPassword,
                NewPassword = model.NewPassword
            });

            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"Auth/change-password/{email}", content);

            return response.IsSuccessStatusCode;
        }
    }
}