using AppointEase.Application.Contracts.Models.EmailConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.Interfaces
{
    public interface IEmailServices
    {
        Task SendEmail(Messages messages);
    }
}
