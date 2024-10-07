using System;
using System.Linq;
using AutoMapper;
using Properly.Data.Models.Entities;
using Properly.Services.Mapping;

namespace Properly.Web.ViewModels.Messages
{
    public class MessageViewModel : IHaveCustomMappings
    {
        public string Id { get; set; }

        public string ListingPhoto { get; set; }
        
        public string ListingAddress { get; set; }

        public string SenderFullName { get; set; }

        public string SenderPhoneNumber { get; set; }

        public string SenderEmail { get; set; }

        public string MessageContent { get; set; }

        public DateTime CreatedOn { get; set; }

        public void CreateMappings(IProfileExpression configuration)
        {
            configuration.CreateMap<Message, MessageViewModel>()
                .ForMember(mv => mv.Id, opt => opt.MapFrom(m => m.Id))
                .ForMember(mv => mv.ListingPhoto, opt => opt.MapFrom(m => m.Listing.Photos.Select(x => x.Url).FirstOrDefault()))
                .ForMember(mv => mv.ListingAddress, opt => opt.MapFrom(m => m.Listing.Property.Address.StreetName + ", " + m.Listing.Property.Address.ZipCode))
                .ForMember(mv => mv.SenderFullName,
                    opt => opt.MapFrom(m => m.Sender.FirstName + " " + m.Sender.LastName))
                .ForMember(mv => mv.SenderPhoneNumber, opt => opt.MapFrom(m => m.Sender.PhoneNumber))
                .ForMember(mv => mv.SenderEmail, opt => opt.MapFrom(m => m.Sender.Email))
                .ForMember(mv => mv.CreatedOn, opt => opt.MapFrom(m => m.CreatedOn));
        }
    }
}
