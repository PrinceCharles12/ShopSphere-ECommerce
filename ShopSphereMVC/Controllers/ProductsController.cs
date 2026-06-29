using Microsoft.AspNetCore.Mvc;
using ShopSphereMVC.Models;
using ShopSphereMVC.Services;

namespace ShopSphereMVC.Controllers
{
    public class ProductsController : BaseController
    {
        private readonly ProductService _productService;
        private readonly CategoryService _categoryService;
        private readonly ReviewService _reviewService;

        public ProductsController(ProductService productService,CategoryService categoryService,
            WishlistService wishlistService,ReviewService reviewService): base(wishlistService)
        {
            _productService = productService;
            _categoryService = categoryService;
            _reviewService = reviewService;
        }

        private bool IsAdmin()
        {
            return HttpContext.Session.GetString("Role") == "Admin";
        }

        public async Task<IActionResult> Index(int page = 1)
        {
            await LoadWishlistCount();
            int pageSize = 8;
            var products = await _productService.GetProductsAsync();
            int totalProducts = products.Count();
            var pagedProducts = products.Skip((page - 1) * pageSize).Take(pageSize).ToList();
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalProducts / pageSize);
            ViewBag.Categories = await _categoryService.GetCategoriesAsync();

            return View(pagedProducts);
        }

        public async Task<IActionResult> Create()
        {
            if (!IsAdmin())
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = await _categoryService.GetCategoriesAsync();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product product)
        {
            if (!IsAdmin())
            {
                return RedirectToAction(nameof(Index));
            }

            if (product.ImageFile != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(product.ImageFile.FileName);
                string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/images");

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                string filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await product.ImageFile.CopyToAsync(stream);
                }

                product.ImageUrl = "/images/" + fileName;
            }

            var success = await _productService.CreateProductAsync(product);

            if (success)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = await _categoryService.GetCategoriesAsync();
            return View(product);
        }

        public async Task<IActionResult> Edit(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction(nameof(Index));
            }

            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            ViewBag.Categories = await _categoryService.GetCategoriesAsync();
            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            if (!IsAdmin())
            {
                return RedirectToAction(nameof(Index));
            }

            if (string.IsNullOrEmpty(product.ImageUrl))
            {
                var oldProduct = await _productService.GetProductByIdAsync(product.Id);
                product.ImageUrl = oldProduct?.ImageUrl;
            }

            if (product.ImageFile != null)
            {
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(product.ImageFile.FileName);

                string uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");

                if (!Directory.Exists(uploadFolder))
                {
                    Directory.CreateDirectory(uploadFolder);
                }

                string filePath = Path.Combine(uploadFolder, fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await product.ImageFile.CopyToAsync(stream);
                }

                product.ImageUrl = "/images/" + fileName;
            }

            var success = await _productService.UpdateProductAsync(product);

            if (success)
            {
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = await _categoryService.GetCategoriesAsync();

            return View(product);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            if (!IsAdmin())
            {
                return RedirectToAction(nameof(Index));
            }

            await _productService.DeleteProductAsync(id);

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Search(string keyword)
        {
            await LoadWishlistCount();
            ViewBag.Categories = await _categoryService.GetCategoriesAsync();
            var products = await _productService.SearchProductsAsync(keyword);
            return View("Index", products);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetProductByIdAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            var reviews = await _reviewService.GetReviewsByProductAsync(id);
            ViewBag.Reviews = reviews;
            return View(product);
        }

        [HttpGet]
        public async Task<IActionResult> Sort(string sortBy,int? categoryId,decimal? minPrice,decimal? maxPrice)
        {
            await LoadWishlistCount();

            var products = await _productService.AdvancedFilterAsync(categoryId,minPrice,maxPrice);

            products = sortBy switch
            {
                "priceAsc" => products.OrderBy(p => p.Price).ToList(),
                "priceDesc" => products.OrderByDescending(p => p.Price).ToList(),
                "nameAsc" => products.OrderBy(p => p.Name).ToList(),
                "nameDesc" => products.OrderByDescending(p => p.Name).ToList(), _ => products
            };

            ViewBag.Categories = await _categoryService.GetCategoriesAsync();
            ViewBag.CategoryId = categoryId;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;
            ViewBag.SortBy = sortBy;

            return View("Index", products);
        }

        [HttpGet]
        public async Task<IActionResult> AdvancedFilter(int? categoryId,decimal? minPrice,decimal? maxPrice)
        {
            await LoadWishlistCount();

            ViewBag.Categories = await _categoryService.GetCategoriesAsync();
            ViewBag.CategoryId = categoryId;
            ViewBag.MinPrice = minPrice;
            ViewBag.MaxPrice = maxPrice;

            var products = await _productService.AdvancedFilterAsync(categoryId,minPrice,maxPrice);

            return View("Index", products);
        }
    }
}