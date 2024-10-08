using System.Collections.Generic;
using Properly.Web.ViewModels.Messages;
using System.Threading.Tasks;

namespace Properly.Services.Data.Contracts
{
    public interface IMessagesService
    {
        Task CreateMessageAsync(string userId,  MessageRequest messageRequest);

        Task ArchiveMessageAsync(string userId, string messageId);

        Task<IEnumerable<MessageViewModel>> GetActiveMessagesForUser(string userId);

        Task<IEnumerable<MessageViewModel>> GetArchivedMessagesForUser(string userId);
    }
}
