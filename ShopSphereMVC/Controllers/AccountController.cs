using Microsoft.AspNetCore.Mvc;
using ShopSphereMVC.Models;
using ShopSphereMVC.Services;

namespace ShopSphereMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;

        public AccountController(AuthService authService)
        {
            _authService = authService;
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequest request)
        {
            var result = await _authService.RegisterAsync(request);

            if (result)
            {
                return RedirectToAction("Login");
            }

            ViewBag.Error = "Registration Failed";
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);

            if (result == null)
            {
                ViewBag.Error = "Invalid Credentials";
                return View();
            }

            HttpContext.Session.SetString("JWToken",result.Token);
            HttpContext.Session.SetString("Role",result.Role);
            HttpContext.Session.SetString("Email", request.Email);
            return RedirectToAction("Index","Home");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}