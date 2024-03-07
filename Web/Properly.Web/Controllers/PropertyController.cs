using Properly.Web.ViewModels.Sell;

namespace Properly.Web.Controllers
{
    using Microsoft.AspNetCore.Mvc;

    public class PropertyController : BaseController
    {
        public IActionResult Sell()
        {
            return this.View();
        }

        [HttpPost]
        public IActionResult Sell(SellFormModel model)
        {
            // TODO : Add listing service
            return this.View();
        }
    }
}
