using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcApp.Models.ViewModels;
using MvcApp.Services.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Reflection;

namespace MvcApp.Controllers
{
    [Authorize(Roles ="Admin")]
    public class AdminController : Controller
    {
        private readonly IAdminServices _adminServices;

        private readonly IUserService _userService;

        public AdminController(IAdminServices adminServices,IUserService userService)
        {
            _adminServices = adminServices;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Users()
        {
            var users = await _adminServices.GetAllUsers();

            return View(users);
        }

        [HttpGet]
        public async Task<IActionResult> EditUser(int id)
        {
            var user = await _userService.GetUserProfile(id);

            if (user == null)
            {
                ModelState.AddModelError("", "No User Found On The curresponding id ");
            }

            Response.HttpContext.Session.Remove("userId");

            Response.HttpContext.Session.SetInt32("userId", id);

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(int id,UserProfileViewModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _userService.UpdateUserProfile(id, model,"Admin");

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.ErrorMessage);
                return View(model);
            }

            TempData["SuccessMessage"] = "Profile Updated successfully.";

            return View(model);
        }

        [HttpGet]

        public async Task<IActionResult> EditImage(int id)
        {
            var model = await _userService.GetProfileImage(id);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditImage(int id,ProfileImageViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _userService.UpdateImage(id, model.File,"Admin");

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.ErrorMessage);
                return View(model);
            }

            TempData["SuccessMessage"] = "Image updated successfully";

            return RedirectToAction("EditUser","Admin" ,new { id });
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await _adminServices.DeleteUser(id);

            if (!result.IsSuccess)
            {
                ModelState.AddModelError("", result.ErrorMessage);
                return RedirectToAction("Users");
            }


            TempData["SuccessMessage"] = "User Deleted successfully";

            return RedirectToAction("Users", "Admin");


        }

    }
}
