namespace Properly.Web.ViewModels.Sell.Models
{
    using System.ComponentModel.DataAnnotations;

    public class AddressInputModel
    {
        public string StreetName { get; set; }

        public string City { get; set; }

        public string ZipCode { get; set; }

        public string Country { get; set; }
    }
}