﻿namespace Properly.Web.ViewModels.Sell
{
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Http;
    using Properly.Web.Infrastructure.Validation;
    using Properly.Web.ViewModels.Sell.Models;

    public class CreateListingViewModel
    {
        public CreateListingViewModel()
        {
            this.Listing = new ListingInputModel();
            this.Property = new PropertyInputModel();
            this.Address = new AddressInputModel();
            this.ListingOptions = new ListingOptions();
            this.UploadedPhotos = new HashSet<IFormFile>();
        }

        public ListingInputModel Listing { get; set; }

        public PropertyInputModel Property { get; set; }

        public AddressInputModel Address { get; set; }

        public ListingOptions ListingOptions { get; set; }

        [AllowedExtensions(new[] { ".png", ".jpg", ".jpeg", ".webp", ".jfif" })]
        public IEnumerable<IFormFile> UploadedPhotos { get; set; }

        public IEnumerable<string> UploadedPhotoUrls { get; set; } = new List<string>();

    }
}
