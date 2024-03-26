// ReSharper disable VirtualMemberCallInConstructor
namespace Properly.Data.Models.User
{
    using System;
    using System.Collections.Generic;

    using Microsoft.AspNetCore.Identity;
    using Properly.Data.Common.Models;
    using Properly.Data.Models.Entities;

    public class ApplicationUser : IdentityUser, IAuditInfo, IDeletableEntity
    {
        public ApplicationUser()
        {
            Id = Guid.NewGuid().ToString();
            Roles = new HashSet<IdentityUserRole<string>>();
            Claims = new HashSet<IdentityUserClaim<string>>();
            Logins = new HashSet<IdentityUserLogin<string>>();
            Listings = new HashSet<Listing>();
            FavoriteListings = new HashSet<FavoriteListing>();
        }

        // Audit info
        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        // Deletable entity
        public bool IsDeleted { get; set; }

        public DateTime? DeletedOn { get; set; }

        public virtual ICollection<IdentityUserRole<string>> Roles { get; set; }

        public virtual ICollection<IdentityUserClaim<string>> Claims { get; set; }

        public virtual ICollection<IdentityUserLogin<string>> Logins { get; set; }

        public virtual ICollection<Listing> Listings { get; set; }

        public virtual ICollection<FavoriteListing> FavoriteListings { get; set; }
    }
}
