namespace Properly.Web.ViewModels.Sell
{
    using Properly.Web.ViewModels.Sell.Models;

    public class CreateListingViewModel
    {
        // ToDO : Rewrite model validation
        // ToDO : Create Cloudinary service
        // ToDO : Add Photo Upload option

        public ListingInputModel Listing { get; set; }

        public PropertyInputModel Property { get; set; }

        public AddressInputModel Address { get; set; }

        public ListingOptions ListingOptions { get; set; }
    }
}
