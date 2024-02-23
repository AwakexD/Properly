namespace Properly.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Properly.Data.Common.Models;

    public class Listing : BaseDeletableModel<Guid>
    {
        public Listing()
        {
            this.Id = Guid.NewGuid();
            this.Photos = new HashSet<Photo>();
        }

        public Guid PropertyId { get; set; }

        public virtual Property Property { get; set; }

        public Guid CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }

        public int ListingTypeId { get; set; }

        public virtual ListingType ListingType { get; set; }

        public int Price { get; set; }
    }
}
