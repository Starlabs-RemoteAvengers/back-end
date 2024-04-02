using AppointEase.Http.Contracts.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace AppointEase.Http.Services
{
    public class TwilioService : ITwilioService
    {
        private readonly string _accountSid;
        private readonly string _authToken;
        private readonly string _phoneNumber;
        public TwilioService()
        {
            _accountSid = Environment.GetEnvironmentVariable("TWILIO_ACCOUNT_SID");
            _authToken = Environment.GetEnvironmentVariable("TWILIO_AUTH_TOKEN");
            _phoneNumber = Environment.GetEnvironmentVariable("TWILIO_PHONE_NUMBER");

            TwilioClient.Init(_accountSid, _authToken);
        }
        public async void SendMessage(string to, string messageBody)
        {
            var from = new PhoneNumber(_phoneNumber);
            var toNumber = new PhoneNumber(to);

            var message = MessageResource.Create(
                to: toNumber,
                from: from,
                body: messageBody
            );
        }
    }
}
