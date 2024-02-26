using AppointEase.Application.Contracts.Interfaces;
using AppointEase.Application.Contracts.Models.EmailConfig;
using MailKit.Net.Smtp;
using MimeKit;

namespace AppointEase.Application.Services
{
    public class EmailService : IEmailSerivces
    {
        private readonly EmailConfiguration _emailConfiguration;

        public EmailService(EmailConfiguration emailConfiguration)
        {
            this._emailConfiguration = emailConfiguration;
        }
        public async Task SendEmail(Messages messages)
        {
            var emailMessage = CreateEmailMessage(messages);
            await SendAsync(emailMessage);

        }
        private MimeMessage CreateEmailMessage(Messages messages)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("email",_emailConfiguration.From));
            mimeMessage.To.AddRange(messages.To);
            mimeMessage.Subject = messages.Subject;
            mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = messages.Content };

            return mimeMessage;

        }
        private async Task SendAsync(MimeMessage message)
        {
            using var client = new SmtpClient();

            try
            {
                client.Connect(_emailConfiguration.SmtpServer,_emailConfiguration.Port,true);
                client.AuthenticationMechanisms.Remove("XOAUTH2");
                client.Authenticate(_emailConfiguration.Username, _emailConfiguration.Password);


                client.Send(message);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                client.Disconnect(true);
                client.Dispose();
            }

        }
    }
}
