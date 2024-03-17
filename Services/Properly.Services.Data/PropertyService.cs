namespace Properly.Services.Data
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Properly.Data.Common.Repositories;
    using Properly.Data.Models;
    using Properly.Services.Data.Contracts;
    using Properly.Web.ViewModels.Listing;
    using Properly.Web.ViewModels.Sell;

    public class PropertyService : IPropertyService
    {
        private readonly IMapper mapper;
        private readonly ICloudinaryService cloudinaryService;
        private readonly IDeletableEntityRepository<Listing> listingRepository;

        public PropertyService(
            IMapper mapper,
            ICloudinaryService cloudinaryService,
            IDeletableEntityRepository<Listing> listingRepository)
        {
            this.mapper = mapper;
            this.cloudinaryService = cloudinaryService;
            this.listingRepository = listingRepository;
        }

        public async Task<string> CreateListingAsync(CreateListingViewModel form, string userId)
        {
            var address = new Address()
            {
                StreetName = form.Address.StreetName,
                City = form.Address.City,
                ZipCode = form.Address.ZipCode,
                Country = form.Address.Country,
            };

            var property = new Property()
            {
                Size = form.Property.Size,
                Bathrooms = form.Property.Bathrooms,
                Bedrooms = form.Property.Bedrooms,
                Description = form.Property.Description,
                ConstructionDate = form.Property.ConstructionDate,
                Address = address,
                PropertyTypeId = form.Property.PropertyTypeId,
            };

            foreach (var feature in form.Property.SelectedFeatures)
            {
                property.PropertyFeatures.Add(new PropertyFeature()
                {
                    Property = property,
                    FeatureId = feature,
                });
            }

            var listing = new Listing()
            {
                Price = form.Listing.Price,
                Property = property,
                ListingTypeId = form.Listing.ListingTypeId,
                CreatorId = userId,
                ListingStatusId = 1,
            };

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
                .OrderByDescending(x => x.CreatedOn)
                .Include(l => l.Property.Address)
                .Take(count)
                .ToListAsync();

            var listingViewModels = this.mapper.Map<IEnumerable<ListingIndexViewModel>>(listings);

            return listingViewModels;
        }
    }
}
