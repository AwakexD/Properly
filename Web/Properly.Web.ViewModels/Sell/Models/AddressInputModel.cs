namespace Properly.Web.ViewModels.Sell.Models
{
    using System.ComponentModel.DataAnnotations;

    using static Properly.Common.EntityConstants.AddressConstants;

    public class AddressInputModel
    {
        [Display(Name = "Street Name")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(StreetNameMaxLength, MinimumLength = StreetNameMinLength, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        public string StreetName { get; set; }

        [Display(Name = "City")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(CityNameMaxLength, MinimumLength = CityNameMinLength, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        public string City { get; set; }

        [Display(Name = "Zip Code")]
        [Required(ErrorMessage = "{0} is required.")]
        [DataType(DataType.PostalCode)]
        [StringLength(ZipCodeMaxLength, MinimumLength = ZipCodeMinLength, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        public string ZipCode { get; set; }

        [Display(Name = "Country")]
        [Required(ErrorMessage = "{0} is required.")]
        [StringLength(CountryNameMaxLength, MinimumLength = CountryNameMinLength, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        public string Country { get; set; }
    }
}