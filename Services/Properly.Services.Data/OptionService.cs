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
    using Properly.Data.Models;
    using Properly.Services.Data.Contracts;
    using Properly.Services.Mapping;
    using Properly.Web.ViewModels.Sell.Options;

    public class OptionService : IOptionsService
    {
        private readonly IDeletableEntityRepository<PropertyType> propertyTypesRepository;
        private readonly IDeletableEntityRepository<ListingType> listingTypesRepository;
        private readonly IDeletableEntityRepository<Feature> featuresRepository;

        public OptionService(
            IDeletableEntityRepository<PropertyType> propertyTypesRepository,
            IDeletableEntityRepository<ListingType> listingTypesRepository,
            IDeletableEntityRepository<Feature> featuresRepository)
        {
            this.propertyTypesRepository = propertyTypesRepository;
            this.listingTypesRepository = listingTypesRepository;
            this.featuresRepository = featuresRepository;
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
    }
}
