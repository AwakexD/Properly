namespace Properly.Data.Models
{
    using Properly.Data.Common.Models;

    public class Address : BaseDeletableModel<int>
    {
        public string PropertyId { get; set; }

        public virtual Property Property { get; set; }

        public string StreetAddress { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }

        public string Country { get; set; }

        public double? Latitude { get; set; } = null;

        public double? Longitude { get; set; } = null;
    }
}
