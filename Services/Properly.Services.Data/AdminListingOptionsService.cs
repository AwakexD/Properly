using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Properly.Data.Common.Repositories;
using Properly.Data.Models.Entities;
using Properly.Services.Data.Contracts;
using Properly.Services.Mapping;
using Properly.Web.ViewModels.AdminListingOptions;

namespace Properly.Services.Data
{
    public class AdminListingOptionsService : IAdminListingOptionsService
    {
        private readonly IDeletableEntityRepository<PropertyType> propertyTypesRepository;
        private readonly IDeletableEntityRepository<Feature> featuresRepository;

        public AdminListingOptionsService(IDeletableEntityRepository<PropertyType> propertyTypesRepository,
            IDeletableEntityRepository<Feature> featuresRepository)
        {
            this.propertyTypesRepository = propertyTypesRepository;
            this.featuresRepository = featuresRepository;
        }

        public async Task<IEnumerable<PropertyTypeAdminModel>> GetAllPropertyTypesAsync()
        {
            if (!await this.propertyTypesRepository.AllAsNoTrackingWithDeleted().AnyAsync())
            {
                throw new InvalidOperationException("PropertyTypes repository is empty.");
            }

            var propertyTypes = await propertyTypesRepository.AllAsNoTrackingWithDeleted()
                .To<PropertyTypeAdminModel>()
                .ToListAsync();

            return propertyTypes;
        }

        public async Task<IEnumerable<FeatureAdminModel>> GetAllFeaturesAsync()
        {
            if (!await this.featuresRepository.AllAsNoTrackingWithDeleted().AnyAsync())
            {
                throw new InvalidOperationException("Features repository is empty.");
            }

            var features = await this.featuresRepository.AllAsNoTrackingWithDeleted()
                .To<FeatureAdminModel>()
                .ToListAsync();

            return features;
        }

        public async Task AddPropertyTypeAsync(PropertyTypeAdminModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("Parameter cannot be null.");
            }

            var propertyTypes = new PropertyType
            {
                Name = model.Name
            };

            await this.propertyTypesRepository.AddAsync(propertyTypes);
            await this.propertyTypesRepository.SaveChangesAsync();
        }

        public Task UpdatePropertyTypeAsync(PropertyTypeAdminModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task DeletePropertyTypeAsync(int id, bool hardDelete)
        {
            throw new System.NotImplementedException();
        }

        public Task AddFeatureAsync(FeatureAdminModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task UpdateFeatureAsync(FeatureAdminModel model)
        {
            throw new System.NotImplementedException();
        }

        public Task DeleteFeatureAsync(int id, bool hardDelete)
        {
            throw new System.NotImplementedException();
        }
    }
}
