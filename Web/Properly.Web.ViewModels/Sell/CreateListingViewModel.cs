namespace Properly.Web.ViewModels.Sell
{
    using System.Collections.Generic;
    using System.Linq;

    using AutoMapper;
    using Microsoft.AspNetCore.Http;
    using Properly.Data.Models;
    using Properly.Services.Mapping;
    using Properly.Web.ViewModels.Sell.Models;

    public class CreateListingViewModel
    {
        public CreateListingViewModel()
        {
            this.UploadedPhotos = new HashSet<IFormFile>();
        }

        // ToDO : Implement AutoMapper (custom mapping)
        public ListingInputModel Listing { get; set; }

        public PropertyInputModel Property { get; set; }

        public AddressInputModel Address { get; set; }

        public ListingOptions ListingOptions { get; set; }

        public IEnumerable<IFormFile> UploadedPhotos { get; set; }

    }
}
