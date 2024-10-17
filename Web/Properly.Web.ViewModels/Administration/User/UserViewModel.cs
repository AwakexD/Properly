using Properly.Data.Models.User;
using Properly.Services.Mapping;

namespace Properly.Web.ViewModels.Administration.User
{
    public class UserViewModel : IMapFrom<ApplicationUser>
    {
        public string Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public bool EmailConfirmed { get; set; }
    }
}
