namespace Properly.Web.ViewModels.Sell.Models
{
    using System.ComponentModel.DataAnnotations;

    public class ListingInputModel
    {
        public int Price { get; set; }

        public int ListingTypeId { get; set; }
    }
}
