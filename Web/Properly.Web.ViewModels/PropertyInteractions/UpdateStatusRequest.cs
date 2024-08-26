namespace Properly.Web.ViewModels.PropertyInteractions
{
    using Properly.Web.ViewModels.Listing.Enums;
    
    public class UpdateStatusRequest
    {
        public string ListingId { get; set; }

        public string ListingStatus { get; set; }
    }
}
