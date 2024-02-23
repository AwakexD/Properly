namespace Properly.Data.Models
{
    using System;

    using Properly.Data.Common.Models;

    public class Photo : BaseDeletableModel<Guid>
    {
        public Photo()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid ListingId { get; set; }

        public virtual Listing Listing { get; set; }
    }
}
