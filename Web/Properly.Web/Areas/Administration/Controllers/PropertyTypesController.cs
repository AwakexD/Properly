using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Properly.Services.Data.Contracts;
using Properly.Web.ViewModels;
using Properly.Web.ViewModels.AdminListingOptions;

namespace Properly.Web.Areas.Administration.Controllers
{
    public class PropertyTypesController : AdministrationController
    {
        private readonly IAdminListingOptionsService adminListingOptionsService;
        public PropertyTypesController(IAdminListingOptionsService adminListingOptionsService)
        {
            this.adminListingOptionsService = adminListingOptionsService;
        }

        public async Task<IActionResult> All()
        {
            try
            {
                var viewModel = await this.adminListingOptionsService.GetAllPropertyTypesAsync();

                if (viewModel == null || !viewModel.Any())
                {
                    return View("Error", new ErrorViewModel());
                }
                
                return this.View(viewModel);
            }
            catch (Exception e)
            {
                return View("Error", new ErrorViewModel());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Add(PropertyTypeAdminModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return this.View();
                }

                await this.adminListingOptionsService.AddPropertyTypeAsync(model);

                return this.RedirectToAction("All", "PropertyTypes");
            }
            catch (Exception e)
            {
                return this.View("Error", new ErrorViewModel());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var propertyType = await adminListingOptionsService.GetPropertyTypeByIdAsync(id);
            if (propertyType == null)
            {
                return NotFound();
            }

            return this.View(propertyType);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PropertyTypeAdminModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return this.View(model);
                }

                await this.adminListingOptionsService.UpdatePropertyTypeAsync(model.Id, model);

                return this.RedirectToAction("All", "PropertyTypes");
            }
            catch (Exception e)
            {
                return this.View("Error", new ErrorViewModel());
            }
        }
    }
}
