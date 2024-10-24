using System.Collections.Generic;
using Properly.Web.ViewModels.Common;
using Properly.Web.ViewModels.Listing.Enums;

namespace Properly.Web.ViewModels.Listing
{
    public class AdminListingViewModel : PagingViewModel
    {
        public AdminListingViewModel()
        {
            this.Listings = new HashSet<BaseListingViewModel>();
        }

        public ListingSorting ListingSorting { get; set; }

        public ListingType ListingType { get; set; }

        public ListingStatus ListingStatus { get; set; }

        public IEnumerable<BaseListingViewModel> Listings { get; set; }
    }
}
