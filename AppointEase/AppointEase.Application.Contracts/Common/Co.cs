using AppointEase.Application.Contracts.Interfaces;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.Co
{
    public class Common : ICommon
    {
        public static string Role { get; set; }

        private readonly ILogger _logger;

        public Common(ILogger<Common> logger)
        {
            _logger = logger;
        }

        public void LogInformation(string message)
        {
            _logger.LogInformation(message);
        }

        public void LogError(string message)
        {
            _logger.LogError(message);
        }

        // Vendosni funksionet e përbashkëta këtu
    }
}
