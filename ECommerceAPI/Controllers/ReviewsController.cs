using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ECommerceAPI.Data;
using ECommerceAPI.Models;
using System.Security.Claims;
using ECommerceAPI.DTOs;
using Microsoft.AspNetCore.Authorization;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly AppDbContext _context;
        public ReviewsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult AddReview(ReviewCreateDto dto)
        {
            var userIdClaim = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

            if (userIdClaim == null)
            {
                return Unauthorized("Please login first.");
            }

            int userId = int.Parse(userIdClaim.Value);
            var review = new Review
            {
                ProductId = dto.ProductId,
                UserId = userId,
                Rating = dto.Rating,
                Comment = dto.Comment,
                CreatedAt = DateTime.Now
            };

            _context.Reviews.Add(review);
            _context.SaveChanges();
            return Ok(review);
        }

        [HttpGet("{productId}")]
        public IActionResult GetReviews(int productId)
        {
            var reviews = _context.Reviews
                .Include(r => r.User)
                .Where(r => r.ProductId == productId)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new
                {
                    r.Id,r.ProductId,r.UserId,
                    UserName = r.User != null? r.User.Name: "Anonymous",
                    r.Rating,r.Comment,r.CreatedAt
                })
                .ToList();
                return Ok(reviews);
        }

        // [Authorize]
        [HttpGet("myreviews")]
        public IActionResult MyReviews()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var reviews = _context.Reviews
                .Where(r => r.UserId == userId)
                .Select(r => new ReviewHistoryDto
            {
                ProductId = r.ProductId,
                ProductName = r.Product != null ? r.Product.Name: "Unknown Product",
                Rating = r.Rating,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt
            })
            .ToList();

            return Ok(reviews);
        }
    }
}