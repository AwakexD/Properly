﻿namespace Properly.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Properly.Web.ViewModels.Listing;
    using Properly.Web.ViewModels.Sell;

    public interface IPropertyService
    {
        Task<string> CreateListingAsync(SellFormModel form, string userId);

        Task<IEnumerable<ListingIndexViewModel>> GetThreeListings();
    }
}
