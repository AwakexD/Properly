using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Properly.Services.Data.Contracts;
using Properly.Web.ViewModels;
using Properly.Web.ViewModels.Listing;
using Properly.Web.ViewModels.Sell;
using ListingStatus = Properly.Web.ViewModels.Listing.Enums.ListingStatus;

namespace Properly.Web.Areas.Administration.Controllers
{
    public class ListingsController : AdministrationController
    {
        private readonly IAdminListingService adminListingService;
        private readonly IListingOptionsService listingOptionsService;

        public ListingsController(IAdminListingService adminListingService,
            IListingOptionsService listingOptionsService)
        {
            this.adminListingService = adminListingService;
            this.listingOptionsService = listingOptionsService;
        }

        [HttpGet]
        public async Task<IActionResult> All(AdminListingViewModel queryModel, int id = 1)
        {
            var (listings, totalCount) = await this.adminListingService.GetAllListingsAsync(queryModel, id);

            var queryParams = new Dictionary<string, string>
            {
                { "ItemsPerPage", queryModel.ItemsPerPage.ToString() },
                { "ListingSorting", ((int)queryModel.ListingSorting).ToString() },
                { "ListingType", ((int)queryModel.ListingType).ToString() }
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

        [HttpPost]
        public async Task<IActionResult> SoftDelete(string listingId)
        {
            try
            {
                await this.adminListingService.SoftDeleteListingAsync(listingId);
                
                return RedirectToAction("All", "Listings");
            }
            catch (Exception e)
            {
                return this.View("Error", new ErrorViewModel());
            }
        }

        [HttpPost]
        public async Task<IActionResult> Undelete(string listingId)
        {
            try
            {
                await this.adminListingService.UndeleteListingAsync(listingId);

                return RedirectToAction("All", "Listings");
            }
            catch (Exception e)
            {
                return this.View("Error", new ErrorViewModel());
            }
        }

        [HttpGet]
        public async Task<IActionResult> UpdateStatus(string listingId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(listingId))
                {
                    return NotFound();
                }

                var viewModel = await adminListingService.GetListingDataAsync(listingId);

                return this.View(viewModel);
            }
            catch (Exception e)
            {
                return this.View("Error", new ErrorViewModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateStatus(ListingStatus listingStatus, string listingId)
        {
            try
            {
                await this.adminListingService.UpdateListingStatus(listingId, listingStatus);

                return this.RedirectToAction("All", "Listings");
            }
            catch (Exception e)
            {
                return this.View("Error", new ErrorViewModel());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(string listingId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(listingId) || !Guid.TryParse(listingId, out Guid parsedListingId))
                {
                    return NotFound();
                }

                var viewModel = await adminListingService.GetListingEditDataAsync(parsedListingId);
                if (viewModel == null)
                {
                    return NotFound();
                }

                viewModel.ListingOptions = await listingOptionsService.GetListingOptionsAsync();

                return View(viewModel);
            }
            catch (Exception e)
            {
                return this.View("Error", new ErrorViewModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CreateListingViewModel viewModel)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    viewModel.ListingOptions = await listingOptionsService.GetListingOptionsAsync();
                    return View(viewModel);
                }

                bool success = await adminListingService.UpdateListingAsync(viewModel, viewModel.Listing.Id.ToString());
                if (!success)
                {
                    viewModel.ListingOptions = await listingOptionsService.GetListingOptionsAsync();
                    return View(viewModel);
                }

                return RedirectToAction("All", "Listings");
            }
            catch (Exception e)
            {
                return this.View("Error", new ErrorViewModel());
            }
        }
    }
}
