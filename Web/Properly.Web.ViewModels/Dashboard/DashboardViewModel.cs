using System.Collections.Generic;
using Properly.Web.ViewModels.Common;

namespace Properly.Web.ViewModels.Dashboard
{
    public class DashboardViewModel : PagingViewModel
    {
        public DashboardViewModel()
        {
            this.Listings = new HashSet<BaseListingViewModel>();
        }
        public int ActiveMessagesCount { get; set; }

        public int FavoritesCount { get; set; }

        public int ListingsCount { get; set; }

        public IEnumerable<BaseListingViewModel> Listings { get; set; }

    }
}
