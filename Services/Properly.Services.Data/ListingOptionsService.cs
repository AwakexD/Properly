namespace Properly.Services.Data
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
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
        private readonly ILogger<ListingOptionsService> logger;

        public ListingOptionsService(
            IDeletableEntityRepository<PropertyType> propertyTypesRepository,
            IDeletableEntityRepository<ListingType> listingTypesRepository,
            IDeletableEntityRepository<Feature> featuresRepository,
            ILogger<ListingOptionsService> logger)
        {
            this.propertyTypesRepository = propertyTypesRepository;
            this.listingTypesRepository = listingTypesRepository;
            this.featuresRepository = featuresRepository;
            this.logger = logger;
        }

        public async Task<IEnumerable<PropertyTypeFormModel>> GetPropertyTypes()
        {
            try
            {
                if (!await this.propertyTypesRepository.All().AnyAsync())
                {
                    logger.LogWarning("Property types not found.");
                    return Enumerable.Empty<PropertyTypeFormModel>();
                }

                var propertyTypeFormModels = await this.propertyTypesRepository.AllAsNoTracking()
                    .To<PropertyTypeFormModel>().ToListAsync();

                return propertyTypeFormModels;
            }   
            catch (Exception e)
            {
                logger.LogError(e, "Error retrieving property types.");
                throw;
            }
        }

        public async Task<IEnumerable<ListingTypeFormModel>> GetListingTypes()
        {
            try
            {

                if (!await this.listingTypesRepository.All().AnyAsync())
                {
                    logger.LogWarning("Listing types not found.");
                    return Enumerable.Empty<ListingTypeFormModel>();
                }

                var listingTypesFormModels =
                    await this.listingTypesRepository.AllAsNoTracking().To<ListingTypeFormModel>().ToListAsync();

                return listingTypesFormModels;
            }
            catch (Exception e)
            {
               logger.LogError(e, "Error retrieving listing types.");
                throw;
            }
        }

        public async Task<IEnumerable<FeatureFormModel>> GetFeatures()
        {
            try
            {
                if (!await this.featuresRepository.All().AnyAsync())
                {
                    logger.LogWarning("Features not found.");
                    return Enumerable.Empty<FeatureFormModel>();
                }

                var featuresFormModels =
                    await this.featuresRepository.AllAsNoTracking().To<FeatureFormModel>().ToListAsync();

                return featuresFormModels;
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error retrieving features.");
                throw;
            }
        }

        public async Task<ListingOptions> GetListingOptionsAsync()
        {
            try
            {
                return new ListingOptions
                {
                    PropertyTypes = await this.GetPropertyTypes(),
                    ListingTypes = await this.GetListingTypes(),
                    Features = await this.GetFeatures(),
                };
            }
            catch (Exception e)
            {
                logger.LogError(e, "Error retrieving listing options.");
                throw;
            }
        }
    }
}
