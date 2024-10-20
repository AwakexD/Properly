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
            var propertyTypes = await propertyTypesRepository.AllAsNoTrackingWithDeleted()
                .To<PropertyTypeAdminModel>()
                .ToListAsync();

            if (propertyTypes == null)
            {
                throw new InvalidOperationException("PropertyTypes repository is empty.");
            }

            return propertyTypes;
        }

        public async Task<PropertyTypeAdminModel> GetPropertyTypeByIdAsync(int id)
        {
            var propertyType = await propertyTypesRepository.AllAsNoTrackingWithDeleted()
                .To<PropertyTypeAdminModel>()
                .FirstOrDefaultAsync(pt => pt.Id == id);

            if (propertyType == null)
            {
                throw new InvalidOperationException("PropertyTypes repository is empty.");
            }

            return propertyType;
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

        public async Task UpdatePropertyTypeAsync(int id, PropertyTypeAdminModel model)
        {
            var propertyType = await GetPropertyTypeByIdInternalAsync(id);

            propertyType.Name = model.Name;

            this.propertyTypesRepository.Update(propertyType);
            await this.propertyTypesRepository.SaveChangesAsync();
        }

        public async Task DeletePropertyTypeAsync(int id, bool hardDelete)
        {
            var propertyType = await GetPropertyTypeByIdInternalAsync(id);

            if (hardDelete)
            { 
                this.propertyTypesRepository.HardDelete(propertyType);
            }
            else
            {
                this.propertyTypesRepository.Delete(propertyType);
            }

            await this.propertyTypesRepository.SaveChangesAsync();
        }

        public async Task ActivatePropertyTypeAsync(int id)
        {
            var propertyType = await GetPropertyTypeByIdInternalAsync(id);

            propertyType.IsDeleted = false;
            await this.propertyTypesRepository.SaveChangesAsync();
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

        public async Task<FeatureAdminModel> GetFeatureByIdAsync(int id)
        {
            var feature = await featuresRepository.AllAsNoTrackingWithDeleted()
                .To<FeatureAdminModel>()
                .FirstOrDefaultAsync(f => f.Id == id);

            if (feature == null)
            {
                throw new InvalidOperationException("Feature not found.");
            }

            return feature;
        }

        public async Task AddFeatureAsync(FeatureAdminModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException("Parameter cannot be null.");
            }

            var feature = new Feature()
            {
                Name = model.Name,
                IconClass = model.IconClass
            };

            await this.featuresRepository.AddAsync(feature);
            await this.featuresRepository.SaveChangesAsync();
        }

        public async Task UpdateFeatureAsync(int id, FeatureAdminModel model)
        {
            var feature = await GetFeatureByIdInternalAsync(id);

            feature.Name = model.Name;
            feature.IconClass = model.IconClass;

            this.featuresRepository.Update(feature);
            await this.featuresRepository.SaveChangesAsync();
        }

        public async Task DeleteFeatureAsync(int id, bool hardDelete)
        {
            var feature = await GetFeatureByIdInternalAsync(id);

            if (hardDelete)
            {
                this.featuresRepository.HardDelete(feature);
            }
            else
            {
                this.featuresRepository.Delete(feature);
            }

            await this.featuresRepository.SaveChangesAsync();
        }

        public async Task ActivateFeatureAsync(int id)
        {
            var feature = await GetFeatureByIdInternalAsync(id);

            feature.IsDeleted = false;
            await this.featuresRepository.SaveChangesAsync();
        }

        private async Task<Feature> GetFeatureByIdInternalAsync(int id)
        {
            var feature = await featuresRepository.AllWithDeleted()
                .FirstOrDefaultAsync(f => f.Id == id);

            if (feature == null)
            {
                throw new ArgumentNullException($"Feature with ID {id} not found.");
            }
            return feature;
        }

        private async Task<PropertyType> GetPropertyTypeByIdInternalAsync(int id)
        {
            var propertyType = await propertyTypesRepository.AllWithDeleted().FirstOrDefaultAsync(pt => pt.Id == id);
            if (propertyType == null)
            {
                throw new ArgumentNullException($"Property Type with ID {id} not found.");
            }
            return propertyType;
        }
    }
}
