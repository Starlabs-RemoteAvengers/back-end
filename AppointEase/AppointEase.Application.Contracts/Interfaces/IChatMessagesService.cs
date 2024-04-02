using AppointEase.Application.Contracts.Models.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.Interfaces
{
    public interface IChatMessagesService
    {
        Task<IEnumerable<object>> GetMessagesAsync(string senderId, string receiverId);
        Task SendMessageAsync(MessageRequest message);
    }
}
