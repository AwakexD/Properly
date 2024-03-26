namespace Properly.Data.Models.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Properly.Data.Common.Models;
    using Properly.Data.Models.User;

    public class Listing : BaseDeletableModel<Guid>
    {
        public Listing()
        {
            this.Id = Guid.NewGuid();
            this.Photos = new HashSet<Photo>();
        }

        [Range(0, int.MaxValue)]
        public int Price { get; set; }

        [Required]
        [ForeignKey(nameof(Property))]
        public Guid PropertyId { get; set; }

        public virtual Property Property { get; set; }

        [Required]
        [ForeignKey(nameof(ListingType))]
        public int ListingTypeId { get; set; }

        public virtual ListingType ListingType { get; set; }

        [Required]
        [ForeignKey(nameof(Creator))]
        public string CreatorId { get; set; }

        public virtual ApplicationUser Creator { get; set; }

        [Required]
        [ForeignKey(nameof(ListingStatus))]
        public int ListingStatusId { get; set; }

        public virtual ListingStatus ListingStatus { get; set; }

        public virtual ICollection<Photo> Photos { get; set; }
    }
}
