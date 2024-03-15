namespace Properly.Web.ViewModels.Sell.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class PropertyInputModel
    {
        public int Size { get; set; }

        public int? Bathrooms { get; set; }

        public int? Bedrooms { get; set; }

        public string Description { get; set; }

        public DateTime ConstructionDate { get; set; }

        public int PropertyTypeId { get; set; }

        public IEnumerable<int> SelectedFeatures { get; set; }
    }
}