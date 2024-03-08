namespace Properly.Web.ViewModels.Sell
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Properly.Web.ViewModels.Sell.Options;

    using static Properly.Common.EntityConstants.PropertyConstants;

    public class SellFormModel : AddressFormModel
    {
        [Required(ErrorMessage = "{0} is required.")]
        [Range(SizeMinLength, SizeMaxLength, ErrorMessage = "Property size must be between {1} and {2} sqft.")]
        [Display(Name = "Size")]
        public int Size { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [Range(0, BathroomsMaxLength, ErrorMessage = "Property must have at least one bathroom.")]
        [Display(Name = "Bathrooms")]
        public int Bathrooms { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [Range(0, BedroomsMaxLength, ErrorMessage = "Property must have at least one bedroom.")]
        [Display(Name = "Bedrooms")]
        public int Bedrooms { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [DataType(DataType.MultilineText)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [Range(100, int.MaxValue, ErrorMessage = "{0} must be more than 100")]
        [Display(Name = "Price")]
        public int Price { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Construction Date")]
        public DateTime ConstructionDate { get; set; }

        public int PropertyType { get; set; }

        public int ListingType { get; set; }

        public IEnumerable<int> SelectedFeatures { get; set; }

        public ListingOptions ListingOptions { get; set; }
    }
}
