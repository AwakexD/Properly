using System.Collections.Generic;
using System.Threading.Tasks;
using Properly.Web.ViewModels.Common;
using Properly.Web.ViewModels.Listing;

namespace Properly.Services.Data.Contracts
{
    public interface IAdminListingService
    {
        Task<(IEnumerable<BaseListingViewModel>, int TotalCount)> GetAllListingsAsync(AdminListingViewModel queryModel, int page);

        Task SoftDeleteListingAsync(string listingId);

        Task UndeleteListingAsync(string listingId);
    }
}
