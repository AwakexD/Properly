namespace Properly.Web.ViewModels.Sell
{
    using System.Collections.Generic;

    using Properly.Web.ViewModels.Sell.Options;

    public class ListingOptions
    {
        public IEnumerable<PropertyTypeFormModel> PropertyTypes { get; set; }

        public IEnumerable<ListingTypeFormModel> ListingTypes { get; set; }

        public IEnumerable<FeatureFormModel> Features { get; set; }
    }
}
