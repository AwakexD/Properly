namespace Properly.Web.ViewModels.Sell.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using Properly.Services.Mapping;
    using Properly.Data.Models.Entities;

    public class ListingInputModel : IHaveCustomMappings
    {
        public Guid Id { get; set; }

        [Display(Name = "Price")]
        [Required(ErrorMessage = "{0} is required.")]
        [Range(100, int.MaxValue, ErrorMessage = "{0} must be more than 100")]
        public int Price { get; set; }

        [Display(Name = "Listing type")]
        [Required(ErrorMessage = "Listing type is required.")]
        public int ListingTypeId { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<ListingInputModel, Listing>()
                .ForMember(x => x.Price, opt => opt.MapFrom(s => s.Price))
                .ForMember(x => x.ListingTypeId, opt => opt.MapFrom(s => s.ListingTypeId))
                .ForMember(x => x.Id, opt => opt.Ignore());

            configuration.CreateMap<Listing, ListingInputModel>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.ListingTypeId, opt => opt.MapFrom(src => src.ListingTypeId));
        }
    }
}
