using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.EmailConfig;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
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

        public ApplicationExtensions(ILogger<ApplicationExtensions> logger, IEmailServices _emailService, IConfiguration _configuration)
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

            var htmlContent = HtmlContnet(confirmationLink,
                " <p>Thank you for registering with AppointEase.</p>\n<p>Please click the button below to confirm your email address.</p>",
                "Email Confirmation");

            var message = new Messages(new string[] { email }, "Confirmation Email", htmlContent);
            await _emailService.SendEmail(message);

        }

        private string HtmlContnet(string confirmationLink, string Message, string Title)
        {
            var htmlContent = $@"
                    <html>
                        <head>
                            <style>
                                body {{
                                    font-family: 'Arial', sans-serif;
                                    margin: 0;
                                    padding: 0;
                                    box-sizing: border-box;
                                    background-color: #f4f4f4;
                                }}

                                .email-container {{
                                    max-width: 600px;
                                    margin: 0 auto;
                                    background-color: #fff;
                                    padding: 20px;
                                    border-radius: 10px;
                                    box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
                                }}

                                .confirmation-title {{
                                    font-size: 24px;
                                    font-weight: bold;
                                    margin-bottom: 20px;
                                    color: #007bff;
                                    text-decoration: underline;
                                }}

                                .confirmation-message {{
                                    font-size: 16px;
                                    color: #333;
                                    margin-bottom: 20px;
                                }}

                                .confirmation-button {{
                                    display: inline-block;
                                    padding: 10px 20px;
                                    background-color: #007bff;
                                    color: #fff;
                                    text-decoration: none;
                                    border-radius: 5px;
                                }}
                            </style>
                        </head>
                        <body>
                            <div class='email-container'>
                                <div class='confirmation-title'>{Title}</div>
                                <div class='confirmation-message'>
                                    {Message}
                                </div>
                                <a href='{confirmationLink}' class='confirmation-button'>Confirm Email</a>
                            </div>
                        </body>
                    </html>";


            return htmlContent;

        }

        public async Task<string> GenerateJwtTokenAsync(string userId, string username, string userRole, Dictionary<string, string> OtherClaims = null)
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

            var htmlContent = HtmlContnet(Url,
            "<p>Thank you for initiating the password reset process with AppointEase.</p>\n<p>Please click the button below to reset your password.</p>",
            "Password Reset");
            var message = new Messages(new string[] { email }, "Reset yout old Password", htmlContent);
            await _emailService.SendEmail(message);
        }
    }
}
