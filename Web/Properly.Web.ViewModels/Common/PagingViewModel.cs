﻿
namespace Properly.Web.ViewModels.Common
{
    using System;
    using System.Collections.Generic;

    public class PagingViewModel
    {
        public PagingViewModel()
        {
            this.QueryParameters = new Dictionary<string, string>();
        }

        public int PageNumber { get; set; }

        public bool HasPreviousPage => this.PageNumber > 1;

        public int PreviousPageNumber => this.PageNumber - 1;

        public bool HasNextPage => this.PageNumber < this.PagesCount;

        public int NextPageNumber => this.PageNumber + 1;

        public int PagesCount => (int)Math.Ceiling((double)this.ListingCount / this.ItemsPerPage);

        public int ListingCount { get; set; }

        public int ItemsPerPage { get; set; } = 6;

        public Dictionary<string, string> QueryParameters { get; set; }
    }
}
