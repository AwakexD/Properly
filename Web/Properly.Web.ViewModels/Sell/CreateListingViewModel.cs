﻿namespace Properly.Web.ViewModels.Sell
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Http;
    using Properly.Web.ViewModels.Sell.Models;

    public class CreateListingViewModel
    {
        // ToDO : Create Cloudinary service
        // ToDO : Add Photo Upload option
        public CreateListingViewModel()
        {
            this.UploadedPhotos = new HashSet<IFormFile>();
        }

        public ListingInputModel Listing { get; set; }

        public PropertyInputModel Property { get; set; }

        public AddressInputModel Address { get; set; }

        public ListingOptions ListingOptions { get; set; }

        public IEnumerable<IFormFile> UploadedPhotos { get; set; }
    }
}
