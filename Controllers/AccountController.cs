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

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost]

        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
               return View(model);
            }

            var result  = await _userService.RegisterUser(model);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.ErrorMessage);
                return View(model);
            }

            TempData["SuccessMessage"] = "Registration successful. Please login.";
            return RedirectToAction("Login", "Account");

        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

           var result = await _userService.LoginUser(model.UserName,model.Password);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.ErrorMessage);
                return View(model);
            }

            TempData["SuccessMessage"] ="Login Sucessfull";



            return View();

        }

    }
}
