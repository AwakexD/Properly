namespace Properly.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Properly.Web.ViewModels.Common;
    using Properly.Web.ViewModels.Listing;
    using Properly.Web.ViewModels.Sell;

    public interface IPropertyService
    {
        Task<string> CreateListingAsync(CreateListingViewModel form, string userId);

        Task<IEnumerable<ListingIndexViewModel>> GetAllListingsByAddedDate(int count);

        Task<IEnumerable<BaseListingViewModel>> GetAll(int page, string type, int itemsPerPage = 6);

        Task<BaseListingViewModel> GetListingById(Guid id);

        int GetCount(string type);

    }
}
