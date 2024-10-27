using Properly.Web.ViewModels.Listing;

namespace Properly.Services.Data
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Properly.Data.Common.Repositories;
    using Properly.Data.Models.Entities;
    using Properly.Services.Data.Contracts;
    using Properly.Services.Mapping;
    using Properly.Web.ViewModels.Sell;
    using Properly.Web.ViewModels.Sell.Options;

    public class ListingOptionsService : IListingOptionsService
    {
        private readonly IDeletableEntityRepository<PropertyType> propertyTypesRepository;
        private readonly IDeletableEntityRepository<ListingType> listingTypesRepository;
        private readonly IDeletableEntityRepository<Feature> featuresRepository;
        private readonly IDeletableEntityRepository<ListingStatus> listingStatusesRepository;

        public ListingOptionsService(
            IDeletableEntityRepository<PropertyType> propertyTypesRepository,
            IDeletableEntityRepository<ListingType> listingTypesRepository,
            IDeletableEntityRepository<Feature> featuresRepository,
            IDeletableEntityRepository<ListingStatus> listingStatusesRepository)
        {
            this.propertyTypesRepository = propertyTypesRepository;
            this.listingTypesRepository = listingTypesRepository;
            this.featuresRepository = featuresRepository;
            this.listingStatusesRepository = listingStatusesRepository;
        }

        public async Task<IEnumerable<PropertyTypeFormModel>> GetPropertyTypes()
        {
            if (!await this.propertyTypesRepository.All().AnyAsync())
            {
                return null;
            }

            var propertyTypeFormModels = await this.propertyTypesRepository.AllAsNoTracking()
                .To<PropertyTypeFormModel>().ToListAsync();

            return propertyTypeFormModels;
        }

        public async Task<IEnumerable<ListingTypeFormModel>> GetListingTypes()
        {
            if (!await this.listingTypesRepository.All().AnyAsync())
            {
                return null;
            }

            var listingTypesFormModels =
                await this.listingTypesRepository.AllAsNoTracking().To<ListingTypeFormModel>().ToListAsync();

            return listingTypesFormModels;
        }

        public async Task<IEnumerable<FeatureFormModel>> GetFeatures()
        {
            if (!await this.featuresRepository.All().AnyAsync())
            {
                return null;
            }

            var featuresFormModels =
                await this.featuresRepository.AllAsNoTracking().To<FeatureFormModel>().ToListAsync();

            return featuresFormModels;
        }

        public async Task<ListingOptions> GetListingOptionsAsync()
        {
            return new ListingOptions
            {
                PropertyTypes = await this.GetPropertyTypes(),
                ListingTypes = await this.GetListingTypes(),
                Features = await this.GetFeatures(),
            };
        }
    }
}
