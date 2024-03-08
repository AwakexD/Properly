namespace Properly.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Properly.Data;
    using Properly.Web.ViewModels.Sell;
    using Properly.Web.ViewModels.Sell.Options;

    public class PropertyController : BaseController
    {
        // TODO : Remove db dependency
        private readonly ApplicationDbContext context;

        public PropertyController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public IActionResult Sell()
        {
            // TODO : Move out PropertyTypes and ListingTypes into service
            var viewModel = new SellFormModel()
            {
                ListingOptions = new ListingOptions()
                {
                    PropertyTypes = this.context.PropertyTypes.Select(x => new PropertyTypeFormModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                    }).ToList(),
                    ListingTypes = this.context.ListingTypes.Select(x => new ListingTypeFormModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                    }).ToList(),
                    Features = this.context.Features.Select(x => new FeatureFormModel()
                    {
                        Id = x.Id,
                        Name = x.Name,
                    }),
                },
            };
            return this.View(viewModel);
        }

        [HttpPost]
        public IActionResult Sell(SellFormModel model)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(model);
            }

            // TODO : Create service that will add the listing to the db
            return this.Redirect("/");
        }
    }
}
