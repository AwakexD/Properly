namespace Properly.Web.ViewModels.Common
{
    using AutoMapper;
    using Properly.Data.Models.Entities;
    using Properly.Services.Mapping;

    public class FeatureViewModel : IHaveCustomMappings
    {
        public string Name { get; set; }

        public string IconClass { get; set; }


        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Feature, FeatureViewModel>()
                .ForMember(fv => fv.Name, x => x.MapFrom(f => f.Name))
                .ForMember(fv => fv.IconClass, x => x.MapFrom(f => f.IconClass));
        }
    }
}
