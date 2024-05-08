namespace Properly.Web.ViewModels.Listing
{
    using System.Linq;

    using AutoMapper;
    using Properly.Services.Mapping;

    public class ListingIndexViewModel : IMapFrom<Data.Models.Entities.Listing>, IHaveCustomMappings
    {
        public string PhotoUrl { get; set; }

        public int Price { get; set; }

        public string Address { get; set; }

        public int Bedrooms { get; set; }

        public int Bathrooms { get; set; }

        public int Size { get; set; }

        public string Type { get; set; }

        public string ListingType { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Data.Models.Entities.Listing, ListingIndexViewModel>()
                .ForMember(x => x.Address, opt =>
                    opt.MapFrom(x => x.Property.Address.StreetName + " " + x.Property.Address.ZipCode))
                .ForMember(x => x.PhotoUrl, opt =>
                    opt.MapFrom(x => x.Photos.FirstOrDefault().Url))
                .ForMember(x => x.Bedrooms, opt =>
                    opt.MapFrom(x => x.Property.Bedrooms))
                .ForMember(x => x.Bathrooms, opt =>
                    opt.MapFrom(x => x.Property.Bathrooms))
                .ForMember(x => x.Size, opt =>
                    opt.MapFrom(x => x.Property.Size))
                .ForMember(x => x.Type, opt =>
                    opt.MapFrom(x => x.Property.PropertyType.Name))
                .ForMember(x => x.ListingType, opt =>
                    opt.MapFrom(x => x.ListingType.Name));
        }
    }
}
