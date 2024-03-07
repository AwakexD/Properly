namespace Properly.Web.ViewModels.Sell
{
    using System;

    public class SellFormModel
    {
        // TODO : Model validation
        public int Size { get; set; }

        public int Bathrooms { get; set; }

        public int Bedrooms { get; set; }

        public string Description { get; set; }

        public DateTime ConstructionDate { get; set; }

        public AddressFormModel Address { get; set; }

        // Dropdown menu with seeded data from the db
        public int PropertyType { get; set; }

        public int Price { get; set; }

        // Dropdown menu with seeded data from the db
        public int ListingType { get; set; }
    }
}
