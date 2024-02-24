namespace Properly.Data.Models
{
    using System.Collections.Generic;

    using Properly.Data.Common.Models;

    public class Feature : BaseDeletableModel<int>
    {
        public Feature()
        {
            this.PropertyFeatures = new HashSet<PropertyFeature>();
        }

        public string Name { get; set; }

        public virtual ICollection<PropertyFeature> PropertyFeatures { get; set; }

    }
}
