﻿namespace Properly.Services.Data
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
        private readonly IDeletableEntityRepository<FavoriteListing> favoriteListingRepository;

        public PropertyService(
            IMapper mapper,
            ICloudinaryService cloudinaryService,
            IDeletableEntityRepository<Listing> listingRepository,
            IDeletableEntityRepository<FavoriteListing> favouriteListingRepository)
        {
            this.mapper = mapper;
            this.cloudinaryService = cloudinaryService;
            this.listingRepository = listingRepository;
            this.favoriteListingRepository = favouriteListingRepository;
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

            property.SelectedFeatures = selectedFeatureIds;

            var model = new CreateListingViewModel()
            {
                Listing = listing,
                Property = property,
                Address = address,
            };

            return model;
        }

        public async Task AddToFavouritesAsync(string listingId, string userId)
        {
            var favourite = await this.favoriteListingRepository.All().FirstOrDefaultAsync(x =>
                x.ListingId == new Guid(listingId) && x.UserId == userId);

            if (favourite is null)
            {
                favourite = new FavoriteListing { ListingId = new Guid(listingId), UserId = userId };

                await this.favoriteListingRepository.AddAsync(favourite);
            }
            else
            {
                this.favoriteListingRepository.HardDelete(favourite);
            }

            await this.favoriteListingRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<FavouritesDto>> GetUserFavourites(string userId)
        {
            var userFavourites = await this.favoriteListingRepository.AllAsNoTracking()
                .Where(x => x.UserId == userId)
                .Select(x => x.Listing)
                .To<FavouritesDto>()
                .ToListAsync();

            return userFavourites;
        }

        public async Task<IEnumerable<BaseListingViewModel>> GetUserListings(string userId)
        {
            return await this.listingRepository.AllAsNoTracking()
                .Where(l => l.CreatorId == userId)
                .To<BaseListingViewModel>()
                .ToListAsync();
        }

        public async Task<bool> UpdateListingAsync(CreateListingViewModel form, string listingId ,string userId)
        {
            var listing = await this.listingRepository.All()
                .Include(l => l.Property)
                .ThenInclude(p => p.PropertyFeatures)
                .Include(l => l.Property.Address)
                .Include(l => l.Photos)
                .FirstOrDefaultAsync(l => l.Id == new Guid(listingId));

            if (listing == null || listing.CreatorId != userId)
            {
                throw new UnauthorizedAccessException("You are not authorized to edit this listing.");
            }

            var address = this.mapper.Map<Address>(form.Address);
            var property = this.mapper.Map<Property>(form.Property);
            property.Address = address;

            listing.Property = property;

            // Update property features
            listing.Property.PropertyFeatures.Clear();
            foreach (var feature in form.Property.SelectedFeatures)
            {
                listing.Property.PropertyFeatures.Add(new PropertyFeature { PropertyId = listing.Property.Id, FeatureId = feature });
            }

            // Update listing details
            listing.Price = form.Listing.Price;
            listing.ListingTypeId = form.Listing.ListingTypeId;

            // Update photos
            foreach (var photo in form.UploadedPhotos)
            {
                string url = await this.cloudinaryService.UploadImageAsync(photo);
                listing.Photos.Add(new Photo() { Url = url });
            }

            this.listingRepository.Update(listing);
            await this.listingRepository.SaveChangesAsync();

            return true;
        }

        public async Task DeactivateListing(string userId, string listingId)
        {
            var listingToBeDeactivated = await this.listingRepository.All()
                .FirstAsync(l => l.CreatorId == userId && l.Id.ToString() == listingId);

            this.listingRepository.Delete(listingToBeDeactivated);

            await this.listingRepository.SaveChangesAsync();
        }

        public async Task ChangeListingStatus(string userId, string listingId, Web.ViewModels.Listing.Enums.ListingStatus status)
        {
            var listingStatusToBeChanged = await this.listingRepository.All()
                .FirstAsync(l => l.CreatorId == userId && l.Id.ToString() == listingId);

            if (listingStatusToBeChanged != null)
            {
                listingStatusToBeChanged.ListingStatusId = (int)status;
                this.listingRepository.Update(listingStatusToBeChanged);
            }

            await this.listingRepository.SaveChangesAsync();
        }

        public async Task<bool> IsInFavourites(string listingId, string userId)
        {
            return await this.favoriteListingRepository.AllAsNoTracking()
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
