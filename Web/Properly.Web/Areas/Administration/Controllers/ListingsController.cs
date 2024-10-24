using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Properly.Services.Data.Contracts;
using Properly.Web.ViewModels.Listing;

namespace Properly.Web.Areas.Administration.Controllers
{
    public class ListingsController : AdministrationController
    {
        private readonly IAdminListingService adminListingService;

        public ListingsController(IAdminListingService adminListingService)
        {
            this.adminListingService = adminListingService;
        }

        [HttpGet]
        public async Task<IActionResult> All(AdminListingViewModel queryModel, int id = 1)
        {
            var (listings, totalCount) = await this.adminListingService.GetAllListingsAsync(queryModel, id);

            var queryParams = new Dictionary<string, string>
            {
                { "ItemsPerPage", queryModel.ItemsPerPage.ToString() },
                { "ListingSorting", ((int)queryModel.ListingSorting).ToString() },
            };

            var viewModel = new AdminListingViewModel()
            {
                Listings = listings,
                QueryParameters = queryParams,
                PageNumber = id,
                ListingCount = totalCount,
                ListingSorting = queryModel.ListingSorting,
            };

            return View(viewModel);
        }
    }
}
