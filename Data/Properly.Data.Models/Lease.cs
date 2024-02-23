namespace Properly.Data.Models
{
    using System;

    using Properly.Data.Common.Models;

    public class Lease : BaseDeletableModel<Guid>
    {
        public Lease()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid ListingId { get; set; }

        public virtual Listing Listing { get; set; }

        public Guid TentantId { get; set; }

        public virtual ApplicationUser Tentant { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int RentPerNight { get; set; }
    }
}
