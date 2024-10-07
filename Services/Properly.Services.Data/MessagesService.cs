using System;
using System.Threading.Tasks;
using AutoMapper.Execution;
using Microsoft.EntityFrameworkCore;
using Properly.Data.Common.Repositories;
using Properly.Data.Models.Entities;
using Properly.Services.Data.Contracts;
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
            var receiverId = await GetListingOwnerIdAsync(userId);

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

        private async Task<string> GetListingOwnerIdAsync(string userId)
        {
            var listing = await listingsRepository.AllAsNoTracking()
                .FirstOrDefaultAsync(l => l.CreatorId ==  userId);
               

            if (listing == null)
            {
                throw new Exception("Listing not found");
            }

            return listing.CreatorId;
        }
    }
}
