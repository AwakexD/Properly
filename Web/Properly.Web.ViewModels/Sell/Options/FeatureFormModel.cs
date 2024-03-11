﻿namespace Properly.Web.ViewModels.Sell.Options
{
    using Properly.Data.Models;
    using Properly.Services.Mapping;

    public class FeatureFormModel : IMapFrom<Feature>
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}
