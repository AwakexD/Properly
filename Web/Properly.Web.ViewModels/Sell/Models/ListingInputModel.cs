namespace Properly.Web.ViewModels.Sell.Models
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using Properly.Services.Mapping;

    public class ListingInputModel : IHaveCustomMappings
    {
        public string Id { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "{0} is required.")]
        [Range(100, int.MaxValue, ErrorMessage = "{0} must be more than 100")]
        public int Price { get; set; }

        [Display(Name = "Listing type")]
        [Required(ErrorMessage = "Listing type is required.")]
        public int ListingTypeId { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ListingInputModel, Data.Models.Entities.Listing>()
                .ForMember(x => x.Price, opt => opt.MapFrom(s => s.Price))
                .ForMember(x => x.ListingTypeId, opt => opt.MapFrom(s => s.ListingTypeId))
                .ForMember(x => x.Id, opt => opt.Ignore());

            configuration.CreateMap<Data.Models.Entities.Listing, Web.ViewModels.Sell.Models.ListingInputModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.ToString()))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.ListingTypeId, opt => opt.MapFrom(src => src.ListingTypeId));
        }
    }
}
