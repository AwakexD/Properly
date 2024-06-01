namespace Properly.Data.Seeding.Listing
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Properly.Data.Models.Entities;

    public class ListingTypesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.ListingTypes.AnyAsync())
            {
                return;
            }

            await ListingTypesSeedAsync(dbContext);
        }

        private static async Task ListingTypesSeedAsync(ApplicationDbContext dbContext)
        {
            var listingTypes = new HashSet<ListingType>();
            var types = new string[] { "For Sale", "For Rent" };
            // For now only these two types are supported, more will be added in the future

            foreach (var type in types)
            {
                listingTypes.Add(new ListingType() { Name = type });
            }

            await dbContext.ListingTypes.AddRangeAsync(listingTypes);
        }
    }
}
