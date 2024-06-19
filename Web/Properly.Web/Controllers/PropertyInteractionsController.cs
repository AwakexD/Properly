using Microsoft.AspNetCore.Identity;
using Properly.Data.Models.User;

namespace Properly.Web.Controllers
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
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
        public async Task<IActionResult> Favourite(PropertyInteractionRequest input)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier)!.Value;

            try
            {
                await this.propertyService.AddToFavouritesAsync(input.ListingId, userId);
                return this.NoContent();
            }
            catch (Exception e)
            {
                return this.StatusCode(500, e.Message);
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Edit()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Sold()
        {
            throw new NotImplementedException();
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Delete(PropertyInteractionRequest input)
        {
            try
            {
                var user = await this.userManager.GetUserAsync(this.User);

                await this.propertyService.DeactivateListing(user.Id, input.ListingId);

                return this.RedirectToAction("Dashboard", "User");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }


        }
    }
}
