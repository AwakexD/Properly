using System.Data;

namespace Properly.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Properly.Data.Common.Repositories;
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
                property.PropertyFeatures.Add(new PropertyFeature {PropertyId = property.Id, FeatureId = feature} );
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
                .Take(count)
                .ToListAsync();

            var listingViewModels = this.mapper.Map<IEnumerable<ListingIndexViewModel>>(listings);

            return listingViewModels;
        }

        public async Task<IEnumerable<BaseListingViewModel>> GetAll(int page, string type, int itemsPerPage = 6)
        {
            var listings = await this.listingRepository.AllAsNoTracking()
                .Where(l => l.ListingType.Name == type)
                .OrderByDescending(l => l.CreatedOn)
                .Skip((page - 1) * itemsPerPage).Take(itemsPerPage)
                .To<BaseListingViewModel>().ToListAsync();

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

        public int GetCount(string listingType)
        {
             int count = this.listingRepository
                .AllAsNoTracking()
                .Count(l => l.ListingType.Name == listingType);

             return count;
        }
    }
}
