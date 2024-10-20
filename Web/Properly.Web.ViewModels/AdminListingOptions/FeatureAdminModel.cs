using Properly.Data.Models.Entities;
using Properly.Services.Mapping;

namespace Properly.Web.ViewModels.AdminListingOptions
{
    public class FeatureAdminModel : IMapFrom<Feature>
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsDeleted { get; set; }
    }
}
