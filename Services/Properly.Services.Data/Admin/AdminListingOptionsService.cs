namespace Properly.Services.Data.Admin
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Properly.Data.Common.Repositories;
    using Properly.Data.Models.Entities;
    using Properly.Services.Data.Contracts;
    using Properly.Services.Mapping;
    using Properly.Web.ViewModels.AdminListingOptions;

    public class AdminListingOptionsService : IAdminListingOptionsService
    {
        private readonly IDeletableEntityRepository<PropertyType> propertyTypesRepository;
        private readonly IDeletableEntityRepository<Feature> featuresRepository;
        private ILogger<AdminListingOptionsService> logger;

        public AdminListingOptionsService(IDeletableEntityRepository<PropertyType> propertyTypesRepository,
            IDeletableEntityRepository<Feature> featuresRepository,
            ILogger<AdminListingOptionsService> logger)
        {
            this.propertyTypesRepository = propertyTypesRepository;
            this.featuresRepository = featuresRepository;
            this.logger = logger;
        }

        public async Task<IEnumerable<PropertyTypeAdminModel>> GetAllPropertyTypesAsync()
        {
            var propertyTypes = await propertyTypesRepository.AllAsNoTrackingWithDeleted()
                .To<PropertyTypeAdminModel>()
                .ToListAsync();

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
                throw new ArgumentNullException(nameof(model),"Parameter cannot be null.");
            }

            try
            {
                var propertyTypes = new PropertyType
                {
                    Name = model.Name
                };

                await this.propertyTypesRepository.AddAsync(propertyTypes);
                await this.propertyTypesRepository.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                logger.LogError(e, $"An error occurred while adding a new property type: {model.Name}");
                throw new InvalidOperationException("Could not add the property type.");
            }
            catch(Exception e)
            {
                this.logger.LogError(e, $"Unexpected error in {nameof(AddPropertyTypeAsync)} for {model.Name}");
                throw;
            }
        }

        public async Task UpdatePropertyTypeAsync(int id, PropertyTypeAdminModel model)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model), "Parameter cannot be null.");
            }

            try
            {
                var propertyType = await GetPropertyTypeByIdInternalAsync(id);

                propertyType.Name = model.Name;

                this.propertyTypesRepository.Update(propertyType);
                await this.propertyTypesRepository.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                logger.LogError(e, $"An error occurred while updating a property type : {model.Name}");
                throw new InvalidOperationException("Could not update the property type.");
            }  
            catch (Exception e)
            {
                logger.LogError(e, $"Unexpected error in {nameof(UpdatePropertyTypeAsync)}, Property Type : {id}");
                throw;
            }
        }

        public async Task DeletePropertyTypeAsync(int id, bool hardDelete)
        {
            try
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
            catch (Exception e)
            {
                logger.LogError(e, $"Unexpected error in {nameof(DeletePropertyTypeAsync)}, Property Type ID : {id}");
                throw;
            }
        }

        public async Task ActivatePropertyTypeAsync(int id)
        {
            try
            {
                var propertyType = await GetPropertyTypeByIdInternalAsync(id);

                this.propertyTypesRepository.Undelete(propertyType);
                await this.propertyTypesRepository.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                logger.LogError(e, $"An error occurred while activating a property type ID : {id}");
                throw new InvalidOperationException("Could not activate the property type.");
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Unexpected error in {nameof(ActivatePropertyTypeAsync)}, Property Type ID : {id}");
                throw;
            }
        }

        public async Task<IEnumerable<FeatureAdminModel>> GetAllFeaturesAsync()
        {
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
                throw new ArgumentNullException(nameof(model),"Parameter cannot be null.");
            }

            try
            {
                var feature = new Feature()
                {
                    Name = model.Name,
                    IconClass = model.IconClass
                };

                await this.featuresRepository.AddAsync(feature);
                await this.featuresRepository.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                logger.LogError(e, $"An error occurred while adding a feature : {model.Name}");
                throw new InvalidOperationException("Could not add the feature.");
            }
            catch (Exception e)
            {
                this.logger.LogError(e, $"Unexpected error in {nameof(AddFeatureAsync)} for {model.Name}");
                throw;
            }
        }

        public async Task UpdateFeatureAsync(int id, FeatureAdminModel model)
        {
            try
            {
                var feature = await GetFeatureByIdInternalAsync(id);

                feature.Name = model.Name;
                feature.IconClass = model.IconClass;

                this.featuresRepository.Update(feature);
                await this.featuresRepository.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                logger.LogError(e, $"An error occurred while updating a feature ID : {id}");
                throw new InvalidOperationException("Could not update the feature.");
            }
            catch (Exception e)
            {
                this.logger.LogError(e, $"Unexpected error in {nameof(UpdateFeatureAsync)}, Feature ID : {id}");
                throw;
            }
        }

        public async Task DeleteFeatureAsync(int id, bool hardDelete)
        {
            try
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
            catch (Exception e)
            {
                logger.LogError(e, $"Unexpected error in {nameof(DeleteFeatureAsync)}, Feature ID : {id}");
                throw;
            }
        }

        public async Task ActivateFeatureAsync(int id)
        {
            try
            {
                var feature = await GetFeatureByIdInternalAsync(id);

                feature.IsDeleted = false;
                await this.featuresRepository.SaveChangesAsync();
            }
            catch (DbUpdateException e)
            {
                logger.LogError(e, $"An error occurred while activating a feature ID : {id}");
                throw new InvalidOperationException("Could not activate the feature.");
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Unexpected error in {nameof(ActivateFeatureAsync)}, Feature ID : {id}");
                throw;
            }
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
