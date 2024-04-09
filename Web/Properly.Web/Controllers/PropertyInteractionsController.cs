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
    [Route("api/[controller]")]
    public class PropertyInteractionsController : BaseController
    {
        private readonly IPropertyService propertyService;

        public PropertyInteractionsController(IPropertyService propertyService)
        {
            this.propertyService = propertyService;
        }

        // ToDO : Fix api request error 404
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Favourite(FavouriteListingRequest input)
        {
            var userId = this.User.FindFirst(ClaimTypes.NameIdentifier).Value;

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

        public IActionResult ApiTest()
        {
            return this.Accepted();
        }
    } 
}
