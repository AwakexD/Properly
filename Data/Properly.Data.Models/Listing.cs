using System.ComponentModel.DataAnnotations.Schema;

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

        public int Price { get; set; }

        public Guid PropertyId { get; set; }

        public virtual Property Property { get; set; }

        public int ListingTypeId { get; set; }

        public virtual ListingType ListingType { get; set; }

        public string CreatorId { get; set; }

        [ForeignKey("CreatorId")]
        public virtual ApplicationUser Creator { get; set; }

        public int ListingStatusId { get; set; }

        public virtual ListingStatus ListingStatus { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }
    }
}
