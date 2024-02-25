namespace Properly.Data.Seeding.Listing
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;
    using Properly.Data.Models;

    public class ListingStatusesSeeder : ISeeder
    {
        public async Task SeedAsync(ApplicationDbContext dbContext, IServiceProvider serviceProvider)
        {
            if (await dbContext.ListingStatuses.AnyAsync())
            {
                return;
            }

            await ListingStatusesSeedAsync(dbContext);
        }

        public static async Task ListingStatusesSeedAsync(ApplicationDbContext dbContext)
        {
            var listingStatuses = new HashSet<ListingStatus>();
            var statuses = new string[]
                { "Active", "Pending", "Sold", "Rented", "Withdrawn", "Coming Soon", "Off Market" };

            foreach (var stat in statuses)
            {
                listingStatuses.Add(new ListingStatus() { Name = stat });
            }
            
            await dbContext.ListingStatuses.AddRangeAsync(listingStatuses);
        }
    }
}
