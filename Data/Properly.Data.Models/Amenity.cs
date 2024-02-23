namespace Properly.Data.Models
{
    using System.Collections.Generic;

    using Properly.Data.Common.Models;

    public class Amenity : BaseDeletableModel<int>
    {
        public Amenity()
        {
            this.Properties = new HashSet<PropertyAmenity>();
        }

        public string Name { get; set; }

        public virtual ICollection<PropertyAmenity> Properties { get; set; }
    }
}
