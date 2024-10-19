using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Properly.Services.Data.Contracts;
using Properly.Web.ViewModels;

namespace Properly.Web.Areas.Administration.Controllers
{
    public class UserController : AdministrationController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<IActionResult> All()
        {
            try
            {
                var users = await this.userService.GetAllUsersAsync();

                if (users == null || !users.Any())
                {
                    return View("Error", new ErrorViewModel());
                }

                return View(users);
            }
            catch (Exception ex)
            {
                return View("Error", new ErrorViewModel());
            }
        }
    }
}
