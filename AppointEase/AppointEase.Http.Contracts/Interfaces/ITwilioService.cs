using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Http.Contracts.Interfaces
{
    public interface ITwilioService
    {
        public void SendMessage(string to, string messageBody);
    }
}
