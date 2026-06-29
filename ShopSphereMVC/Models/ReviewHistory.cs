namespace ShopSphereMVC.Models
{
    public class ReviewHistory
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int Rating { get; set; }

        public string Comment { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
    }
}