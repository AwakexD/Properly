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
        private readonly IDeletableEntityRepository<Listing> listingRepository;

        public PropertyService(
            IMapper mapper,
            IDeletableEntityRepository<Listing> listingRepository)
        {
            this.mapper = mapper;
            this.listingRepository = listingRepository;
        }

        public async Task<string> CreateListingAsync(SellFormModel form, string userId)
        {
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

            return listing.Id.ToString();
        }

        public async Task<IEnumerable<ListingIndexViewModel>> GetAllByAddDate(int count)
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
