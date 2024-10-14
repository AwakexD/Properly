using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Properly.Data.Common.Repositories;
using Properly.Data.Models.Entities;
using Properly.Services.Data.Contracts;
using Properly.Services.Mapping;
using Properly.Web.ViewModels.Messages;

namespace Properly.Services.Data
{
    public class MessagesService : IMessagesService
    {
        private readonly IDeletableEntityRepository<Message> messagesRepository;
        private readonly IDeletableEntityRepository<Listing> listingsRepository;
        public MessagesService(IDeletableEntityRepository<Message> messagesRepository,
            IDeletableEntityRepository<Listing> listingsRepository)
        {
            this.messagesRepository = messagesRepository;   
            this.listingsRepository = listingsRepository;
        }

        public async Task CreateMessageAsync(string userId, MessageRequest messageRequest)
        {
            var receiverId = await GetListingOwnerIdAsync(messageRequest.ListingId);

            if (string.IsNullOrEmpty(receiverId))
            {
                throw new InvalidOperationException("User Not Found!");
            }

            var message = new Message
            {
                FullName = messageRequest.FullName,
                PhoneNumber = messageRequest.PhoneNumber,
                Email = messageRequest.Email,
                MessageContent = messageRequest.MessageContent,
                ListingId = new Guid(messageRequest.ListingId),
                SenderId = userId,
                ReceiverId = receiverId,
            };

            await messagesRepository.AddAsync(message);
            await messagesRepository.SaveChangesAsync();
        }

        public async Task ArchiveMessageAsync(string userId, string messageId)
        {
            var messageToArchive = await this.messagesRepository.AllAsNoTracking()
                .Where(m => m.ReceiverId == userId)
                .FirstOrDefaultAsync(m => m.Id == new Guid(messageId));

            if (messageToArchive == null)
            {
                throw new InvalidOperationException("Message Not Found!");
            }

            messageToArchive.IsDeleted = true;

            this.messagesRepository.Update(messageToArchive);
            await this.messagesRepository.SaveChangesAsync();
        }

        public async Task<IEnumerable<MessageViewModel>> GetActiveMessagesForUser(string userId)
        {
            var activeMessages = await this.messagesRepository.AllAsNoTracking()
                .Where(m => m.ReceiverId == userId)
                .To<MessageViewModel>()
                .ToListAsync();

            return activeMessages;
        }

        public async Task<IEnumerable<MessageViewModel>> GetArchivedMessagesForUser(string userId)
        {
            var archivedMessages = await this.messagesRepository.AllAsNoTrackingWithDeleted()
                .Where(m => m.ReceiverId == userId && m.IsDeleted) 
                .To<MessageViewModel>()
                .ToListAsync();

            return archivedMessages;
        }

        private async Task<string> GetListingOwnerIdAsync(string listingId)
        {
            var listing = await listingsRepository.AllAsNoTracking()
                .FirstOrDefaultAsync(l => l.Id == new Guid(listingId));
               

            if (listing == null)
            {
                throw new Exception("Listing Not Found!");
            }

            return listing.CreatorId;
        }

        public async Task<int> GetActiveMessagesCountForUser(string userId)
        {
            int count = await this.messagesRepository.AllAsNoTracking()
                .Where(m => m.ReceiverId == userId)

                .CountAsync();
            
            return count;
        }
    }
}
