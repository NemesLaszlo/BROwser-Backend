using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Email
{
    /// <summary>
    /// Automatic email sender logic for email verification after the registration process and for password reser requests
    /// </summary>
    public class EmailSender
    {
        private readonly IConfiguration _config;

        public EmailSender(IConfiguration config)
        {
            _config = config;
        }

        /// <summary>
        /// Configure and send the email
        /// </summary>
        /// <param name="userEmail">Email address to send mail here</param>
        /// <param name="emailSubject">Subject / "Title" of the email</param>
        /// <param name="msg">The message body of the email</param>
        /// <returns></returns>
        public async Task SendEmailAsync(string userEmail, string emailSubject, string msg)
        {
            var client = new SendGridClient(_config["SendGrid:Key"]);
            var message = new SendGridMessage
            {
                From = new EmailAddress(_config["Sendgrid:Email"], _config["Sendgrid:User"]),
                Subject = emailSubject,
                PlainTextContent = msg,
                HtmlContent = msg
            };

            message.AddTo(new EmailAddress(userEmail));
            message.SetClickTracking(false, false);

            await client.SendEmailAsync(message);
        }
    }
}
