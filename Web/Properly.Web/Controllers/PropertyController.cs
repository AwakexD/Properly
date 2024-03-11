namespace Properly.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Properly.Data;
    using Properly.Data.Models;
    using Properly.Services.Data.Contracts;
    using Properly.Web.ViewModels.Sell;
    using Properly.Web.ViewModels.Sell.Options;

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

        public async Task<IActionResult> Sell()
        {
            // TODO : Move out PropertyTypes and ListingTypes into service
            var viewModel = new SellFormModel()
            {
                ListingOptions =
                {
                    PropertyTypes = await this.optionsService.GetPropertyTypes(),
                    ListingTypes = await this.optionsService.GetListingTypes(),
                    Features = await this.optionsService.GetFeatures(),
                },
            };
            return this.View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Sell(SellFormModel viewModel)
        {
            if (!this.ModelState.IsValid)
            {
                viewModel = new SellFormModel()
                {
                    ListingOptions =
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
    }
}
