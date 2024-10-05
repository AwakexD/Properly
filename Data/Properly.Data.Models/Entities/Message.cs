using Properly.Data.Common.Models;
using Properly.Data.Models.User;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Properly.Data.Models.Entities
{
    public class Message : BaseDeletableModel<Guid>
    {
        public Message()
        {
            this.Id = Guid.NewGuid();
        }

        [Required]
        [MaxLength(100)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public string LastName { get; set; }

        [Required]
        [MaxLength(20)]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string MessageContent { get; set; }

        [Required]
        [ForeignKey(nameof(Sender))]
        public string SenderId { get; set; }
        public ApplicationUser Sender { get; set; }

        [Required]
        [ForeignKey(nameof(Receiver))]
        public string ReceiverId { get; set; }
        public ApplicationUser Receiver { get; set; }
    }
}
