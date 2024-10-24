using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Properly.Web.ViewModels.Buy;
using Properly.Web.ViewModels.Common;
using Properly.Web.ViewModels.Listing;
using Properly.Web.ViewModels.Sell;

namespace Properly.Services.Data.Contracts
{
    public interface IAdminListingService
    {
        Task<(IEnumerable<BaseListingViewModel>, int TotalCount)> GetAllListingsAsync(AdminListingViewModel queryModel, int page);
        Task<CreateListingViewModel> GetListingEditDataAsync(Guid listingId);
        Task<bool> UpdateListingAsync(CreateListingViewModel form, string listingId);
        Task<bool> DeactivateListingAsync(string listingId);
        Task<bool> DeleteListingImageAsync(string listingId, string imageUrl);
        Task<bool> HardDeleteListingAsync(string listingId);
    }
}
