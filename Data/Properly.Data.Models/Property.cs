namespace Properly.Data.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    using Properly.Data.Common.Models;

    using static Properly.Common.EntityConstants.PropertyConstants;

    public class Property : BaseDeletableModel<Guid>
    {
        public Property()
        {
            this.Id = Guid.NewGuid();
            this.PropertyFeatures = new HashSet<PropertyFeature>();
        }

        [Required]
        [Range(SizeMinLength, SizeMaxLength)]
        public int Size { get; set; }

        public int? Bathrooms { get; set; }

        public int? Bedrooms { get; set; }

        [Required]
        [StringLength(DescriptionMaxLength)]
        public string Description { get; set; }

        [Required]
        public DateTime ConstructionDate { get; set; }

        [Required]
        [ForeignKey(nameof(Address))]
        public int AddressId { get; set; }

        public virtual Address Address { get; set; }

        [Required]
        [ForeignKey(nameof(PropertyType))]
        public int PropertyTypeId { get; set; }

        public virtual PropertyType PropertyType { get; set; }

        public virtual ICollection<PropertyFeature> PropertyFeatures { get; set; }
    }
}
