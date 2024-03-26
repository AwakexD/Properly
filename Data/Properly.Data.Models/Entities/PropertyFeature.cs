namespace Properly.Data.Models.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;

    using Properly.Data.Common.Models;

    public class PropertyFeature : BaseDeletableModel<int>
    {
        [ForeignKey(nameof(Property))]
        public Guid PropertyId { get; set; }

        public virtual Property Property { get; set; }

        [ForeignKey(nameof(Feature))]
        public int FeatureId { get; set; }

        public virtual Feature Feature { get; set; }
    }
}
