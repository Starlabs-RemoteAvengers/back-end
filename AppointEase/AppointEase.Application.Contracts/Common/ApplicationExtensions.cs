using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.EmailConfig;
using AppointEase.Application.Contracts.Models.Request;
using MailKit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
        public async Task<string> GenerateJwtTokenAsync(string userId,string username, string userRole,Dictionary<string,string> OtherClaims = null)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, userId),
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, userRole),
            };

            if (OtherClaims != null)
            {
                foreach (var claim in OtherClaims)
                {
                    claims.Add(new Claim(claim.Key, claim.Value));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("eU8o5n@9^2LpWdG!iZtYrCw123456789012345")); 
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "AppointEase",
                audience: "AppointEase",
                claims: claims,
                expires: DateTime.Now.AddHours(3), 
                signingCredentials: creds
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public async Task<string> GenerateJwtTokenAsync(Dictionary<string, string> OtherClaims)
        {
            var claims = new List<Claim>();
          
            if (OtherClaims != null)
            {
                foreach (var claim in OtherClaims)
                {
                    claims.Add(new Claim(claim.Key, claim.Value));
                }
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("eU8o5n@9^2LpWdG!iZtYrCw123456789012345"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "AppointEase",
                audience: "AppointEase",
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds
            );

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }


        public async Task SendEmail(string token, string email, string Url)
        {
            var message = new Messages(new string[] { email }, "Confirmation Email", Url);
            await _emailService.SendEmail(message);
        }
    }
}
