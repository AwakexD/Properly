namespace Properly.Data.Models.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    using Properly.Data.Common.Models;
    using Properly.Data.Models.User;

    public class FavoriteListing : BaseDeletableModel<int>
    {
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [ForeignKey(nameof(Listing))]
        public Guid ListingId { get; set; }

        public Listing Listing { get; set; }
    }
}
