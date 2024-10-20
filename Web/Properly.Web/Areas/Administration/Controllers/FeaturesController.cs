using System.Linq;
using System;
using System.Threading.Tasks;
using Humanizer.DateTimeHumanizeStrategy;
using Microsoft.AspNetCore.Mvc;
using Properly.Services.Data.Contracts;
using Properly.Web.ViewModels;
using Properly.Web.ViewModels.AdminListingOptions;

namespace Properly.Web.Areas.Administration.Controllers
{
    public class FeaturesController : AdministrationController
    {
        private readonly IAdminListingOptionsService adminListingOptionsService;
        public FeaturesController(IAdminListingOptionsService adminListingOptionsService)
        {
            this.adminListingOptionsService = adminListingOptionsService;
        }

        public async Task<IActionResult> All()
        {
            try
            {
                var viewModel = await this.adminListingOptionsService.GetAllFeaturesAsync();

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
        public async Task<IActionResult> Add(FeatureAdminModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return this.View(model);
                }

                await this.adminListingOptionsService.AddFeatureAsync(model);

                return this.RedirectToAction("All", "Features");
            }
            catch (Exception e)
            {
                return this.View("Error", new ErrorViewModel());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var feature = await adminListingOptionsService.GetFeatureByIdAsync(id);

            if (feature == null)
            {
                return this.View("Error", new ErrorViewModel());
            }

            return this.View(feature);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(FeatureAdminModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return this.View(model);
                }

                await this.adminListingOptionsService.UpdateFeatureAsync(model.Id, model);

                return this.RedirectToAction("All", "Features");
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
                await this.adminListingOptionsService.DeleteFeatureAsync(id, hardDelete);

                return this.RedirectToAction("All", "Features");
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
                await this.adminListingOptionsService.ActivateFeatureAsync(id);

                return this.RedirectToAction("All", "Features");
            }
            catch (Exception e)
            {
                return this.View("Error", new ErrorViewModel());
            }
        }
    }
}
