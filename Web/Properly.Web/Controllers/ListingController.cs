namespace Properly.Web.Controllers
{
    using System;
    using System.Threading.Tasks;

    using Microsoft.AspNetCore.Mvc;
    using Properly.Services.Data.Contracts;
    using Properly.Web.ViewModels.Common;

    public class ListingController : BaseController
    {
        private readonly IPropertyService propertyService;

        public ListingController(IPropertyService propertyService)
        {
            this.propertyService = propertyService;
        }

        public async Task<IActionResult> Id(Guid id)
        {
            BaseListingViewModel viewModel = await this.propertyService.GetListingById(id);

            return this.View(viewModel);
        }
    }
}
