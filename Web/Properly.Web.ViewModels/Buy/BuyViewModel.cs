namespace Properly.Web.ViewModels.Buy
{
    using System.Collections.Generic;

    using Properly.Web.ViewModels.Common;
    using Properly.Web.ViewModels.Listing.Enums;
    using Properly.Web.ViewModels.Sell.Options;

    public class BuyViewModel : PagingViewModel
    {
        public BuyViewModel()
        {
            this.Listings = new HashSet<BaseListingViewModel>();
            this.PropertyTypes = new HashSet<PropertyTypeFormModel>();
        }

        public int ListingsPerPage { get; set; }

        public ListingSorting ListingSorting { get; set; }

        public IEnumerable<BaseListingViewModel> Listings { get; set; }

        public IEnumerable<PropertyTypeFormModel> PropertyTypes { get; set; }
    }
}
