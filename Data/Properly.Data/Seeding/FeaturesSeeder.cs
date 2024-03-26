namespace Properly.Data.Seeding
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Properly.Data.Models.Entities;

    public class FeaturesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.Features.AnyAsync())
            {
                return;
            }

            await FeaturesSeedAsync(dbContext);
        }

        public static async Task FeaturesSeedAsync(ApplicationDbContext dbContext)
        {
            var features = new HashSet<Feature>();
            var featuresNames = new string[]
            {
                "Swimming pool", "Garden", "Backyard", "Garage", "Parking space", "Balcony",
                "Air conditioning", "Heating system", "Kitchen appliances", "Internet connection", "TV connection",
                "Gated community", "Security cameras", "Tennis court", "Gym", "Solar panels", "Laundry room", "Mountain view", "Ocean view", "Guest suite",
            };

            foreach (var feature in featuresNames)
            {
                features.Add(new Feature() { Name = feature });
            }

            await dbContext.Features.AddRangeAsync(features);
        }
    }
}
