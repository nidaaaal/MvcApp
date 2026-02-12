using Microsoft.AspNetCore.Mvc;
using MvcApp.Models.ViewModels;
using MvcApp.Services.Interfaces;
using System.Diagnostics.Eventing.Reader;

namespace MvcApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = Convert.ToInt32(HttpContext.Session.GetInt32("userId"));

            var result = await _userService.GetUserProfile(userId);


            if(result == null)
            {
                ModelState.AddModelError("", "Please Login Again !");
                return RedirectToAction("Login", "Account");

            }

            HttpContext.Session.SetString("name", result.DisplayName ?? result.FirstName);

            return View(result);

        }

        [HttpPost]
        public async Task<IActionResult> Profile(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int userId = HttpContext.Session.GetInt32("userId") ?? 0;

            var result = await _userService.UpdateUserProfile(userId,model);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("",result.ErrorMessage);
                return View(model);
            }

            TempData["SuccessMessage"] = "Profile Updated successfully.";

            return View(model);
            
        }

    }
}
