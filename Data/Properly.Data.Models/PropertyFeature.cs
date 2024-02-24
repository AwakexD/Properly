namespace Properly.Data.Models
{
    using System;

    public class PropertyFeature
    {
        public int Id { get; set; }

        public Guid PropertyId { get; set; }

        public virtual Property Property { get; set; }

        public int FeatureId { get; set; }

        public virtual Feature Feature { get; set; }
    }
}
