namespace Properly.Web.ViewModels.Listing
{
    using System.Collections.Generic;

    public class FavoritesViewModel
    {
        public FavoritesViewModel()
        {
            this.FavoriteListings = new HashSet<FavouritesDto>();
        }

        public IEnumerable<FavouritesDto> FavoriteListings { get; set; }
    }
}
