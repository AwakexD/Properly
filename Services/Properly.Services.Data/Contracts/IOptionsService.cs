namespace Properly.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Properly.Web.ViewModels.Sell;
    using Properly.Web.ViewModels.Sell.Options;

    public interface IOptionsService
    {
        Task<IEnumerable<PropertyTypeFormModel>> GetPropertyTypes();

        Task<IEnumerable<ListingTypeFormModel>> GetListingTypes();

        Task<IEnumerable<FeatureFormModel>> GetFeatures();

        Task<ListingOptions> GetListingOptionsAsync();
    }
}
