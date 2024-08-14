using System.Diagnostics.Eventing.Reader;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Properly.Web.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Properly.Common;
    using Properly.Data.Models.User;
    using Properly.Services.Data.Contracts;
    using Properly.Web.ViewModels.Buy;
    using Properly.Web.ViewModels.Listing;
    using Properly.Web.ViewModels.Rent;
    using Properly.Web.ViewModels.Sell;
    using Properly.Web.ViewModels.Sell.Models;

    public class PropertyController : BaseController
    {
        private readonly IOptionsService optionsService;
        private readonly IPropertyService propertyService;
        private readonly UserManager<ApplicationUser> userManager;

        public PropertyController(
            IOptionsService optionsService,
            IPropertyService propertyService,
            UserManager<ApplicationUser> userManager)
        {
            this.optionsService = optionsService;
            this.propertyService = propertyService;
            this.userManager = userManager;
        }

        public async Task<IActionResult> Sell(string? id)
        {
            var user = await userManager.GetUserAsync(User);
            CreateListingViewModel viewModel;

            try
            {
                if (Guid.TryParse(id, out var listingId))
                {
                    // Fetch the edit model if `id` is provided
                    viewModel = await propertyService.GetListingEditDataAsync(listingId, user.Id);

                    if (viewModel == null)
                    {
                        return NotFound();
                    }

                    TempData["ListingId"] = id;
                    TempData["PropertyId"] = viewModel.Property.Id.ToString();
                    TempData["AddressId"] = viewModel.Address.Id.ToString();
                }
                else
                {
                    // Initialize a new listing model
                    viewModel = new CreateListingViewModel();
                }

                // Populate listing options (property types, listing types, features)
                viewModel.ListingOptions = new ListingOptions
                {
                    PropertyTypes = await optionsService.GetPropertyTypes(),
                    ListingTypes = await optionsService.GetListingTypes(),
                    Features = await optionsService.GetFeatures(),
                };
            }
            catch (Exception)
            {
                // Handle general errors more gracefully
                return StatusCode(500, ExceptionsAndNotificationsMessages.AnErrorOccurred);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Sell(CreateListingViewModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                viewModel = new CreateListingViewModel()
                {
                    ListingOptions = new ListingOptions()
                    {
                        PropertyTypes = await this.optionsService.GetPropertyTypes(),
                        ListingTypes = await this.optionsService.GetListingTypes(),
                        Features = await this.optionsService.GetFeatures(),
                    },
                };

                return this.View(viewModel);
            }

            try
            {
                var user = await this.userManager.GetUserAsync(this.User);

                viewModel.Listing.Id = new Guid(TempData["ListingId"].ToString()); 
                viewModel.Property.Id = new Guid(TempData["PropertyId"].ToString());
                viewModel.Address.Id = Convert.ToInt32(TempData["PropertyId"] as string);

                if (viewModel.ListingOptions is null)
                {
                    await this.propertyService.CreateListingAsync(viewModel, user.Id);
                }
                else
                {
                    await this.propertyService.UpdateListingAsync(viewModel, viewModel.Listing.Id.ToString(), user.Id);
                }
                return this.Redirect("/");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        // ToDO : Add Search
        public async Task<IActionResult> Buy([FromQuery] BuyViewModel queryModel, int id = 1)
        {
            const string listingType = "For Sale";

            if (id <= 0)
            {
                return this.NotFound();
            }

            BuyViewModel viewModel = new BuyViewModel()
            {
                PageNumber = id,
                ItemsPerPage = queryModel.ListingsPerPage,
                ListingSorting = queryModel.ListingSorting,
                Listings = await this.propertyService.GetAll(id, listingType, queryModel.ListingSorting, queryModel.ListingsPerPage),
                ListingCount = this.propertyService.GetCount(listingType),
                PropertyTypes = await this.optionsService.GetPropertyTypes(),
                QueryParameters = new Dictionary<string, string>()
                {
                    { "ListingSorting", ((int)queryModel.ListingSorting).ToString() },
                    { "ListingsPerPage", queryModel.ListingsPerPage.ToString() }
                }
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Rent([FromQuery] BuyViewModel queryModel, int id = 1)
        {
            const string listingType = "For Rent";

            if (id <= 0)
            {
                return this.NotFound();
            }

            BuyViewModel viewModel = new BuyViewModel()
            {
                PageNumber = id,
                ItemsPerPage = queryModel.ListingsPerPage,
                ListingSorting = queryModel.ListingSorting,
                Listings = await this.propertyService.GetAll(id, listingType, queryModel.ListingSorting, queryModel.ListingsPerPage),
                ListingCount = this.propertyService.GetCount(listingType),
                PropertyTypes = await this.optionsService.GetPropertyTypes(),
                QueryParameters = new Dictionary<string, string>()
                {
                    { "ListingSorting", ((int)queryModel.ListingSorting).ToString() },
                    { "ListingsPerPage", queryModel.ListingsPerPage.ToString() }
                }
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Favourites()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            var viewModel = new FavoritesViewModel()
            {
                FavoriteListings = await this.propertyService.GetUserFavourites(user.Id),
            };

            return this.View(viewModel);
        }
    }
}
