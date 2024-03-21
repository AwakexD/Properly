namespace Properly.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Properly.Web.ViewModels.Listing;
    using Properly.Web.ViewModels.Sell;

    public interface IPropertyService
    {
        Task<string> CreateListingAsync(CreateListingViewModel form, string userId);

        Task<IEnumerable<ListingIndexViewModel>> GetAllListingsByAddedDate(int count);

        Task<IEnumerable<ListingInListViewModel>> GetAll(int page, int itemsPerPage = 8);

        int GetCount(string type);
    }
}
