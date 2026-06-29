namespace ShopSphereMVC.Models
{
    public class PaymentResponse
    {
        public string OrderId { get; set; } = string.Empty;

        public string Amount { get; set; } = string.Empty;

        public string Currency { get; set; } = string.Empty;

        public string Receipt { get; set; } = string.Empty;

        public string Status { get; set; } = string.Empty;
    }
}