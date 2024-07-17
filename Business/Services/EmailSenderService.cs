﻿using Business.Interfaces;
using Business.Models;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;

namespace IdentityApp2.Services
{
    public class EmailSenderService : IEmailSenderService
    {
        private readonly MailSettingsModel _mailSettings;

        public EmailSenderService(IOptions<MailSettingsModel> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(string emailAddress, string subject, string htmlMessage)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail));
            message.To.Add(new MailboxAddress("Recipient", emailAddress));

            message.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = htmlMessage;

            message.Body = bodyBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                // Connect to SMTP server
                await client.ConnectAsync(_mailSettings.SmtpServer, _mailSettings.SmtpPort, _mailSettings.UseSsl).ConfigureAwait(false);

                // Authenticate if SMTP username and password are provided
                if (!string.IsNullOrEmpty(_mailSettings.SmtpUsername) && !string.IsNullOrEmpty(_mailSettings.SmtpPassword))
                {
                    await client.AuthenticateAsync(_mailSettings.SmtpUsername, _mailSettings.SmtpPassword).ConfigureAwait(false);
                }

                // Send message
                await client.SendAsync(message).ConfigureAwait(false);

                // Disconnect from SMTP server
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }

        public async Task SendPasswordResetEmailAsync(string emailAddress, string resetLink)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail));
            message.To.Add(new MailboxAddress("Recipient", emailAddress)); // Adjusted line

            message.Subject = "Password Reset";

            // Customize the email body with the password reset link
            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = $"<p>Dear User,</p><p>Please click the following link to reset your password:</p><p><a href=\"{resetLink}\">Reset Password</a></p>";

            message.Body = bodyBuilder.ToMessageBody();

            // Send the email
            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_mailSettings.SmtpServer, _mailSettings.SmtpPort, _mailSettings.UseSsl).ConfigureAwait(false);

                if (!string.IsNullOrEmpty(_mailSettings.SmtpUsername) && !string.IsNullOrEmpty(_mailSettings.SmtpPassword))
                {
                    await client.AuthenticateAsync(_mailSettings.SmtpUsername, _mailSettings.SmtpPassword).ConfigureAwait(false);
                }

                await client.SendAsync(message).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }
    }
}