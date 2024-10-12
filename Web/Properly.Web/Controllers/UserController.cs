using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Properly.Data.Models.User;
using Properly.Services.Data.Contracts;
using Properly.Web.ViewModels.Dashboard;
using Properly.Web.ViewModels.Messages;

namespace Properly.Web.Controllers
{
    public class UserController : BaseController
    {
        private readonly IPropertyService propertyService;
        private readonly IMessagesService messagesService;
        private readonly UserManager<ApplicationUser> userManager;

        public UserController(IPropertyService propertyService,
            IMessagesService messagesService,
            UserManager<ApplicationUser> userManager)
        {
            this.propertyService = propertyService;
            this.messagesService = messagesService;
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

        [Authorize]
        public async Task<IActionResult> Messages()
        {
            var user = await this.userManager.GetUserAsync(this.User);

            var viewModel = new MessagesViewModel()
            {
                ActiveMessages = await this.messagesService.GetActiveMessagesForUser(user.Id),
                ArchivedMessages = await this.messagesService.GetArchivedMessagesForUser(user.Id),
            };
            
            return this.View(viewModel);
        }

        [Authorize]
        public async Task<IActionResult> MyFavorites()
        {
            return this.View();
        }
    }
}
