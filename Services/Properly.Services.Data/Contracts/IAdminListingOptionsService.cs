using System.Collections.Generic;
using System.Threading.Tasks;
using Properly.Web.ViewModels.AdminListingOptions;

namespace Properly.Services.Data.Contracts
{
    public interface IAdminListingOptionsService
    {
        Task<IEnumerable<PropertyTypeAdminModel>> GetAllPropertyTypesAsync();

        Task<PropertyTypeAdminModel> GetPropertyTypeByIdAsync(int id);

        Task<IEnumerable<FeatureAdminModel>> GetAllFeaturesAsync();

        Task AddPropertyTypeAsync(PropertyTypeAdminModel model);

        Task UpdatePropertyTypeAsync(int id, PropertyTypeAdminModel model);

        Task DeletePropertyTypeAsync(int id, bool hardDelete);

        Task AddFeatureAsync(FeatureAdminModel model);

        Task UpdateFeatureAsync(FeatureAdminModel model);

        Task DeleteFeatureAsync(int id, bool hardDelete);
    }
}
