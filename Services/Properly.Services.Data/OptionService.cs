namespace Properly.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;
    using Microsoft.EntityFrameworkCore;
    using Properly.Data.Common.Repositories;
    using Properly.Data.Models;
    using Properly.Services.Data.Contracts;
    using Properly.Web.ViewModels.Sell.Options;

    public class OptionService : IOptionsService
    {
        private readonly IMapper mapper;
        private readonly IDeletableEntityRepository<PropertyType> propertyTypesRepository;
        private readonly IDeletableEntityRepository<ListingType> listingTypesRepository;
        private readonly IDeletableEntityRepository<Feature> featuresRepository;

        public OptionService(
            IMapper mapper,
            IDeletableEntityRepository<PropertyType> propertyTypesRepository,
            IDeletableEntityRepository<ListingType> listingTypesRepository,
            IDeletableEntityRepository<Feature> featuresRepository)
        {
            this.mapper = mapper;
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

            var propertyTypes = await this.propertyTypesRepository.AllAsNoTracking().ToListAsync();
            var propertyTypeFormModels = this.mapper.Map<IEnumerable<PropertyTypeFormModel>>(propertyTypes);

            return propertyTypeFormModels;
        }

        public async Task<IEnumerable<ListingTypeFormModel>> GetListingTypes()
        {
            if (!await this.listingTypesRepository.All().AnyAsync())
            {
                return null;
            }

            var listingTypes = await this.listingTypesRepository.AllAsNoTracking().ToListAsync();
            var listingTypesFormModels = this.mapper.Map<IEnumerable<ListingTypeFormModel>>(listingTypes);

            return listingTypesFormModels;
        }

        public async Task<IEnumerable<FeatureFormModel>> GetFeatures()
        {
            if (!await this.featuresRepository.All().AnyAsync())
            {
                return null;
            }

            var features = await this.featuresRepository.AllAsNoTracking().ToListAsync();
            var featuresFormModels = this.mapper.Map<IEnumerable<FeatureFormModel>>(features);

            return featuresFormModels;
        }
    }
}
