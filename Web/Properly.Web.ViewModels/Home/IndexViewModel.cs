namespace Properly.Web.ViewModels.Home
{
    using System.Collections.Generic;
    using Properly.Web.ViewModels.Listing;
    
    public class IndexViewModel
    {
        public IndexViewModel()
        {
            this.Listings = new HashSet<ListingIndexViewModel>();
        }

        public int FeaturesCount { get; set; }

        public IEnumerable<ListingIndexViewModel> Listings { get; set; }
    }
}
