namespace Properly.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PropertyFeature
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Property))]
        public Guid PropertyId { get; set; }

        public virtual Property Property { get; set; }

        [ForeignKey(nameof(Feature))]
        public int FeatureId { get; set; }

        public virtual Feature Feature { get; set; }
    }
}
