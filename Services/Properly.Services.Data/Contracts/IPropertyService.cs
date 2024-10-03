﻿using Properly.Web.ViewModels.Buy;

namespace Properly.Services.Data.Contracts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Properly.Web.ViewModels.Common;
    using Properly.Web.ViewModels.Listing;
    using Properly.Web.ViewModels.Listing.Enums;
    using Properly.Web.ViewModels.Sell;

    public interface IPropertyService
    {
        Task<string> CreateListingAsync(CreateListingViewModel form, string userId);

        Task<IEnumerable<ListingIndexViewModel>> GetAllListingsByAddedDate(int count);

        Task<IEnumerable<BaseListingViewModel>> GetAll(int page, string type, ListingSorting sorting, int itemsPerPage = 6);

        Task<(IEnumerable<BaseListingViewModel>, int TotalCount)> GetAllAsync(BuyViewModel queryModel, int page, string type);

        Task<BaseListingViewModel> GetListingById(Guid id);

        Task<CreateListingViewModel> GetListingEditDataAsync(Guid listingId, string userId);

        Task<IEnumerable<BaseListingViewModel>> GetUserListings(string userId);

        Task<bool> UpdateListingAsync(CreateListingViewModel form, string listingId, string userId);

        Task<bool> DeactivateListing(string userId, string listingId);

        Task<bool> DeleteListingImage(string userId, string listingId, string imageUrl);

        Task<bool> ChangeListingStatus(string userId, string listingId, ListingStatus status);

        int GetCount(string type);

    }
}
