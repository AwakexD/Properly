namespace Properly.Web.ViewModels.PropertyInteractions
{
    using Properly.Web.ViewModels.Listing.Enums;
    
    public class UpdateStatusRequest : PropertyInteractionRequest
    {
        public string ListingStatus { get; set; }
    }
}
