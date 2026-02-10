using Microsoft.AspNetCore.Mvc;
using MvcApp.Services.Interfaces;

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
    }
}
