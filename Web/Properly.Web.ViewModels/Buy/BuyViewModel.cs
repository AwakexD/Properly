namespace Properly.Web.ViewModels.Buy
{
    using System.Collections.Generic;

    using Properly.Web.ViewModels.Common;
    using Properly.Web.ViewModels.Listing;

    public class BuyViewModel : PagingViewModel
    {
        public IEnumerable<ListingInListViewModel> Listings { get; set; }
    }
}
