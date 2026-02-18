using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcApp.Helper;
using MvcApp.Models.ViewModels;
using MvcApp.Services.Interfaces;

namespace MvcApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<AccountController> _logger;
        private readonly IUserFinder _userFinder;
        private readonly IAccountServices _accountServices;

        public AccountController(IUserService userService,ILogger<AccountController> logger,IUserFinder userFinder,IAccountServices accountServices)
        {
            _userService = userService;
            _logger = logger;
            _userFinder = userFinder;
            _accountServices = accountServices;
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
                _logger.LogInformation("Registration attempt for email: {Email}", model.UserName);
                return View(model);
            }

            var result  = await _accountServices.RegisterUser(model);

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

           var result = await _accountServices.LoginUser(model.UserName,model.Password);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.ErrorMessage);
                return View(model);
            }

            TempData["SuccessMessage"] ="Login Sucessfull";

            Response.Cookies.Append("jwt", result.Token, new CookieOptions
            {
                HttpOnly=true,
                Secure=true,
                SameSite=SameSiteMode.Strict
            });

            if (result.IsAdmin) return RedirectToAction("Users", "Admin");


            return RedirectToAction("Profile", "User");

        }

        [Authorize]
        [HttpPost]
        public IActionResult Logout()
        {
            var userId = _userFinder.GetId();

            _logger.LogInformation("User {UserId} logged out.", userId);

            HttpContext.Session.Clear();
            return RedirectToAction("Login");

        }

        [Authorize]
        [HttpGet]
        public IActionResult ChangePassword()
        {
            
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            };

            int userId = _userFinder.GetId();

            var result = await _userService.ChangePassword(userId, model.OldPassword, model.NewPassword);
            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.ErrorMessage);
                return View(model);
            }

            TempData["SuccessMessage"] = "Password Changed Successfully";

            return RedirectToAction("Login");
        }

    }
}
