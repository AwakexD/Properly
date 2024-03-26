namespace Properly.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Properly.Data.Models.Entities;

    public class PropertyTypesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.PropertyTypes.AnyAsync())
            {
                return;
            }

            await PropertyTypesSeedAsync(dbContext);
        }

        private static async Task PropertyTypesSeedAsync(ApplicationDbContext dbContext)
        {
            var propertyTypes = new HashSet<PropertyType>();

            var typeNames = new string[] { "Apartment", "House", "Villa", "Studio", "Cottage", "Penthouse", "Mansion", "Bungalow", "Farmhouse" };

            foreach (var typeName in typeNames)
            {
                propertyTypes.Add(new PropertyType() { Name = typeName });
            }

            await dbContext.PropertyTypes.AddRangeAsync(propertyTypes);
        }
    }
}
