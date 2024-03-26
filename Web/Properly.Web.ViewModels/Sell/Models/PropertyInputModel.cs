namespace Properly.Web.ViewModels.Sell.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using AutoMapper;
    using Properly.Data.Models.Entities;
    using Properly.Services.Mapping;

    using static Properly.Common.EntityConstants.PropertyConstants;

    public class PropertyInputModel : IHaveCustomMappings
    {
        [Display(Name = "Size")]
        [Required(ErrorMessage = "{0} is required.")]
        [Range(SizeMinLength, SizeMaxLength, ErrorMessage = "Property size must be between {1} and {2} sqft.")]
        public int Size { get; set; }

        [Display(Name = "Bathrooms")]
        [Range(0, BathroomsMaxLength)]
        public int? Bathrooms { get; set; }

        [Display(Name = "Bathrooms")]
        [Range(0, BathroomsMaxLength)]
        public int? Bedrooms { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "{0} is required.")]
        [DataType(DataType.MultilineText)]
        [StringLength(DescriptionMaxLength, MinimumLength = DescriptionMinLength, ErrorMessage = "{0} must be between {2} and {1} characters.")]
        public string Description { get; set; }

        [Display(Name = "Construction Date")]
        [Required(ErrorMessage = "{0} is required.")]
        [DataType(DataType.Date)]
        public DateTime ConstructionDate { get; set; }

        [Display(Name = "Property type")]
        [Required(ErrorMessage = "{0} is required")]
        public int PropertyTypeId { get; set; }

        public IEnumerable<int> SelectedFeatures { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<PropertyInputModel, Property>()
                .ForMember(x => x.Size, opt => opt.MapFrom(s => s.Size))
                .ForMember(x => x.Bathrooms, opt => opt.MapFrom(s => s.Bathrooms))
                .ForMember(x => x.Bedrooms, opt => opt.MapFrom(s => s.Bedrooms))
                .ForMember(x => x.Description, opt => opt.MapFrom(s => s.Description))
                .ForMember(x => x.ConstructionDate, opt => opt.MapFrom(s => s.ConstructionDate))
                .ForMember(x => x.PropertyTypeId, opt => opt.MapFrom(s => s.PropertyTypeId));
        }
    }
}