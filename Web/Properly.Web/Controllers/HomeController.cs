using Properly.Web.ViewModels.Buy;
using Properly.Web.ViewModels.Listing.Enums;

namespace Properly.Web.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Properly.Services.Data.Contracts;
    using Properly.Web.ViewModels;
    using Properly.Web.ViewModels.Home;
    using Properly.Web.ViewModels.Listing;

    public class HomeController : BaseController
    {
        private readonly IPropertyService propertyService;
        private readonly IListingOptionsService listingOptionsService;

        public HomeController(IPropertyService propertyService, IListingOptionsService listingOptionsService)
        {
            this.propertyService = propertyService;
            this.listingOptionsService = listingOptionsService;
        }

        public async Task<IActionResult> Index()
        {
            var (listings, totalCount) = await this.propertyService.GetAllAsync(
                new BuyViewModel() { ListingSorting = ListingSorting.Newest }, 1, "For Sale");
            
            var viewModel = new IndexViewModel()
            {
                ListingModels = listings,
                Features = await this.listingOptionsService.GetFeatures(),
                PropertyTypes = await this.listingOptionsService.GetPropertyTypes(),
            };
            return this.View(viewModel);
        }

        public IActionResult Privacy()
        {
            return this.View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return this.View(
                new ErrorViewModel { RequestId = Activity.Current?.Id ?? this.HttpContext.TraceIdentifier });
        }
    }
}
