namespace Properly.Web.ViewModels.Listing
{
    using System.Linq;

    using AutoMapper;
    using Properly.Services.Mapping;

    public class FavouritesDto : IHaveCustomMappings
    {
        public string PhotoUrl { get; set; }

        public int Price { get; set; }

        public string Address { get; set; }

        public string ListingType { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Data.Models.Entities.Listing, FavouritesDto>()
                .ForMember(x => x.PhotoUrl, opt =>
                    opt.MapFrom(x => x.Photos.FirstOrDefault().Url))
                .ForMember(x => x.Address, opt =>
                    opt.MapFrom(x => x.Property.Address.StreetName + " " + x.Property.Address.ZipCode));

        }
    }
}
