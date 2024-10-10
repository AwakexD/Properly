using Properly.Common;
using Properly.Web.ViewModels.Buy;
using Properly.Web.ViewModels.Home;

namespace Properly.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Properly.Data.Common.Repositories;
    using Properly.Web.ViewModels.Sell.Models;
    using Properly.Web.ViewModels.Listing.Enums;
    using Properly.Data.Models.Entities;
    using Properly.Services.Data.Contracts;
    using Properly.Services.Mapping;
    using Properly.Web.ViewModels.Common;
    using Properly.Web.ViewModels.Listing;
    using Properly.Web.ViewModels.Sell;

    public class PropertyService : IPropertyService
    {
        private readonly IMapper mapper;
        private readonly ICloudinaryService cloudinaryService;
        private readonly IDeletableEntityRepository<Listing> listingRepository;
        private readonly IDeletableEntityRepository<FavoriteListing> favouriteListingRepository;
        private readonly IDeletableEntityRepository<PropertyFeature> propertyFeatureRepository;
        private readonly IDeletableEntityRepository<Photo> photoRepository;

        public PropertyService(
            IMapper mapper,
            ICloudinaryService cloudinaryService,
            IDeletableEntityRepository<Listing> listingRepository,
            IDeletableEntityRepository<FavoriteListing> favouriteListingRepository,
            IDeletableEntityRepository<PropertyFeature> propertyFeatureRepository,
            IDeletableEntityRepository<Photo> photoRepository)
        {
            this.mapper = mapper;
            this.cloudinaryService = cloudinaryService;
            this.listingRepository = listingRepository;
            this.favouriteListingRepository = favouriteListingRepository;
            this.propertyFeatureRepository = propertyFeatureRepository;
            this.photoRepository = photoRepository;
        }

        public async Task<string> CreateListingAsync(CreateListingViewModel form, string userId)
        {
            var address = this.mapper.Map<Address>(form.Address);
            var property = this.mapper.Map<Property>(form.Property);
            property.Address = address;

            foreach (var feature in form.Property.SelectedFeatures)
            {
                property.PropertyFeatures.Add(new PropertyFeature { PropertyId = property.Id, FeatureId = feature } );
            }

            var listing = this.mapper.Map<Listing>(form.Listing);
            listing.CreatorId = userId;
            listing.Property = property;
            listing.ListingStatusId = 1;

            foreach (var photo in form.UploadedPhotos)
            {
                string url = await this.cloudinaryService.UploadImageAsync(photo);
                listing.Photos.Add(new Photo() { Url = url });
            }

            await this.listingRepository.AddAsync(listing);
            await this.listingRepository.SaveChangesAsync();

            return listing.Id.ToString();
        }

        public async Task<IEnumerable<ListingIndexViewModel>> GetAllListingsByAddedDate(int count)
        {
            var listings = await this.listingRepository.AllAsNoTracking()
                .OrderByDescending(l => l.CreatedOn)
                .Include(l => l.Property.Address)
                .Include(l => l.Photos)
                .Include(l => l.Property.PropertyType)
                .Include(l => l.ListingType)
                .Take(count)
                .ToListAsync();

            var listingViewModels = this.mapper.Map<IEnumerable<ListingIndexViewModel>>(listings);

            return listingViewModels;
        }

        public async Task<IEnumerable<BaseListingViewModel>> GetAll(int page, string type, ListingSorting sorting, int itemsPerPage = 6)
        {
            IQueryable<Listing> query = this.listingRepository.AllAsNoTracking()
                .Where(l => l.ListingType.Name == type);

            query = sorting switch
            {
                ListingSorting.Newest => query.OrderByDescending(l => l.CreatedOn),
                ListingSorting.Oldest => query.OrderBy(l => l.CreatedOn),
                ListingSorting.AscendingPrice => query.OrderBy(l => l.Price),
                ListingSorting.DescendingPrice => query.OrderByDescending(l => l.Price),
                _ => query.OrderByDescending(l => l.CreatedOn),
            };

            var listings = await query
                .Skip((page - 1) * itemsPerPage)
                .Take(itemsPerPage)
                .To<BaseListingViewModel>()
                .ToListAsync();

            return listings;
        }

        public async Task<(IEnumerable<BaseListingViewModel>, int TotalCount)> GetAllAsync(BuyViewModel queryModel, int page, string type)
        {
            var query = this.listingRepository.AllAsNoTracking()
                .Where(l => l.ListingType.Name == type);


            if (!string.IsNullOrWhiteSpace(queryModel.Keyword))
            {
                query = query.Where(l => l.Property.Description.Contains(queryModel.Keyword));
            }

            if (!string.IsNullOrWhiteSpace(queryModel.Location))
            {
                string location = queryModel.Location.Trim().ToLower();

                query = query.Where(l =>
                    l.Property.Address.City.ToLower().Contains(location) ||
                    l.Property.Address.StreetName.ToLower().Contains(location) ||
                    l.Property.Address.Country.ToLower().Contains(location) ||
                    l.Property.Address.ZipCode.ToLower().Contains(location));
            }

            if (queryModel.PropertyType.HasValue)
            {
                query = query.Where(l => l.Property.PropertyType.Id == queryModel.PropertyType.Value);
            }

            if (queryModel.MinPrice.HasValue)
            {
                query = query.Where(l => l.Price >= queryModel.MinPrice.Value);
            }

            if (queryModel.MaxPrice.HasValue)
            {
                query = query.Where(l => l.Price <= queryModel.MaxPrice.Value);
            }

            if (queryModel.MinSize.HasValue)
            {
                query = query.Where(l => l.Property.Size >= queryModel.MinSize.Value);
            }

            if (queryModel.MaxSize.HasValue)
            {
                query = query.Where(l => l.Property.Size <= queryModel.MaxSize.Value);
            }

            if (queryModel.Bedrooms.HasValue)
            {
                query = query.Where(l => l.Property.Bedrooms == queryModel.Bedrooms.Value);
            }

            if (queryModel.Bathrooms.HasValue)
            {
                query = query.Where(l => l.Property.Bathrooms == queryModel.Bathrooms.Value);
            }

            if (queryModel.Features != null && queryModel.Features.Any())
            {
                query = query.Where(l =>
                    l.Property.PropertyFeatures.Any(f => queryModel.Features.Contains(f.Feature.Id))
                );
            }

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

        public async Task<BaseListingViewModel> GetListingById(Guid id)
        {
            var listing = await this.listingRepository.AllAsNoTracking()
                .Where(x => x.Id == id)
                .To<BaseListingViewModel>()
                .FirstOrDefaultAsync();

            return listing;
        }

        public async Task<CreateListingViewModel> GetListingEditDataAsync(Guid listingId, string userId)
        {
            var listing = await this.listingRepository.AllAsNoTracking()
                .Where(x => x.Id == listingId && x.CreatorId == userId)
                .Include(x => x.Photos)
                .To<ListingInputModel>()
                .FirstOrDefaultAsync();

            var property = await this.listingRepository.AllAsNoTracking()
                .Where(x => x.Id == listingId && x.CreatorId == userId)
                .Select(x => x.Property)
                .To<PropertyInputModel>()
                .FirstOrDefaultAsync();

            var address = await this.listingRepository.AllAsNoTracking()
                .Where(x => x.Id == listingId && x.CreatorId == userId)
                .Select(x => x.Property.Address)
                .To<AddressInputModel>()
                .FirstOrDefaultAsync();

            var selectedFeatureIds = await this.listingRepository.AllAsNoTracking()
                .Where(x => x.Id == listingId && x.CreatorId == userId)
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

        public async Task<IEnumerable<BaseListingViewModel>> GetUserListings(string userId)
        {
            return await this.listingRepository.AllAsNoTracking()
                .Where(l => l.CreatorId == userId)
                .OrderByDescending(l => l.CreatedOn)
                .To<BaseListingViewModel>()
                .ToListAsync();
        }

        public async Task<bool> UpdateListingAsync(CreateListingViewModel form, string listingId ,string userId)
        {
            var listingGuid = new Guid(listingId);

            var listing = await this.listingRepository.All()
                .Include(l => l.Property)
                .ThenInclude(p => p.PropertyFeatures)
                .Include(l => l.Property.Address)
                .Include(l => l.Photos)
                .FirstOrDefaultAsync(l => l.Id == listingGuid && l.CreatorId == userId);

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
                await this.photoRepository.AddAsync(new Photo() { Url = url, ListingId = listing.Id});
            }

            this.listingRepository.Update(listing);

            await this.listingRepository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeactivateListing(string userId, string listingId)
        {
            var listingToBeDeactivated = await this.listingRepository.All()
                .FirstAsync(l => l.CreatorId == userId && l.Id.ToString() == listingId);

            if (listingToBeDeactivated == null)
            {
                return false;
            }

            this.listingRepository.Delete(listingToBeDeactivated);

            // SaveChanges() return affected rows
            var saveResult = await this.listingRepository.SaveChangesAsync();

            return saveResult > 0;
        }

        public async Task<bool> DeleteListingImage(string userId, string listingId, string imageUrl)
        {
            var imageToDelete = await this.photoRepository.All()
                .FirstOrDefaultAsync(p => p.ListingId == new Guid(listingId) && p.Url == imageUrl);

            if (imageToDelete == null)
            {
                throw new InvalidOperationException(ExceptionsAndNotificationsMessages.ImageDoesNotExistError);
            }

            string imagePublicId = imageUrl.Split(new[] { "upload/" }, StringSplitOptions.None)[1]
                .Split(new[] { '/' }, 2)[1]
                .Replace(".webp", "");

            var deleteResult = await this.cloudinaryService.DeleteImageAsync([imagePublicId]);

            if (!deleteResult)
            {
                throw new InvalidOperationException(ExceptionsAndNotificationsMessages.ImageDeleteUnsuccessful);
            }

            this.photoRepository.HardDelete(imageToDelete);
            await this.photoRepository.SaveChangesAsync();

            return deleteResult;
        }

        public async Task<bool> ChangeListingStatus(string userId, string listingId, Web.ViewModels.Listing.Enums.ListingStatus status)
        {
            var listingStatusToBeChanged = await this.listingRepository.All()
                .FirstAsync(l => l.CreatorId == userId && l.Id.ToString() == listingId);

            if (listingStatusToBeChanged != null)
            {
                listingStatusToBeChanged.ListingStatusId = (int)status;
                this.listingRepository.Update(listingStatusToBeChanged);
            }

            var saveResult =  await this.listingRepository.SaveChangesAsync();

            return saveResult > 0;
        }

        public async Task<bool> AddToFavoritesAsync(string userId, string listingId)
        {
            var listing = await this.favouriteListingRepository.AllAsNoTracking()
                .FirstOrDefaultAsync(x => x.ListingId == new Guid(listingId) && x.UserId == userId);

            if (listing != null)
            {
                return false;
            }

            var listingToAdd = new FavoriteListing()
            {
                ListingId = new Guid(listingId),
                UserId = userId,
            };

            await this.favouriteListingRepository.AddAsync(listingToAdd);
            var saveResult = await this.favouriteListingRepository.SaveChangesAsync();

            return saveResult > 0;
        }

        public async Task<bool> RemoveFromFavoritesAsync(string userId, string listingId)
        {
            var listing = await this.favouriteListingRepository.All()
                .FirstOrDefaultAsync(x => x.ListingId == new Guid(listingId) && x.UserId == userId);

            if (listing == null)
            {
                return false;
            }

            this.favouriteListingRepository.HardDelete(listing);
            var saveResult = await this.favouriteListingRepository.SaveChangesAsync();

            return saveResult > 0;
        }

        public async Task<bool> IsFavoriteAsync(string userId, string listingId)
        {
            return await this.favouriteListingRepository.AllAsNoTracking()
                .AnyAsync(x => x.ListingId == new Guid(listingId) && x.UserId == userId);
        }


        public int GetCount(string listingType)
        {
             int count = this.listingRepository
                .AllAsNoTracking()
                .Count(l => l.ListingType.Name == listingType);

             return count;
        }
    }
}
