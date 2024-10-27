using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading;
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
                ListingStatus.Sold => query.Where(l => l.ListingStatus.Name == ListingStatus.Sold.ToString()),
                ListingStatus.Rented => query.Where(l => l.ListingStatus.Name == ListingStatus.Rented.ToString()),
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

        public async Task<BaseListingViewModel> GetListingEditDataAsync(string listingId)
        {
            var listing = await this.listingRepository.AllWithDeleted()
                .To<BaseListingViewModel>()
                .FirstOrDefaultAsync(l => l.Id == new Guid(listingId));

            if (listing == null)
            {
                throw new ArgumentNullException($"Listing with ID {listingId} not found.");
            }

            return listing;
        }

        public async Task SoftDeleteListingAsync(string listingId)
        {
            var listing = await this.listingRepository.AllWithDeleted()
                .FirstOrDefaultAsync(l => l.Id == new Guid(listingId));

            if (listing == null)
            {
                throw new ArgumentNullException($"Listing with ID {listingId} not found.");
            }

            listing.ListingStatusId = (int)ListingStatus.OffMarket;

            this.listingRepository.Delete(listing);
            var result = await this.listingRepository.SaveChangesAsync();
        }

        public async Task UndeleteListingAsync(string listingId)
        {
            var listing = await this.listingRepository.AllWithDeleted()
                .FirstOrDefaultAsync(l => l.Id == new Guid(listingId));

            if (listing == null)
            {
                throw new ArgumentNullException($"Listing with ID {listingId} not found.");
            }

            listing.ListingStatusId = (int)ListingStatus.Active;
            listingRepository.Undelete(listing);
            await this.listingRepository.SaveChangesAsync();
        }

        public async Task UpdateListingStatus(string listingId, ListingStatus listingStatus)
        {
            var listing = await this.listingRepository.AllWithDeleted()
                .FirstOrDefaultAsync(l => l.Id == new Guid(listingId));

            if (listing == null)
            {
                throw new ArgumentNullException($"Listing with ID {listingId} not found.");
            }

            listing.ListingStatusId = (int)listingStatus;
            listingRepository.Update(listing);
            await this.listingRepository.SaveChangesAsync();
        }
    }
}
