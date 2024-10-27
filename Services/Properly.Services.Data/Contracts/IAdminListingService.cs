using System.Collections.Generic;
using System.Threading.Tasks;
using Properly.Web.ViewModels.Common;
using Properly.Web.ViewModels.Listing;
using Properly.Web.ViewModels.Listing.Enums;
using Properly.Web.ViewModels.Sell;

namespace Properly.Services.Data.Contracts
{
    public interface IAdminListingService
    {
        Task<(IEnumerable<BaseListingViewModel>, int TotalCount)> GetAllListingsAsync(AdminListingViewModel queryModel, int page);

        Task<BaseListingViewModel> GetListingEditDataAsync(string listingId);

        Task SoftDeleteListingAsync(string listingId);

        Task UndeleteListingAsync(string listingId);

        Task UpdateListingStatus(string listingId, ListingStatus listingStatus);
    }
}
