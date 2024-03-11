namespace Properly.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Properly.Data.Common.Repositories;
    using Properly.Data.Models;
    using Properly.Services.Data.Contracts;
    using Properly.Web.ViewModels.Listing;
    using Properly.Web.ViewModels.Sell;

    public class PropertyService : IPropertyService
    {
        private readonly IDeletableEntityRepository<Listing> listingRepository;

        public PropertyService(
            IDeletableEntityRepository<Listing> listingRepository)
        {
            this.listingRepository = listingRepository;
        }

        public async Task<string> CreateListingAsync(SellFormModel form, string userId)
        {
            // TODO : Implement AutoMapper
            var address = new Address()
            {
                StreetName = form.StreetName,
                City = form.City,
                ZipCode = form.ZipCode,
                Country = form.Country,
            };

            var property = new Property()
            {
                Size = form.Size,
                Bathrooms = form.Bathrooms,
                Bedrooms = form.Bedrooms,
                Description = form.Description,
                ConstructionDate = form.ConstructionDate,
                Address = address,
                PropertyTypeId = form.PropertyType,
            };

            foreach (var feature in form.SelectedFeatures)
            {
                property.PropertyFeatures.Add(new PropertyFeature()
                {
                    Property = property,
                    FeatureId = feature,
                });
            }

            var listing = new Listing()
            {
                Price = form.Price,
                Property = property,
                ListingTypeId = form.ListingType,
                CreatorId = userId,
                ListingStatusId = 1,
            };

            await this.listingRepository.AddAsync(listing);
            await this.listingRepository.SaveChangesAsync();

            return property.Id.ToString();
        }

        public Task<ListingOptions> GetListingOptionsFromTheDb()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ListingIndexViewModel>> GetThreeListings()
        {
            throw new NotImplementedException();
        }
    }
}
