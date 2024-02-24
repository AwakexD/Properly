using System.ComponentModel.DataAnnotations;

namespace Properly.Data.Models
{
    using Properly.Data.Common.Models;

    public class ListingType : BaseDeletableModel<int>
    {
        [Required]
        public string Name { get; set; }

    }
}
