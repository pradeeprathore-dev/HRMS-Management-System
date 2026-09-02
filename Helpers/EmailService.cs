using Helpers;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace HRMSAPI.Helpers
{
    public class EmailService : IEmailService
    {
        private readonly MailSettings _mailSettings;

        public EmailService(
            IOptions<MailSettings> mailSettings)
        {
            _mailSettings =
                mailSettings.Value;
        }

        public async Task SendEmailAsync(
            string toEmail,
            string subject,
            string body)
        {
            var email =
                new MimeMessage();

            email.From.Add(
                MailboxAddress.Parse(
                    _mailSettings.Mail));

            email.To.Add(
                MailboxAddress.Parse(
                    toEmail));

            email.Subject = subject;

            email.Body =
                new TextPart(
                    MimeKit.Text.TextFormat.Html)
                {
                    Text = body
                };

            using var smtp =
                new SmtpClient();

            await smtp.ConnectAsync(
                _mailSettings.Host,
                _mailSettings.Port,
                SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                _mailSettings.Username,
                _mailSettings.Password);

            await smtp.SendAsync(email);

            await smtp.DisconnectAsync(true);
        }
    }
}