namespace Properly.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Properly.Common;
    using Properly.Data.Models.User;
    using Properly.Services.Data.Contracts;
    using Properly.Web.ViewModels.Listing;
    using Microsoft.AspNetCore.Http;

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
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteImage([FromBody]DeleteImageRequest input)
        {
            try
            {
                var user = await this.userManager.GetUserAsync(this.User);

                if (string.IsNullOrEmpty(user.Id))
                {
                    return Unauthorized("User is not authenticated.");
                }

                var success = await this.propertyService.DeleteListingImage(user.Id, input.ListingId, input.ImageUrl);

                return Ok(new { Message = "Image deleted successfully.", Success = success });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred.");
            }
        }


        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Sold([FromBody]PropertyInteractionRequest input)
        {
            try
            {
                var user = await this.userManager.GetUserAsync(this.User);
                await this.propertyService.ChangeListingStatus(user.Id, input.ListingId, ViewModels.Listing.Enums.ListingStatus.Sold);

                return this.RedirectToAction("Dashboard", "User");

            }
            catch (Exception e)
            {
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

                if (string.IsNullOrEmpty(user.Id))
                {
                    return Unauthorized("User is not authenticated.");
                }

                var success = await this.propertyService.DeactivateListing(user.Id, input.ListingId);

                return this.Ok(new {Message = "Listing deleted successfully.", Success = success });
            }
            catch (Exception e)
            {
                return this.StatusCode(501, ExceptionsAndNotificationsMessages.AnErrorOccurred);
            }


        }
    }
}
