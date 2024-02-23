namespace Properly.Data.Models
{
    using System;
    using System.Collections.Generic;

    using Properly.Data.Common.Models;

    public class Property : BaseDeletableModel<Guid>
    {
        public Property()
        {
            this.Id = Guid.NewGuid();
            this.Amenities = new HashSet<PropertyAmenity>();
        }

        public string OwnerId { get; set; }

        public virtual ApplicationUser Owner { get; set; }

        public int AddressId { get; set; }

        public virtual Address Address { get; set; }

        public int Bedrooms { get; set; }

        public int Bathrooms { get; set; }

        public double Size { get; set; }

        public DateTime ConstructionDate { get; set; }

        public string Description { get; set; }

        public virtual ICollection<PropertyAmenity> Amenities { get; set; }
    }
}
