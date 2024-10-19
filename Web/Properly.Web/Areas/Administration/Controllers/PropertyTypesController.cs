using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Properly.Services.Data.Contracts;
using Properly.Web.ViewModels;

namespace Properly.Web.Areas.Administration.Controllers
{
    public class PropertyTypesController : AdministrationController
    {
        private readonly IOptionsService optionsService;
        public PropertyTypesController(IOptionsService optionsService)
        {
            this.optionsService = optionsService;
        }

        public async Task<IActionResult> All()
        {
            try
            {
                var viewModel = await this.optionsService.GetPropertyTypes();

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
    }
}
