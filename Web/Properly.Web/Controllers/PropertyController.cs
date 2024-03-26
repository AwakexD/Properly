using System;
using System.Drawing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Build.Framework;
using Properly.Web.ViewModels.Rent;

namespace Properly.Web.Controllers
{
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Properly.Data.Models.User;
    using Properly.Services.Data.Contracts;
    using Properly.Web.ViewModels.Buy;
    using Properly.Web.ViewModels.Sell;

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

        [Authorize]
        public async Task<IActionResult> Sell()
        {
            var viewModel = new CreateListingViewModel()
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

            var user = await this.userManager.GetUserAsync(this.User);

            await this.propertyService.CreateListingAsync(viewModel, user.Id);

            return this.Redirect("/");
        }

        // ToDO : Add Search
        public async Task<IActionResult> Buy(int id = 1)
        {
            const string listingType = "For Sale";

            if (id <= 0)
            {
                return this.NotFound();
            }

            BuyViewModel viewModel = new BuyViewModel()
            {
                PageNumber = id,
                ItemsPerPage = 6,
                Listings = await this.propertyService.GetAll(id, listingType),
                ListingCount = this.propertyService.GetCount(listingType),
            };

            return this.View(viewModel);
        }

        public async Task<IActionResult> Rent(int id = 1)
        {
            const string listingType = "Rent";

            if (id <= 0)
            {
                return this.NotFound();
            }

            RentViewModel viewModel = new RentViewModel()
            {
                PageNumber = id,
                ItemsPerPage = 6,
                Listings = await this.propertyService.GetAll(id, listingType),
                ListingCount = this.propertyService.GetCount(listingType),
            };

            return this.View(viewModel);
        }
    }
}
