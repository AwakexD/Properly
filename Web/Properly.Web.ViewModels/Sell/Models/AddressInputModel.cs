namespace Properly.Web.ViewModels.Sell.Models
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using Properly.Data.Models.Entities;
    using Properly.Services.Mapping;

    using static Properly.Common.EntityConstants.AddressConstants;

    public class AddressInputModel : IHaveCustomMappings
    {
        public int Id { get; set; }

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

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<AddressInputModel, Address>()
                .ForMember(x => x.StreetName, opt => opt.MapFrom(s => s.StreetName))
                .ForMember(x => x.City, opt => opt.MapFrom(s => s.City))
                .ForMember(x => x.ZipCode, opt => opt.MapFrom(s => s.ZipCode))
                .ForMember(x => x.Country, opt => opt.MapFrom(s => s.Country))
                .ForMember(x => x.Id, opt => opt.Ignore());

            configuration.CreateMap<Address, AddressInputModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(s => s.Id))
                .ForMember(x => x.StreetName, opt => opt.MapFrom(s => s.StreetName))
                .ForMember(x => x.City, opt => opt.MapFrom(s => s.City))
                .ForMember(x => x.ZipCode, opt => opt.MapFrom(s => s.ZipCode))
                .ForMember(x => x.Country, opt => opt.MapFrom(s => s.Country));
        }
    }
}