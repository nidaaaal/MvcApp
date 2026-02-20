using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcApp.Helper;
using MvcApp.Models.ViewModels;
using MvcApp.Services.Interfaces;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using static System.Net.Mime.MediaTypeNames;

namespace MvcApp.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserFinder _userFinder;

        public UserController(IUserService userService,IUserFinder userFinder)
        {
            _userService = userService;
            _userFinder = userFinder;
        }


        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userId = _userFinder.GetId();

            var result = await _userService.GetUserProfile(userId);


            if(result == null)
            {
                ModelState.AddModelError("", "Please Login Again !");
                return RedirectToAction("Login", "Account");

            }

            HttpContext.Session.SetString("name", result.DisplayName ?? result.FirstName);

            HttpContext.Session.SetString("profileImage",string.IsNullOrEmpty(result.ProfilePath)? "/images/profile.jpg" : result.ProfilePath);

            return View(result);

        }

        [HttpPost]
        public async Task<IActionResult> Profile(UserProfileViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = _userFinder.GetId();

            var result = await _userService.UpdateUserProfile(userId,model,"User");

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("",result.ErrorMessage);
                return View(model);
            }

            TempData["SuccessMessage"] = "Profile Updated successfully.";

            return View(model);
            
        }

        [HttpGet]
        public IActionResult ProfileImage()
        {
            return View(new ProfileImageViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> ProfileImage(ProfileImageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var userId = _userFinder.GetId();
            var result = await _userService.UpdateImage(userId,model.File,"User");

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.ErrorMessage);
                return View(model);
            }

            TempData["SuccessMessage"] = "Image updated successfully";

            return RedirectToAction("Profile");
        }


    }
}
