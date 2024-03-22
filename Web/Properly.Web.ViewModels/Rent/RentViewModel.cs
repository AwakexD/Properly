using Properly.Web.ViewModels.Common;

namespace Properly.Web.ViewModels.Rent
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    using Properly.Web.ViewModels.Listing;

    public class RentViewModel : PagingViewModel
    {
        public IEnumerable<ListingInListViewModel> Listings;
    }
}
