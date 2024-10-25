using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using Microsoft.EntityFrameworkCore;
using Properly.Data.Common.Repositories;
using Properly.Data.Models.Entities;
using Properly.Services.Data.Contracts;
using Properly.Services.Mapping;
using Properly.Web.ViewModels.Buy;
using Properly.Web.ViewModels.Common;
using Properly.Web.ViewModels.Listing;
using Properly.Web.ViewModels.Listing.Enums;
using Properly.Web.ViewModels.Sell;
using ListingStatus = Properly.Web.ViewModels.Listing.Enums.ListingStatus;
using ListingType = Properly.Web.ViewModels.Listing.Enums.ListingType;

namespace Properly.Services.Data.Admin 
{
    public class AdminListingService : IAdminListingService
    {
        private readonly ICloudinaryService cloudinaryService;
        private readonly IDeletableEntityRepository<Listing> listingRepository;
        private readonly IDeletableEntityRepository<FavoriteListing> favouriteListingRepository;
        private readonly IDeletableEntityRepository<PropertyFeature> propertyFeatureRepository;
        private readonly IDeletableEntityRepository<Photo> photoRepository;

        public AdminListingService(
            ICloudinaryService cloudinaryService,
            IDeletableEntityRepository<Listing> listingRepository,
            IDeletableEntityRepository<FavoriteListing> favouriteListingRepository,
            IDeletableEntityRepository<PropertyFeature> propertyFeatureRepository,
            IDeletableEntityRepository<Photo> photoRepository)
        {
            this.cloudinaryService = cloudinaryService;
            this.listingRepository = listingRepository;
            this.favouriteListingRepository = favouriteListingRepository;
            this.propertyFeatureRepository = propertyFeatureRepository;
            this.photoRepository = photoRepository;
        }

        public async Task<(IEnumerable<BaseListingViewModel>, int TotalCount)> GetAllListingsAsync(AdminListingViewModel queryModel, int page)
        {
            var query = this.listingRepository.AllAsNoTrackingWithDeleted();

            query = queryModel.ListingType switch
            {
                ListingType.ForSale => query.Where(l => l.ListingType.Name == "For Sale"),
                ListingType.ForRent => query.Where(l => l.ListingType.Name == "For Rent"),
                ListingType.All => query
            };

            query = queryModel.ListingStatus switch
            {
                ListingStatus.Active => query.Where(l => l.ListingStatus.Name == ListingStatus.Active.ToString()),
                ListingStatus.Pending => query.Where(l => l.ListingStatus.Name == ListingStatus.Pending.ToString()),
                ListingStatus.Sold => query.Where(l => l.ListingStatus.Name == "Sold"),
                ListingStatus.Rented => query.Where(l => l.ListingStatus.Name == "Rented"),
                ListingStatus.Withdrawn => query.Where(l => l.ListingStatus.Name == ListingStatus.Withdrawn.ToString()),
                ListingStatus.ComingSoon => query.Where(l => l.ListingStatus.Name == ListingStatus.ComingSoon.ToString()),
                ListingStatus.OffMarket => query.Where(l => l.ListingStatus.Name == ListingStatus.OffMarket.ToString()),
                _ => query
            };

            int totalCount = await query.CountAsync();

            query = queryModel.ListingSorting switch
            {
                ListingSorting.Newest => query.OrderByDescending(l => l.CreatedOn),
                ListingSorting.Oldest => query.OrderBy(l => l.CreatedOn),
                ListingSorting.AscendingPrice => query.OrderBy(l => l.Price),
                ListingSorting.DescendingPrice => query.OrderByDescending(l => l.Price),
                _ => query.OrderByDescending(l => l.CreatedOn),
            };

            var listings = await query
                .Skip((page - 1) * queryModel.ItemsPerPage)
                .Take(queryModel.ItemsPerPage)
                .To<BaseListingViewModel>()
                .ToListAsync();

            return (listings, totalCount);
        }

        public Task<CreateListingViewModel> GetListingEditDataAsync(Guid listingId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateListingAsync(CreateListingViewModel form, string listingId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeactivateListingAsync(string listingId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteListingImageAsync(string listingId, string imageUrl)
        {
            throw new NotImplementedException();
        }

        public Task<bool> HardDeleteListingAsync(string listingId)
        {
            throw new NotImplementedException();
        }
    }
}
