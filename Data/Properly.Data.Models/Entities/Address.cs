namespace Properly.Data.Models.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Properly.Data.Common.Models;

    using static Properly.Common.EntityConstants.AddressConstants;

    public class Address : BaseDeletableModel<int>
    {
        [Required]
        [StringLength(StreetNameMaxLength)]
        public string StreetName { get; set; }

        [Required]
        [StringLength(CityNameMaxLength)]
        public string City { get; set; }

        [Required]
        [StringLength(ZipCodeMaxLength)]
        public string ZipCode { get; set; }

        [Required]
        [StringLength(CountryNameMaxLength)]
        public string Country { get; set; }

        [Range(LatitudeMinLength, LatitudeMaxLength)]
        public double? Latitude { get; set; } = null;

        [Range(LongitudeMinLength, LongitudeMaxLength)]
        public double? Longitude { get; set; } = null;

    }
}