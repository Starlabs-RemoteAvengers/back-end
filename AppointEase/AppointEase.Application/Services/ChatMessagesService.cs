using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.Request;
using AppointEase.Data.Contracts.Identity;
using AppointEase.Data.Contracts.Interfaces;
using AppointEase.Data.Contracts.Models;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Services
{
    public class ChatMessagesService : IChatMessagesService
    {
        private readonly IChatMessagesRepository _chatMessagesRepository;
        private readonly IMapper _mapper;
        private readonly IHubContext<ChatHub> _hubContext;
        private readonly UserManager<ApplicationUser> _manager;

        public ChatMessagesService()
        {
        }

        public ChatMessagesService(IChatMessagesRepository chatMessagesRepository,IMapper mapper, IHubContext<ChatHub> hubContext, UserManager<ApplicationUser> user)
        {
            _chatMessagesRepository = chatMessagesRepository;
            _mapper = mapper;
            _hubContext = hubContext;
            this._manager = user;
        }
        public async Task<IEnumerable<object>> GetMessagesAsync(string senderId, string receiverId)
        {
            var messages =  await _chatMessagesRepository.GetMessages(senderId, receiverId);
            messages = messages.OrderBy(m => m.Timestamp);

            var mappedMessages = messages.Select(d => _mapper.Map<MessageRequest>(d));

            var senderObject = await _manager.FindByIdAsync(senderId);
            var senderName = (senderObject == null) ? "User" : senderObject.UserName;

            var receiverObject = await _manager.FindByIdAsync(receiverId);
            var receiverName = (receiverObject == null) ? "User" : receiverObject.UserName;

            var listObject = mappedMessages.Select(m => new
            {
                sender = m.SenderId,
                receiver = m.ReceiverId,
                senderName = senderName,
                receiverName = receiverName,
                content = m.Message,
                timestamp = m.Timestamp

            });

            return listObject;
        }

        public async Task SendMessageAsync(MessageRequest messagerequest)
        {
            var message = _mapper.Map<ChatMessages>(messagerequest);
            await _chatMessagesRepository.SaveMessages(message);
        }
    }
}
