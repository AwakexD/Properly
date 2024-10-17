using System.Collections.Generic;
using System.Threading.Tasks;
using Properly.Web.ViewModels.Administration.User;

namespace Properly.Services.Data.Contracts
{
    public interface IUserService
    {
        Task<IEnumerable<UserViewModel>> GetAllUsersAsync();
    }
}
