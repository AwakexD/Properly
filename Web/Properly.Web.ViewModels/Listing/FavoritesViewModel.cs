using Properly.Web.ViewModels.Common;

namespace Properly.Web.ViewModels.Listing
{
    using System.Collections.Generic;

    public class FavoritesViewModel
    {
        public FavoritesViewModel()
        {
            this.FavoriteListings = new HashSet<BaseListingViewModel>();
        }

        public IEnumerable<BaseListingViewModel> FavoriteListings { get; set; }
    }
}
