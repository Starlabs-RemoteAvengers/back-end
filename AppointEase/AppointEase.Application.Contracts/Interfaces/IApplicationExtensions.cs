using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppointEase.Application.Contracts.Interfaces
{
    public interface IApplicationExtensions
    {
        void AddInformationMessage(string message);
        void AddErrorMessage(string message);
        Task SendEmailConfirmation(string token, string email);
        Task SendEmail(string token, string email,string Url);
        Task<string> GenerateJwtTokenAsync(string userId, string username, string userRole, Dictionary<string, string> OtherClaims = null);
        Task<string> GenerateJwtTokenAsync(Dictionary<string, string> OtherClaims = null);
    }
}
