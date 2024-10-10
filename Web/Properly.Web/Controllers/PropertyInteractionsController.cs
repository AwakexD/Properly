using Properly.Web.ViewModels.Listing.Enums;

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
    using Microsoft.AspNetCore.Http;
    using Properly.Web.ViewModels.PropertyInteractions;

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
        public async Task<IActionResult> UpdateStatus([FromBody]UpdateStatusRequest input)
        {
            try
            {
                var user = await this.userManager.GetUserAsync(this.User);

                if (string.IsNullOrEmpty(user.Id))
                {
                    return Unauthorized("User is not authenticated.");
                }

                Enum.TryParse(input.ListingStatus, out ListingStatus status);

                var success = await this.propertyService.ChangeListingStatus(user.Id, input.ListingId, status);

                return Ok(new { Message = "Listing status successfully updated.", Success = success });

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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> AddToFavorites([FromBody]PropertyInteractionRequest input)
        {
            var userId = this.userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var result = await this.propertyService.AddToFavoritesAsync(userId, input.ListingId);

            if (!result)
            {
                return BadRequest("Failed to add property to favorites.");
            }

            return Ok(new { success = true });
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> RemoveFromFavorites([FromBody] PropertyInteractionRequest input)
        {
            var userId = this.userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized();
            }

            var result = await this.propertyService.RemoveFromFavoritesAsync(userId, input.ListingId);

            if (!result)
            {
                return BadRequest("Failed to remove property from favorites.");
            }

            return Ok(new { success = true });
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> IsFavorite(string listingId)
        {
            var userId = this.userManager.GetUserId(User);

            if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(listingId))
            {
                return BadRequest("Invalid user or listing.");
            }

            var isFavorite = await this.propertyService.IsFavoriteAsync(userId, listingId);
            return Ok(new { isFavorite });
        }

    }
}
