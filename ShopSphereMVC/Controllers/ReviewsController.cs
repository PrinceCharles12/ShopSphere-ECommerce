using Microsoft.AspNetCore.Mvc;
using ShopSphereMVC.Models;
using ShopSphereMVC.Services;

namespace ShopSphereMVC.Controllers
{
    public class ReviewsController : Controller
    {
        private readonly ReviewService _reviewService;

        public ReviewsController(ReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost]
        public async Task<IActionResult> Add(int productId, int rating, string comment)
        {
            var review = new Review
            {
                ProductId = productId,
                Rating = rating,
                Comment = comment,
                CreatedAt = DateTime.Now
            };

            var success = await _reviewService.AddReviewAsync(review);

            if (success)
            {
                TempData["Message"] = "Review added successfully.";
            }
            else
            {
                TempData["Message"] = "Unable to add review.";
            }

            return RedirectToAction("Details","Products",new { id = productId });
        }
        public async Task<IActionResult> History()
        {
            var reviews = await _reviewService.GetMyReviewsAsync();

            return View(reviews);
        }
    }
}