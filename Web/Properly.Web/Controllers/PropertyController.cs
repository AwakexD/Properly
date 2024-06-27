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

        // ToDo : Complete the edit form
        public async Task<IActionResult> Sell(string? id)
        {
            CreateListingViewModel viewModel;
            var user = await this.userManager.GetUserAsync(this.User);

            if (id is not null)
            {
                try
                {
                    var editModel = await propertyService.GetListingEditDataAsync(new Guid(id), user.Id);
                    if (editModel is null)
                    {
                        return NotFound();
                    }

                    editModel.ListingOptions = new ListingOptions()
                    {
                        PropertyTypes = await optionsService.GetPropertyTypes(),
                        ListingTypes = await optionsService.GetListingTypes(),
                        Features = await optionsService.GetFeatures(),
                    };

                    viewModel = editModel;
                    viewModel.Listing.Id = id;
                }
                catch (Exception e)
                {
                    return this.StatusCode(403, ExceptionsAndNotificationsMessages.ErrorEditingListing);
                }
            }
            else
            {
                viewModel = new CreateListingViewModel
                {
                    ListingOptions = new ListingOptions
                    {
                        PropertyTypes = await optionsService.GetPropertyTypes(),
                        ListingTypes = await optionsService.GetListingTypes(),
                        Features = await optionsService.GetFeatures(),
                    }
                };
            }

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Sell(CreateListingViewModel viewModel)
        {
            // Error here in the view model binding(Listing, Property, Address are null)
            // ListingId in the viewModel is null
            // Fix Construction Date Format
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

                if (viewModel.ListingOptions is null)
                {
                    await this.propertyService.CreateListingAsync(viewModel, user.Id);
                }
                else
                {
                    // ToDo : Create update service method
                    await this.propertyService.UpdateListingAsync(viewModel, viewModel.Listing.Id, user.Id);
                }
                return this.Redirect("/");
            }
            catch (Exception e)
            {
                return StatusCode(500, ExceptionsAndNotificationsMessages.AnErrorOccurred);
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
