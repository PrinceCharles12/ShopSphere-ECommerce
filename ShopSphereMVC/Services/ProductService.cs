using Newtonsoft.Json;
using ShopSphereMVC.Models;
using System.Text;

namespace ShopSphereMVC.Services
{
    public class ProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Product>> GetProductsAsync()
        {
            var response = await _httpClient.GetAsync("Products");

            if (!response.IsSuccessStatusCode)
                return new List<Product>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Product>>(json) ?? new List<Product>();
        }

        public async Task<bool> CreateProductAsync(Product product)
        {
            var json = JsonConvert.SerializeObject(product);
            var content = new StringContent(json,Encoding.UTF8,"application/json");
            var response = await _httpClient.PostAsync("Products",content);

            return response.IsSuccessStatusCode;
        }

        public async Task<Product?> GetProductByIdAsync(int id)
        {
            var response = await _httpClient.GetAsync($"Products/{id}");

            if (!response.IsSuccessStatusCode)
                return null;

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<Product>(json);
        }

        public async Task<bool> UpdateProductAsync(Product product)
        {
            var json = JsonConvert.SerializeObject(product);
            var content = new StringContent(json,Encoding.UTF8,"application/json");
            var response = await _httpClient.PutAsync($"Products/{product.Id}",content);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteProductAsync(int id)
        {
            var response = await _httpClient.DeleteAsync($"Products/{id}");
            return response.IsSuccessStatusCode;
        }

        public async Task<List<Product>> SearchProductsAsync(string keyword)
        {
            var response = await _httpClient.GetAsync($"Products/search?keyword={keyword}");

            if (!response.IsSuccessStatusCode)
                return new List<Product>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Product>>(json) ?? new List<Product>();
        }

        public async Task<List<Product>> GetTopRatedProductsAsync()
        {
            var response = await _httpClient.GetAsync("Products/toprated");

            if (!response.IsSuccessStatusCode)
                return new List<Product>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Product>>(json) ?? new List<Product>();
        }

        public async Task<List<Product>> SortProductsAsync(string sortBy)
        {
            var response = await _httpClient.GetAsync($"Products/sort?sortBy={sortBy}");

            if (!response.IsSuccessStatusCode)
                return new List<Product>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Product>>(json) ?? new List<Product>();
        }

        public async Task<List<Product>> AdvancedFilterAsync(int? categoryId,decimal? minPrice,decimal? maxPrice)
        {
            var response = await _httpClient.GetAsync(
                $"Products/advancedfilter?categoryId={categoryId}&minPrice={minPrice}&maxPrice={maxPrice}");

            if (!response.IsSuccessStatusCode)
                return new List<Product>();

            var json = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<List<Product>>(json)
                ?? new List<Product>();
        }
    }
}