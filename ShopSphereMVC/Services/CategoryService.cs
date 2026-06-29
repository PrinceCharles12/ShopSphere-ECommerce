using Newtonsoft.Json;
using ShopSphereMVC.Models;
using System.Text;

namespace ShopSphereMVC.Services
{
    public class CategoryService
    {
        private readonly HttpClient _httpClient;

        public CategoryService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            var response = await _httpClient.GetAsync("Categories");

            if (!response.IsSuccessStatusCode)
            {
                return new List<Category>();
            }

            var json = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<List<Category>>(json) ?? new List<Category>();
        }

        public async Task<bool> CreateCategoryAsync(Category category)
        {
            var json = JsonConvert.SerializeObject(category);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("Categories",content);

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> DeleteCategoryAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"Categories/{id}");
            return response.IsSuccessStatusCode;
        }
    }
}