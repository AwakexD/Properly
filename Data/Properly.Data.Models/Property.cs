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
            this.PropertyFeatures = new HashSet<PropertyFeature>();
        }

        public int Size { get; set; }

        public int Bathrooms { get; set; }

        public int Bedrooms { get; set; }

        public string Description { get; set; }

        public DateTime ConstructionDate { get; set; }

        public int AddressId { get; set; }

        public virtual Address Address { get; set; }

        public int PropertyTypeId { get; set; }

        public virtual PropertyType PropertyType { get; set; }

        public virtual ICollection<PropertyFeature> PropertyFeatures { get; set; }
    }
}
