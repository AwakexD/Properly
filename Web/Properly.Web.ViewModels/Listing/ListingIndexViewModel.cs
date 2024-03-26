namespace Properly.Web.ViewModels.Listing
{
    using System.Linq;

    using AutoMapper;
    using Properly.Services.Mapping;

    public class ListingIndexViewModel : IMapFrom<Data.Models.Entities.Listing>, IHaveCustomMappings
    {
        public string PhotoUrl { get; set; } =
            "https://content.app-sources.com/s/85506704990285511/uploads/Balfour/3875-Balfour-Ct-36-4527457.jpg?format=webp";

        public int Price { get; set; }

        public string Address { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Data.Models.Entities.Listing, ListingIndexViewModel>()
                .ForMember(x => x.Address, opt =>
                    opt.MapFrom(x => x.Property.Address.StreetName + " " + x.Property.Address.ZipCode))
                .ForMember(x => x.PhotoUrl, opt =>
                    opt.MapFrom(x => x.Photos.FirstOrDefault().Url));
        }
    }
}
