namespace Properly.Data.Models.Entities
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Properly.Data.Common.Models;

    public class Feature : BaseDeletableModel<int>
    {
        public Feature()
        {
            this.PropertyFeatures = new HashSet<PropertyFeature>();
        }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<PropertyFeature> PropertyFeatures { get; set; }

    }
}
