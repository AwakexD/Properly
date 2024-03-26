namespace Properly.Data.Models.Entities
{
    using System.ComponentModel.DataAnnotations;

    using Properly.Data.Common.Models;

    public class ListingStatus : BaseDeletableModel<int>
    {
        [Required]
        public string Name { get; set; }
    }
}
