using AutoMapper;
using ECommerceAPI.DTOs;
using ECommerceAPI.Models;
using ECommerceAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly IMapper _mapper;
        public ProductsController(IProductService productService, IMapper mapper)
        {
            _productService = productService;
            _mapper = mapper;
        }

        // [Authorize]
        [HttpGet]
        public IActionResult GetProducts()
        {
            var products = _productService.GetAllProducts();
            var result = products.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                Stock = p.Stock,
                CategoryId = p.CategoryId,
                ImageUrl = p.ImageUrl,
                AverageRating = p.Reviews.Any()? p.Reviews.Average(r => r.Rating): 0,
                ReviewCount = p.Reviews.Count
            }).ToList();

            return Ok(result);
        }

        // [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetProductById(int id)
        {
            var product = _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }
            var result = _mapper.Map<ProductResponseDto>(product);
            return Ok(result);
        }

        // [Authorize(Roles = "Admin")]
        [HttpPost]
        public IActionResult AddProduct(ProductCreateDto dto)
        {
            var product = _mapper.Map<Product>(dto);
            _productService.AddProduct(product);
            return Ok(product);
        }

        // [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(int id, ProductUpdateDto dto)
        {
            var product = _productService.GetProductById(id);

            if (product == null)
            {
                return NotFound();
            }

            _mapper.Map(dto, product);
            _productService.UpdateProduct(product);
            return Ok(product);
        }

        // [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(int id)
        {
            var deleted = _productService.DeleteProduct(id);

            if (!deleted)
            {
                return NotFound();
            }

            return Ok("Product Deleted Successfully");
        }

        [HttpGet("search")]
        public IActionResult Search(string keyword)
        {
            var products = _productService.GetAllProducts()
            .Where(p => p.Name.Contains(keyword) || p.Description.Contains(keyword))
            .ToList();

            return Ok(products);
        }

        [HttpGet("advancedfilter")]
        public IActionResult AdvancedFilter(int? categoryId,decimal? minPrice,decimal? maxPrice)
        {
            var products = _productService.GetAllProducts().AsQueryable();

            if (categoryId.HasValue)
            {
                products = products.Where(p => p.CategoryId == categoryId.Value);
            }

            if (minPrice.HasValue)
            {
                products = products.Where(p => p.Price >= minPrice.Value);
            }

            if (maxPrice.HasValue)
            {
                products = products.Where(p => p.Price <= maxPrice.Value);
            }

            var result = products.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                CategoryId = p.CategoryId,
                ImageUrl = p.ImageUrl,

                AverageRating = p.Reviews.Any() ? p.Reviews.Average(r => r.Rating): 0,
                ReviewCount = p.Reviews.Count
            }).ToList();

            return Ok(result);
        }

        [HttpGet("toprated")]
        public IActionResult GetTopRatedProducts()
        {
            var products = _productService.GetAllProducts().Take(5).ToList();
            return Ok(products);
        }

        [HttpGet("sort")]
        public IActionResult Sort(string sortBy)
        {
            var products = _productService.GetAllProducts();

            products = sortBy switch
            {
                "priceAsc"  => products.OrderBy(p => p.Price).ToList(),
                "priceDesc" => products.OrderByDescending(p => p.Price).ToList(),
                "nameAsc"   => products.OrderBy(p => p.Name).ToList(),
                "nameDesc"  => products.OrderByDescending(p => p.Name).ToList(), _ => products
            };

            var result = products.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                Stock = p.Stock,
                CategoryId = p.CategoryId,
                ImageUrl = p.ImageUrl,

                AverageRating = p.Reviews.Any() ? p.Reviews.Average(r => r.Rating) : 0,
                ReviewCount = p.Reviews.Count
            }).ToList();

            return Ok(result);
        }
    }
}