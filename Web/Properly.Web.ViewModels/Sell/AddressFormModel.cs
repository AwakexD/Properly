namespace Properly.Web.ViewModels.Sell
{
    using System.ComponentModel.DataAnnotations;

    using static Properly.Common.EntityConstants.AddressConstants;

    public class AddressFormModel
    {
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(StreetNameMaxLength, MinimumLength = StreetNameMinLength, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        [Display(Name = "Street Name")]
        public string StreetName { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(CityNameMaxLength, MinimumLength = CityNameMinLength, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        [Display(Name = "City")]
        public string City { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [DataType(DataType.PostalCode)]
        [StringLength(ZipCodeMaxLength, MinimumLength = ZipCodeMinLength, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        [Display(Name = "Zip Code")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(CountryNameMaxLength, MinimumLength = CountryNameMinLength, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        [Display(Name = "Country")]
        public string Country { get; set; }
    }
}
