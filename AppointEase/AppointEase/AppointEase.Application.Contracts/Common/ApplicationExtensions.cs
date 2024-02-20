using AppointEase.Application.Contracts.Interfaces;
using Microsoft.Extensions.Logging;
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

        public ApplicationExtensions(ILogger<ApplicationExtensions> logger)
        {
            _logger = logger;
        }

        public void AddInformationMessage(string message)
        {
            _logger.LogInformation(message);
        }

        public void AddErrorMessage(string message)
        {
            _logger.LogError(message);
        }

    }
}
