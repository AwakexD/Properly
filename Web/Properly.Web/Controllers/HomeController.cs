namespace Properly.Web.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics;

    using Microsoft.AspNetCore.Mvc;
    using Properly.Web.ViewModels;
    using Properly.Web.ViewModels.Home;
    using Properly.Web.ViewModels.Listing;

    public class HomeController : BaseController
    {
        public IActionResult Index()
        {
            // TODO : Service that will retrieve the data from the db
            // Test view model
            var viewModel = new IndexViewModel()
            {
                ListingModels = new HashSet<ListingIndexViewModel>()
                {
                    new ListingIndexViewModel()
                        { Address = "10644 Bellagio Rd, Los Angeles, CA 90077", Price = 20000000 },
                    new ListingIndexViewModel()
                        { Address = "594 S Mapleton Dr, Los Angeles, CA 90024", Price = 6000000 },
                    new ListingIndexViewModel()
                        { Address = "594 S Mapleton Dr, Los Angeles, CA 90024", Price = 6000000 },
                },
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
