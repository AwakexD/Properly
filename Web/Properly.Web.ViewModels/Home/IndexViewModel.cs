using Properly.Web.ViewModels.Common;
using Properly.Web.ViewModels.Sell.Options;

namespace Properly.Web.ViewModels.Home
{
    using System.Collections.Generic;

    using Properly.Web.ViewModels.Listing;

    public class IndexViewModel : PropertyQueryModel
    {
        public IndexViewModel()
        {
            this.ListingModels = new HashSet<ListingIndexViewModel>();
        }

        public IEnumerable<ListingIndexViewModel> ListingModels { get; set; }

        public IEnumerable<FeatureFormModel> Features { get; set; }

        public IEnumerable<PropertyTypeFormModel> PropertyTypes { get; set; }
    }
}
