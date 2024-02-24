namespace Properly.Data.Models
{
    using System.ComponentModel.DataAnnotations;

    using Properly.Data.Common.Models;

    public class ListingStatus : BaseDeletableModel<int>
    {
        [Required]
        public string Name { get; set; }
    }
}
