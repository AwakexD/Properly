﻿namespace Properly.Web.ViewModels.Sell.Options
{
    using Properly.Data.Models.Entities;
    using Properly.Services.Mapping;

    public class ListingTypeFormModel : IMapFrom<ListingType>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
