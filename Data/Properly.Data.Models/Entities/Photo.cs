
namespace Properly.Data.Models.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using Properly.Data.Common.Models;

    public class Photo : BaseDeletableModel<Guid>
    {
        public Photo()
        {
            this.Id = Guid.NewGuid();
        }

        public string Url { get; set; }

        [ForeignKey(nameof(Listing))]
        public Guid ListingId { get; set; }
    }
}
