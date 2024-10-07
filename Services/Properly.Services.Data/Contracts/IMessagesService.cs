using Properly.Web.ViewModels.Messages;
using System.Threading.Tasks;

namespace Properly.Services.Data.Contracts
{
    public interface IMessagesService
    {
        Task CreateMessageAsync(string userId,  MessageRequest messageRequest);
    }
}
