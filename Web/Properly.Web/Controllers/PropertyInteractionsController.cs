using Microsoft.AspNetCore.Identity;
using Properly.Data.Models.User;
using Properly.Web.ViewModels.Listing.Enums;

namespace Properly.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Properly.Common;
    using Properly.Services.Data.Contracts;
    using Properly.Web.ViewModels.Listing;

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class PropertyInteractionsController : BaseController
    {
        private readonly IPropertyService propertyService;
        private readonly UserManager<ApplicationUser> userManager;

        public PropertyInteractionsController(IPropertyService propertyService,
            UserManager<ApplicationUser> userManager)
        {
            this.propertyService = propertyService;
            this.userManager = userManager;
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Favourite([FromBody]PropertyInteractionRequest input)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit([FromBody]PropertyInteractionRequest input)
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Sold([FromBody]PropertyInteractionRequest input)
        {
            try
            {
                var user = await this.userManager.GetUserAsync(this.User);
                await this.propertyService.ChangeListingStatus(user.Id, input.ListingId, ListingStatus.Sold);

                TempData["Notification"] = ExceptionsAndNotificationsMessages.ListingWasUpdatedSuccessfully;
                TempData["NotificationType"] = "success";

                return this.RedirectToAction("Dashboard", "User");

            }
            catch (Exception e)
            {
                TempData["Notification"] = ExceptionsAndNotificationsMessages.AnErrorOccurred;
                TempData["NotificationType"] = "error";

                return this.StatusCode(501, ExceptionsAndNotificationsMessages.AnErrorOccurred);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete([FromBody]PropertyInteractionRequest input)
        {
            try
            {
                var user = await this.userManager.GetUserAsync(this.User);
                await this.propertyService.DeactivateListing(user.Id, input.ListingId);

                return this.RedirectToAction("Dashboard", "User");
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e);
                return this.StatusCode(501, e.Message);
            }


        }
    }
}
