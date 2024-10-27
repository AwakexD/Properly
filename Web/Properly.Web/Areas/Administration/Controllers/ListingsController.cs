using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Properly.Data.Models.Entities;
using Properly.Services.Data.Contracts;
using Properly.Web.ViewModels;
using Properly.Web.ViewModels.Listing;
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
            var viewModel = await adminListingService.GetListingEditDataAsync(listingId);

            return this.View(viewModel);
        }

        [HttpPost]
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
    }
}
