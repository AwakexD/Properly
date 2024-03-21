namespace Properly.Web.ViewModels.Listing
{
    using System.Linq;

    using AutoMapper;
    using Properly.Data.Models;
    using Properly.Data.Models;
    using Properly.Services.Mapping;

    public class ListingInListViewModel : IHaveCustomMappings
    {
        public int Price { get; set; }

        public int Size { get; set; }

        public int Bathrooms { get; set; }

        public int Bedrooms { get; set; }

        public string PropertyTypeName { get; set; }

        public string StreetName { get; set; }

        public string City { get; set; }
        
        public string ZipCode { get; set; }

        public string PhotoUrl { get; set; }

        public string Details => $"{Bathrooms} ba | {Bedrooms} bds | {Size} sqft - {PropertyTypeName}";

        public string FullAddress => $"{StreetName}, {City}, {ZipCode}";

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Listing, ListingInListViewModel>()
                .ForMember(d => d.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(d => d.Size, opt => opt.MapFrom(src => src.Property.Size))
                .ForMember(d => d.Bathrooms, opt => opt.MapFrom(src => src.Property.Bathrooms))
                .ForMember(d => d.Bedrooms, opt => opt.MapFrom(src => src.Property.Bedrooms))
                .ForMember(d => d.PropertyTypeName, opt => opt.MapFrom(src => src.Property.PropertyType.Name))
                .ForMember(d => d.StreetName, opt => opt.MapFrom(src => src.Property.Address.StreetName))
                .ForMember(d => d.City, opt => opt.MapFrom(src => src.Property.Address.City))
                .ForMember(d => d.ZipCode, opt => opt.MapFrom(src => src.Property.Address.ZipCode))
                .ForMember(d => d.PhotoUrl, opt => opt.MapFrom(src => src.Photos.FirstOrDefault().Url))
                .ForMember(d => d.Details, opt => opt.Ignore())
                .ForMember(d => d.FullAddress, opt => opt.Ignore());
        }
    }
}
