namespace Properly.Data.Models
{

    public class PropertyAmenity
    {
        public int Id { get; set; }

        public string PropertyId { get; set; }

        public virtual Property Property { get; set; }

        public string AmenityId { get; set; }

        public virtual Amenity Amenity { get; set; }
    }
}
