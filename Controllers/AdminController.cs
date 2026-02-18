using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MvcApp.Services.Interfaces;

namespace MvcApp.Controllers
{
    [Authorize(Roles ="Admin")]

    public class AdminController : Controller
    {
        private readonly IAdminServices _adminServices;

        public AdminController(IAdminServices adminServices)
        {
            _adminServices = adminServices;
        }

        public async Task<IActionResult> Users()
        {
            var users = await _adminServices.GetAllUsers();

            return View(users);
        }


    }
}
