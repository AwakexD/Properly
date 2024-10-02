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
        private readonly IOptionsService optionsService;

        public HomeController(IPropertyService propertyService, IOptionsService optionsService)
        {
            this.propertyService = propertyService;
            this.optionsService = optionsService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new IndexViewModel()
            {
                ListingModels = await this.propertyService.GetAllListingsByAddedDate(6),
                Features = await this.optionsService.GetFeatures(),
                PropertyTypes = await this.optionsService.GetPropertyTypes(),
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

        public IActionResult About()
        {
            return this.View();
        }
    }
}
