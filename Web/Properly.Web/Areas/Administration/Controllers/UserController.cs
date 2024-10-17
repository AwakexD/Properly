using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Properly.Services.Data.Contracts;

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
            var users = await this.userService.GetAllUsersAsync();

            return this.View(users);
        }
    }
}
