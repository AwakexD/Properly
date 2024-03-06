namespace Properly.Web.ViewModels.Home
{
    using System.Collections.Generic;

    using Properly.Web.ViewModels.Listing;

    public class IndexViewModel
    {
        public IndexViewModel()
        {
            this.ListingModels = new HashSet<ListingIndexViewModel>();
        }

        public IEnumerable<ListingIndexViewModel> ListingModels { get; set; }
    }
}
