using AppointEase.Application.Contracts.Models.Operations;
using AppointEase.Application.Contracts.Models.Request;
using AppointEase.Data.Contracts.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Data.Contracts.Interfaces
{
    public interface IChatMessagesRepository
    {
        Task<IEnumerable<ChatMessages>> GetMessages(string senderId, string receiverId);
        Task<OperationResult> SaveMessages(ChatMessages message);
    }
}
