using Microsoft.AspNetCore.Mvc;
using MvcApp.Models.ViewModels;
using MvcApp.Services.Interfaces;

namespace MvcApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }



        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                View(model);
            }

            var result  = await _userService.RegisterUser(model);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.ErrorMessage);
                return View(model);
            }

            TempData["successMessage"] = result.ErrorMessage;
            return RedirectToAction("Login", "Account");

        }
    }
}
