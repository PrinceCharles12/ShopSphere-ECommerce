using Newtonsoft.Json;
using ShopSphereMVC.Models;
using System.Text;

namespace ShopSphereMVC.Services
{
    public class PaymentService
    {
        private readonly HttpClient _httpClient;

        public PaymentService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<PaymentResponse?> CreateOrder(decimal amount)
        {
            var request = new PaymentRequest
            {
                Amount = amount
            };

            var json = JsonConvert.SerializeObject(request);
            var content = new StringContent(json,Encoding.UTF8,"application/json");
            var response = await _httpClient.PostAsync("Payments/create-order",content);

            if (!response.IsSuccessStatusCode)
                return null;

            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine(result);

            return JsonConvert.DeserializeObject<PaymentResponse>(result);
        }

        public async Task<bool> VerifyPaymentAsync(VerifyPaymentDto dto)
        {
            var json = JsonConvert.SerializeObject(dto);
            var content = new StringContent(json,Encoding.UTF8,"application/json");
            var response = await _httpClient.PostAsync("Payments/verify",content);

            return response.IsSuccessStatusCode;
        }
    }
}