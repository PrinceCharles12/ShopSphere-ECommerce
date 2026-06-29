using Microsoft.AspNetCore.Mvc;
using ShopSphereMVC.Models;
using ShopSphereMVC.Services;

namespace ShopSphereMVC.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserService _userService;

        public ProfileController(UserService userService)
        {
            _userService = userService;
        }

        public async Task<IActionResult> Index()
        {
            var email = HttpContext.Session.GetString("Email");

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Account");
            }

            var profile = await _userService.GetProfileAsync(email);
            return View(profile);
        }

        public async Task<IActionResult> Edit()
        {
            var email = HttpContext.Session.GetString("Email");

            if (string.IsNullOrEmpty(email))
            {
                return RedirectToAction("Login", "Account");
            }

            var profile = await _userService.GetProfileAsync(email);

            if (profile == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(new EditProfileViewModel
            {
                Name = profile.Name,
                Email = profile.Email
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var email = HttpContext.Session.GetString("Email");
            var success = await _userService.UpdateProfileAsync(email!, model);

            if (success)
            {
                HttpContext.Session.SetString("Email", model.Email);

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error = "Profile Update Failed";

            return View(model);
        }

        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (model.NewPassword != model.ConfirmPassword)
            {
                ViewBag.Error = "New Password and Confirm Password do not match";
                return View(model);
            }

            var email = HttpContext.Session.GetString("Email");
            var success = await _userService.ChangePasswordAsync(email!, model);

            if (success)
            {
                TempData["Success"] = "Password Changed Successfully";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Error = "Current Password Incorrect";
            return View(model);
        }
    }
}