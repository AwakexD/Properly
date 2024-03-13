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

        public HomeController(IPropertyService propertyService)
        {
            this.propertyService = propertyService;
        }

        public async Task<IActionResult> Index()
        {
            var viewModel = new IndexViewModel()
            {
                ListingModels = await this.propertyService.GetAllByAddDate(3),
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
