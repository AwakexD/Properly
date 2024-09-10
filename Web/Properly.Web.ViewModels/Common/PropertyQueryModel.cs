namespace Properly.Web.ViewModels.Common
{
    using Properly.Web.ViewModels.Listing.Enums;
    using System.Collections.Generic;

    public class PropertyQueryModel : PagingViewModel
    {
        public string Type { get; set; }

        public string Keyword { get; set; }

        public string Location { get; set; }

        public int? PropertyType { get; set; }

        public int? MinPrice { get; set; }

        public int? MaxPrice { get; set; }

        public int? MinSize { get; set; }

        public int? MaxSize { get; set; }

        public int? Bedrooms { get; set; }

        public int? Bathrooms { get; set; }

        public int? Rooms { get; set; }

        public List<string> Features { get; set; }
    }
}
