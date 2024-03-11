namespace Properly.Web.ViewModels.Sell.Options
{
    using Properly.Data.Models;
    using Properly.Services.Mapping;

    public class PropertyTypeFormModel : IMapFrom<PropertyType>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
