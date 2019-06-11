using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace GlodnyStudent.Services
{
    public class EmailSender : IEmailSender
    {
        public EmailSender(IOptions<AuthMessageSenderOptions> optionsAccessor)
        {
            Options = optionsAccessor.Value;
        }

        public AuthMessageSenderOptions Options { get; } //set only via Secret Manager
        /// <summary>
        /// Metoda służąca do wysyłania maili np o zmiane zapomnianego hasła do użytkowników
        /// </summary>
        /// <param name="email">Email odbiorcy</param>
        /// <param name="subject">Temat wiadomości</param>
        /// <param name="message">Treść wiadomości</param>
        /// <returns></returns>
        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Execute(Options.SendGridKey, subject, message, email);
        }

        /// <summary>
        /// Funkcja zajmująca sie wykonywaniem zadania jakim jest wysłanie maila
        /// </summary>
        /// <param name="apiKey">Klucz api z aplikacji SendGrid</param>
        /// <param name="subject">Temat wiadomości</param>
        /// <param name="message">Treść wiadomości</param>
        /// <param name="email">Email odbiorcy</param>
        /// <returns></returns>
        public Task Execute(string apiKey, string subject, string message, string email)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("support@glodnystudent.pl", "Głodny Student"),
                Subject = subject,
                PlainTextContent = message,
                HtmlContent = message
            };
            msg.AddTo(new EmailAddress(email));

            // Disable click tracking.
            // See https://sendgrid.com/docs/User_Guide/Settings/tracking.html
            msg.SetClickTracking(false, false);

            return client.SendEmailAsync(msg);
        }
    }
}
