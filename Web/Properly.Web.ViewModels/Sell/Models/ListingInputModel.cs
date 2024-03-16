namespace Properly.Web.ViewModels.Sell.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ListingInputModel
    {
        [Display(Name = "Price")]
        [Required(ErrorMessage = "{0} is required.")]
        [Range(100, int.MaxValue, ErrorMessage = "{0} must be more than 100")]
        public int Price { get; set; }

        [Display(Name = "Listing type")]
        [Required(ErrorMessage = "Listing type is required.")]
        public int ListingTypeId { get; set; }
    }
}
