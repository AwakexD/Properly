namespace Properly.Services.Data.Contracts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Properly.Web.ViewModels.Messages;

    public interface IMessagesService
    {
        Task CreateMessageAsync(string userId,  MessageRequest messageRequest);

        Task ArchiveMessageAsync(string userId, string messageId);

        Task<IEnumerable<MessageViewModel>> GetActiveMessagesForUser(string userId);

        Task<IEnumerable<MessageViewModel>> GetArchivedMessagesForUser(string userId);

        Task<int> GetActiveMessagesCountForUser(string userId);
    }
}
