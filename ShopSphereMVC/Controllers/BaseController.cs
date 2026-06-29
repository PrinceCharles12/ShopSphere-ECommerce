using Microsoft.AspNetCore.Mvc;
using ShopSphereMVC.Services;

namespace ShopSphereMVC.Controllers
{
    public class BaseController : Controller
    {
        protected readonly WishlistService _wishlistService;

        public BaseController(WishlistService wishlistService)
        {
            _wishlistService = wishlistService;
        }

        protected async Task LoadWishlistCount()
        {
            var items = await _wishlistService.GetWishlistAsync();
            ViewBag.WishlistCount = items.Count;
        }

        public override void OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext context)
        {
            var countTask = _wishlistService.GetWishlistAsync();
            countTask.Wait();
            ViewBag.WishlistCount = countTask.Result.Count;
            base.OnActionExecuting(context);
        }
    }
}