using AppointEase.Application.Contracts.Models.EmailConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.Interfaces
{
    public interface IEmailSerivces
    {
        Task SendEmail(Messages messages);
    }
}
