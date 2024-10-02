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
                    viewModel = new CreateListingViewModel();

                    TempData.Clear();
                }

                viewModel.ListingOptions = await this.optionsService.GetListingOptionsAsync();
            }
            catch (Exception)
            {
                return StatusCode(500, ExceptionsAndNotificationsMessages.AnErrorOccurred);
            }

            return View(viewModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Sell(CreateListingViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.ListingOptions = await this.optionsService.GetListingOptionsAsync();
                return View(viewModel);
            }

            try
            {
                var user = await userManager.GetUserAsync(User);

                if (TempData.ContainsKey("ListingId"))
                {
                    viewModel.Listing.Id = new Guid(TempData["ListingId"].ToString());
                    viewModel.Property.Id = new Guid(TempData["PropertyId"].ToString());
                    viewModel.Address.Id = Convert.ToInt32(TempData["PropertyId"] as string);

                    await propertyService.UpdateListingAsync(viewModel, viewModel.Listing.Id.ToString(), user.Id);

                    TempData.Clear();
                }
                else
                {
                    await propertyService.CreateListingAsync(viewModel, user.Id);

                    TempData.Clear();
                }

                return RedirectToAction("Dashboard", "User");
            }
            catch (Exception ex)
            {
                return StatusCode(500, ExceptionsAndNotificationsMessages.AnErrorOccurred);
            }
        }

        public async Task<IActionResult> Buy([FromQuery] BuyViewModel queryModel, int id = 1)
        {
            const string listingType = "For Sale";

            if (id <= 0)
            {
                return this.NotFound();
            }

            var queryParams = new Dictionary<string, string>
            {
                { "Keyword", queryModel.Keyword ?? "" },
                { "Location", queryModel.Location ?? "" },
                { "PropertyType", queryModel.PropertyType?.ToString() ?? "" },
                { "MinPrice", queryModel.MinPrice?.ToString() ?? "" },
                { "MaxPrice", queryModel.MaxPrice?.ToString() ?? "" },
                { "MinSize", queryModel.MinSize?.ToString() ?? "" },
                { "MaxSize", queryModel.MaxSize?.ToString() ?? "" },
                { "Bathrooms", queryModel.Bathrooms?.ToString() ?? "" },
                { "Bedrooms", queryModel.Bedrooms?.ToString() ?? "" },
                { "Features", queryModel.Features != null ? string.Join(",", queryModel.Features) : "" },
                { "ListingsPerPage", queryModel.ListingsPerPage.ToString() },
                { "ListingSorting", ((int)queryModel.ListingSorting).ToString() }
            };

            BuyViewModel viewModel = new BuyViewModel()
            {
                PageNumber = id,
                ItemsPerPage = queryModel.ListingsPerPage,
                ListingSorting = queryModel.ListingSorting,
                Listings = await this.propertyService.GetAllAsync(queryModel, id, listingType, queryModel.ListingSorting, queryModel.ListingsPerPage),
                ListingCount = this.propertyService.GetCount(listingType),
                PropertyTypes = await this.optionsService.GetPropertyTypes(),
                QueryParameters = queryParams
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
    }
}
