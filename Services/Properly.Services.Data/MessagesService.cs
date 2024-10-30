namespace Properly.Services.Data
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Properly.Data.Common.Repositories;
    using Properly.Data.Models.Entities;
    using Properly.Services.Data.Contracts;
    using Properly.Services.Mapping;
    using Properly.Web.ViewModels.Messages;

    public class MessagesService : IMessagesService
    {
        private readonly IDeletableEntityRepository<Message> messagesRepository;
        private readonly IDeletableEntityRepository<Listing> listingsRepository;
        private readonly ILogger<MessagesService> logger;

        public MessagesService(IDeletableEntityRepository<Message> messagesRepository,
            IDeletableEntityRepository<Listing> listingsRepository,
            ILogger<MessagesService> logger)
        {
            this.messagesRepository = messagesRepository;   
            this.listingsRepository = listingsRepository;
            this.logger = logger;
        }

        public async Task CreateMessageAsync(string userId, MessageRequest messageRequest)
        {
            try
            {
                var receiverId = await GetListingOwnerIdAsync(messageRequest.ListingId);

                if (string.IsNullOrEmpty(receiverId))
                {
                    logger.LogWarning($"Receiver ID not found for Listing ID : {messageRequest.ListingId}");
                    throw new InvalidOperationException("User not found!");
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
            catch (Exception e)
            {
                logger.LogError(e, $"Error creating message from user {userId}");
                throw;
            }
        }

        public async Task ArchiveMessageAsync(string userId, string messageId)
        {
            try
            {
                var messageToArchive = await this.messagesRepository.AllAsNoTracking()
                    .Where(m => m.ReceiverId == userId)
                    .FirstOrDefaultAsync(m => m.Id == new Guid(messageId));

                if (messageToArchive == null)
                {
                    logger.LogWarning($"Message not found for archiving, Message ID : {messageId}");
                    throw new InvalidOperationException("Message not found!");
                }

                messageToArchive.IsDeleted = true;

                this.messagesRepository.Update(messageToArchive);
                await this.messagesRepository.SaveChangesAsync();
            }
            catch (Exception e)
            {
                logger.LogError(e, $"Error archiving message with ID : {messageId} for user : {userId}");
                throw;
            }
        }

        public async Task<IEnumerable<MessageViewModel>> GetActiveMessagesForUser(string userId)
        {
            try
            {
                var activeMessages = await this.messagesRepository.AllAsNoTracking()
                    .Where(m => m.ReceiverId == userId)
                    .To<MessageViewModel>()
                    .ToListAsync();

                if (!activeMessages.Any())
                {
                    logger.LogWarning($"No active messages found for user {userId}.");
                }

                return activeMessages;
            }
            catch (Exception e)
            {
                logger.LogError(e, $"An error occurred while retrieving active messages for user {userId}");
                return Enumerable.Empty<MessageViewModel>();
            }
        }

        public async Task<IEnumerable<MessageViewModel>> GetArchivedMessagesForUser(string userId)
        {
            try
            {
                var archivedMessages = await this.messagesRepository.AllAsNoTrackingWithDeleted()
                    .Where(m => m.ReceiverId == userId && m.IsDeleted)
                    .To<MessageViewModel>()
                    .ToListAsync();

                if (!archivedMessages.Any())
                {
                    logger.LogWarning($"No archived messages found for user {userId}.");
                }

                return archivedMessages;
            }
            catch (Exception e)
            {
                logger.LogError(e, $"An error occurred while retrieving archived messages for user {userId}");
                return Enumerable.Empty<MessageViewModel>();
            }
        }

        public async Task<int> GetActiveMessagesCountForUser(string userId)
        {
            try
            {
                int count = await this.messagesRepository.AllAsNoTracking()
                    .Where(m => m.ReceiverId == userId)
                    .CountAsync();

                return count;
            }
            catch (Exception e)
            {
                logger.LogError(e, $"An error occurred while counting active messages for user {userId}.");
                return 0;
            }
        }

        private async Task<string> GetListingOwnerIdAsync(string listingId)
        {
            try
            {
                var listing = await listingsRepository.AllAsNoTracking()
                    .FirstOrDefaultAsync(l => l.Id == new Guid(listingId));


                if (listing == null)
                {
                    logger.LogWarning($"Listing not found, Listing ID : {listingId}");
                    throw new Exception("Listing not found!");
                }

                return listing.CreatorId;
            }
            catch (Exception e)
            {
                logger.LogError(e, $"An error occurred while retrieving listing with ID : {listingId}");
                throw;
            }
        }
    }
}
