namespace ShopSphereMVC.Models
{
    public class VerifyPaymentDto
    {
        public int OrderId { get; set; }

        public decimal Amount { get; set; }

        public string RazorpayOrderId { get; set; } = "";

        public string RazorpayPaymentId { get; set; } = "";

        public string RazorpaySignature { get; set; } = "";
    }
}