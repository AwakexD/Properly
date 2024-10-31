namespace Properly.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Properly.Web.ViewModels.AdminListingOptions;

    public interface IAdminListingOptionsService
    {
        Task<IEnumerable<PropertyTypeAdminModel>> GetAllPropertyTypesAsync();

        Task<PropertyTypeAdminModel> GetPropertyTypeByIdAsync(int id);

        Task AddPropertyTypeAsync(PropertyTypeAdminModel model);

        Task UpdatePropertyTypeAsync(int id, PropertyTypeAdminModel model);

        Task DeletePropertyTypeAsync(int id, bool hardDelete);

        Task ActivatePropertyTypeAsync(int id);

        Task<IEnumerable<FeatureAdminModel>> GetAllFeaturesAsync();

        Task<FeatureAdminModel> GetFeatureByIdAsync(int id);

        Task AddFeatureAsync(FeatureAdminModel model);

        Task UpdateFeatureAsync(int id, FeatureAdminModel model);

        Task ActivateFeatureAsync(int id);

        Task DeleteFeatureAsync(int id, bool hardDelete);
    }
}
