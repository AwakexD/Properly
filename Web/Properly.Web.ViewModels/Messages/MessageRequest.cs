using System.ComponentModel.DataAnnotations;

namespace Properly.Web.ViewModels.Messages
{
    public class MessageRequest
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string MessageContent { get; set; }

        [Required]
        public string ListingId { get; set; }
    }
}
