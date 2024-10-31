namespace Properly.Services.Data.Admin 
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using System.Linq;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
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
    using Properly.Web.ViewModels.Sell.Models;

    public class AdminListingService : IAdminListingService
    {
        private readonly ICloudinaryService cloudinaryService;
        private readonly IDeletableEntityRepository<Listing> listingRepository;
        private readonly IDeletableEntityRepository<PropertyFeature> propertyFeatureRepository;
        private readonly IDeletableEntityRepository<Photo> photoRepository;
        private readonly ILogger<AdminListingService> logger;

        public AdminListingService(
            ICloudinaryService cloudinaryService,
            IDeletableEntityRepository<Listing> listingRepository,
            IDeletableEntityRepository<PropertyFeature> propertyFeatureRepository,
            IDeletableEntityRepository<Photo> photoRepository,
            ILogger<AdminListingService> logger)
        {
            this.cloudinaryService = cloudinaryService;
            this.listingRepository = listingRepository;
            this.propertyFeatureRepository = propertyFeatureRepository;
            this.photoRepository = photoRepository;
            this.logger = logger;
        }

        public async Task<(IEnumerable<BaseListingViewModel>, int TotalCount)> GetAllListingsAsync(AdminListingViewModel queryModel, int page)
        {
            try
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
            catch (Exception e)
            {
                logger.LogError(e, "Error retrieving listings.");
                throw;
            }
        }

        public async Task<CreateListingViewModel> GetListingEditDataAsync(Guid listingId)
        {
            try
            {
                var listing = await this.listingRepository.AllAsNoTracking()
                    .Where(x => x.Id == listingId)
                    .Include(x => x.Photos)
                    .To<ListingInputModel>()
                    .FirstOrDefaultAsync();

                var property = await this.listingRepository.AllAsNoTracking()
                    .Where(x => x.Id == listingId)
                    .Select(x => x.Property)
                    .To<PropertyInputModel>()
                    .FirstOrDefaultAsync();

                var address = await this.listingRepository.AllAsNoTracking()
                    .Where(x => x.Id == listingId)
                    .Select(x => x.Property.Address)
                    .To<AddressInputModel>()
                    .FirstOrDefaultAsync();

                var selectedFeatureIds = await this.listingRepository.AllAsNoTracking()
                    .Where(x => x.Id == listingId)
                    .SelectMany(x => x.Property.PropertyFeatures.Select(pf => pf.FeatureId))
                    .ToListAsync();

                var listingUploadedPhotos = await this.photoRepository.AllAsNoTracking()
                    .Where(x => x.ListingId == listingId)
                    .Select(x => x.Url)
                    .ToListAsync();

                property.SelectedFeatures = selectedFeatureIds;

                var model = new CreateListingViewModel()
                {
                    Listing = listing,
                    Property = property,
                    Address = address,
                    UploadedPhotoUrls = listingUploadedPhotos
                };

                return model;
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error retrieving edit data for listing ID: {listingId}");
                throw;
            }
        }

        public async Task<BaseListingViewModel> GetListingDataAsync(string listingId)
        {
            try
            {
                var listing = await this.listingRepository.AllWithDeleted()
                    .To<BaseListingViewModel>()
                    .FirstOrDefaultAsync(l => l.Id == new Guid(listingId));

                if (listing == null)
                {
                    throw new InvalidOperationException($"Error retrieving data for listing ID: {listingId}");
                }

                return listing;
            }
            catch (Exception ex)
            {
                logger.LogError(ex, $"Error retrieving data for listing ID: {listingId}");
                throw;
            }
        }

        public async Task<bool> UpdateListingAsync(CreateListingViewModel form, string listingId)
        {
            try
            {
                var listingGuid = new Guid(listingId);

                var listing = await this.listingRepository.All()
                    .Include(l => l.Property)
                    .ThenInclude(p => p.PropertyFeatures)
                    .Include(l => l.Property.Address)
                    .Include(l => l.Photos)
                    .FirstOrDefaultAsync(l => l.Id == listingGuid);

                if (listing == null)
                {
                    throw new UnauthorizedAccessException("You are not authorized to edit this listing.");
                }

                listing.Property.Size = form.Property.Size;
                listing.Property.Bathrooms = form.Property.Bathrooms;
                listing.Property.Bedrooms = form.Property.Bedrooms;
                listing.Property.Description = form.Property.Description;
                listing.Property.ConstructionDate = form.Property.ConstructionDate;
                listing.Property.PropertyTypeId = form.Property.PropertyTypeId;

                listing.Property.Address.StreetName = form.Address.StreetName;
                listing.Property.Address.City = form.Address.City;
                listing.Property.Address.ZipCode = form.Address.ZipCode;
                listing.Property.Address.Country = form.Address.Country;

                foreach (var feature in form.Property.SelectedFeatures)
                {
                    if (listing.Property.PropertyFeatures.All(x => x.FeatureId != feature))
                    {
                        await this.propertyFeatureRepository.AddAsync(new PropertyFeature()
                            { PropertyId = listing.Property.Id, FeatureId = feature });
                    }
                }

                listing.Price = form.Listing.Price;
                listing.ListingTypeId = form.Listing.ListingTypeId;

                foreach (var photo in form.UploadedPhotos)
                {
                    string url = await this.cloudinaryService.UploadImageAsync(photo);
                    await this.photoRepository.AddAsync(new Photo() { Url = url, ListingId = listing.Id });
                }

                this.listingRepository.Update(listing);

                await this.listingRepository.SaveChangesAsync();

                return true;
            }
            catch (DbUpdateException e)
            {
                logger.LogError(e, $"An error occurred while updating listing with ID : {listingId}");
                throw new InvalidOperationException("Could not update listing.");
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Unexpected error in {nameof(UpdateListingAsync)} for Listing ID : {listingId}");
                throw;
            }
        }

        public async Task SoftDeleteListingAsync(string listingId)
        {
            try
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
            catch (Exception e)
            {
                logger.LogError(e, $"Error soft deleting listing ID: {listingId}");
                throw;
            }
        }

        public async Task UndeleteListingAsync(string listingId)
        {
            try
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
            catch (Exception e)
            {
                logger.LogError(e, $"Error undeleting listing ID : {listingId}");
                throw;
            }
        }

        public async Task UpdateListingStatus(string listingId, ListingStatus listingStatus)
        {
            try
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
            catch (Exception e)
            {
                logger.LogError(e, $"Error updating status of a listing ID : {listingId}.");
                throw;
            }
        }
    }
}
