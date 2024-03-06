namespace Properly.Web.ViewModels.Listing
{
    using Properly.Data.Models;
    using Properly.Services.Mapping;

    public class ListingIndexViewModel
    {
        public string PhotoUrl { get; set; } =
            "https://content.app-sources.com/s/85506704990285511/uploads/Balfour/3875-Balfour-Ct-36-4527457.jpg?format=webp";

        public int Price { get; set; }

        public string Address { get; set; }
    }
}
