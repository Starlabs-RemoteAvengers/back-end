using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.EmailConfig;
using AppointEase.Application.Contracts.Models.Request;
using MailKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NETCore.MailKit.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.Common
{
    public class ApplicationExtensions : IApplicationExtensions
    {
        private readonly ILogger _logger;
        private readonly IEmailServices _emailService;
        private readonly IConfiguration _configuration;

        public ApplicationExtensions(ILogger<ApplicationExtensions> logger, IEmailServices _emailService, IConfiguration _configuration )
        {
            _logger = logger;
            this._emailService = _emailService;
            this._configuration = _configuration;
        }

        public void AddInformationMessage(string message)
        {
            _logger.LogInformation(message);
        }

        public void AddErrorMessage(string message)
        {
            _logger.LogError(message);
        }

        public async Task SendEmailConfirmation(string token, string email)
        {
            var confirmationBaseUrl = _configuration["ConfirmationBaseUrl"];
            var confirmationLink = $"{confirmationBaseUrl.TrimEnd('/')}/Authentication/ConfirmEmail?token={Uri.EscapeDataString(token)}&email={Uri.EscapeDataString(email)}";
            var message = new Messages(new string[] { email }, "Confirmation Email", confirmationLink);
            await _emailService.SendEmail(message);

        }
    }
}
