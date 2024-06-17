using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Properly.Data.Models.User;
using Properly.Services.Data.Contracts;
using Properly.Web.ViewModels.Dashboard;

namespace Properly.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly IPropertyService propertyService;
        private readonly UserManager<ApplicationUser> userManager;

        public UserController(IPropertyService propertyService,
            UserManager<ApplicationUser> userManager)
        {
            this.propertyService = propertyService;
            this.userManager = userManager;
        }

        [Authorize]
        public async Task<IActionResult> Dashboard()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var viewModel = new DashboardViewModel()
            {
                Listings = await this.propertyService.GetUserListings(user.Id),
            };

            return this.View(viewModel);
        }
    }
}
