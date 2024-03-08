namespace Properly.Web.Controllers
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.AspNetCore.Mvc;
    using Properly.Data;
    using Properly.Web.ViewModels.Sell;

    public class PropertyController : BaseController
    {
        // TODO : Remove db dependency
        private readonly ApplicationDbContext _context;

        public PropertyController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Sell()
        {
            // TODO : Move out PropertyTypes and ListingTypes into service
            var viewModel = new SellFormModel()
            {
                PropertyTypes = _context.PropertyTypes.Select(x => new PropertyTypesFormModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToList(),
                ListingTypes = _context.ListingTypes.Select(x => new ListingTypesFormModel()
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToList(),
                
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

            return Redirect("/");
        }
    }
}
