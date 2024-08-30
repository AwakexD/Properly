namespace Properly.Web.ViewModels.Sell.Options
{
    using AutoMapper;
    using Properly.Data.Models.Entities;
    using Properly.Services.Mapping;
    using Properly.Web.ViewModels.Common;

    public class FeatureFormModel : IHaveCustomMappings
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string IconClass { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Feature, FeatureFormModel>()
                .ForMember(fv => fv.Id, x => x.MapFrom(f => f.Id))
                .ForMember(fv => fv.Name, x => x.MapFrom(f => f.Name))
                .ForMember(fv => fv.IconClass, x => x.MapFrom(f => f.IconClass));
        }
    }
}
