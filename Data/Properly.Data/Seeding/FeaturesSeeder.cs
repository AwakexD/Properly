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
            var featuresNames = new Dictionary<string, string>()
            {
                { "Swimming pool", "fa-solid fa-swimming-pool" },
                { "Garden", "fa-solid fa-leaf" },
                { "Backyard", "fa-solid fa-tree" },
                { "Garage", "fa-solid fa-car-garage" },
                { "Parking space", "fa-solid fa-parking" },
                { "Balcony", "fa-solid fa-home" },
                { "Air conditioning", "fa-solid fa-snowflake" },
                { "Heating system", "fa-solid fa-fire" },
                { "Kitchen appliances", "fa-solid fa-blender" },
                { "Internet connection", "fa-solid fa-wifi" },
                { "TV connection", "fa-solid fa-tv" },
                { "Gated community", "fa-solid fa-house-lock" },
                { "Security cameras", "fa-solid fa-video" },
                { "Tennis court", "fa-solid fa-tennis-ball" },
                { "Gym", "fa-solid fa-dumbbell" },
                { "Solar panels", "fa-solid fa-solar-panel" },
                { "Laundry room", "fa-solid fa-sink" },
                { "Mountain view", "fa-solid fa-mountain" },
                { "Ocean view", "fa-solid fa-water" },
                { "Guest suite", "fa-solid fa-bed" }
            };

            foreach (var feature in featuresNames)
            {
                features.Add(new Feature() { Name = feature.Key, IconClass = feature.Value});
            }

            await dbContext.Features.AddRangeAsync(features);
        }
    }
}
