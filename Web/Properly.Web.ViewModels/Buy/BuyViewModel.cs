namespace Properly.Web.ViewModels.Buy
{
    using System.Collections.Generic;

    using Properly.Web.ViewModels.Common;

    public class BuyViewModel : PagingViewModel
    {
        public IEnumerable<BaseListingViewModel> Listings { get; set; }
    }
}
