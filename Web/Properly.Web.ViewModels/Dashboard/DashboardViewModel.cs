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

        public IEnumerable<BaseListingViewModel> Listings { get; set; }
    }
}
