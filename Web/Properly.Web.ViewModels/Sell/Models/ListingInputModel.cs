namespace Properly.Web.ViewModels.Sell.Models
{
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using Properly.Data.Models;
    using Properly.Services.Mapping;

    public class ListingInputModel : IHaveCustomMappings
    {
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
                .ForMember(x => x.ListingTypeId, opt => opt.MapFrom(s => s.ListingTypeId));
        }
    }
}
