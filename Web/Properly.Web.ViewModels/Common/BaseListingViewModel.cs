namespace Properly.Web.ViewModels.Common
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Properly.Services.Mapping;

    public class BaseListingViewModel : IHaveCustomMappings
    {
        public Guid Id { get; set; }

        public int Price { get; set; }

        public int Size { get; set; }

        public int Bathrooms { get; set; }

        public int Bedrooms { get; set; }

        public string Description { get; set; }

        public DateTime ConstructionDate { get; set; }

        public string PropertyTypeName { get; set; }

        public string ListingStatus { get; set; }

        public string ListingType { get; set; }

        public string StreetName { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }

        public DateTime CreatedOn { get; set; }

        public IEnumerable<string> Photos { get; set; }

        public IEnumerable<string> PropertyFeatures { get; set; }

        public string FullAddress => $"{this.StreetName}, {this.City}, {this.ZipCode}";

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Data.Models.Entities.Listing, BaseListingViewModel>()
                .ForMember(d => d.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(d => d.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(d => d.Size, opt => opt.MapFrom(src => src.Property.Size))
                .ForMember(d => d.Description, opt => opt.MapFrom(src => src.Property.Description))
                .ForMember(d => d.ConstructionDate, opt => opt.MapFrom(src => src.Property.ConstructionDate))
                .ForMember(d => d.Bathrooms, opt => opt.MapFrom(src => src.Property.Bathrooms))
                .ForMember(d => d.PropertyFeatures, opt => opt.MapFrom(src => src.Property.PropertyFeatures.Select(x => x.Feature.Name)))
                .ForMember(d => d.Bedrooms, opt => opt.MapFrom(src => src.Property.Bedrooms))
                .ForMember(d => d.PropertyTypeName, opt => opt.MapFrom(src => src.Property.PropertyType.Name))
                .ForMember(d => d.ListingStatus, opt => opt.MapFrom(src => src.ListingStatus.Name))
                .ForMember(d => d.ListingType, opt => opt.MapFrom(src => src.ListingType.Name))
                .ForMember(d => d.StreetName, opt => opt.MapFrom(src => src.Property.Address.StreetName))
                .ForMember(d => d.City, opt => opt.MapFrom(src => src.Property.Address.City))
                .ForMember(d => d.ZipCode, opt => opt.MapFrom(src => src.Property.Address.ZipCode))
                .ForMember(d => d.CreatedOn, opt => opt.MapFrom(src => src.CreatedOn))
                .ForMember(d => d.Photos, opt => opt.MapFrom(src => src.Photos.Select(p => p.Url)))
                .ForMember(d => d.FullAddress, opt => opt.Ignore());
        }
    }
}
