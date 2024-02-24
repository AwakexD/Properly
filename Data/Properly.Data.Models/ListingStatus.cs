namespace Properly.Data.Models
{
    using Properly.Data.Common.Models;

    public class ListingStatus : BaseDeletableModel<int>
    {
        public string Name { get; set; }
    }
}
