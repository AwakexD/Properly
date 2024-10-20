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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(PropertyTypeAdminModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return this.View(model);
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
                return this.View("Error", new ErrorViewModel());
            }

            return this.View(propertyType);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id, bool hardDelete)
        {
            try
            {
                await this.adminListingOptionsService.DeletePropertyTypeAsync(id, hardDelete);

                return this.RedirectToAction("All", "PropertyTypes");
            }
            catch (Exception e)
            {
                return this.View("Error", new ErrorViewModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Activate(int id)
        {
            try
            {
                await this.adminListingOptionsService.ActivatePropertyTypeAsync(id);

                return this.RedirectToAction("All", "PropertyTypes");
            }
            catch (Exception e)
            {
                return this.View("Error", new ErrorViewModel());
            }
        }
    }
}
