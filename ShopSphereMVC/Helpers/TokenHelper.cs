using Microsoft.AspNetCore.Http;

namespace ShopSphereMVC.Helpers
{
    public static class TokenHelper
    {
        public static string? GetToken(
            IHttpContextAccessor httpContextAccessor)
        {
            return httpContextAccessor
                .HttpContext?
                .Session
                .GetString("JWToken");
        }
    }
}