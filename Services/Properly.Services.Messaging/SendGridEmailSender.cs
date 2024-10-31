namespace Properly.Services.Messaging
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Microsoft.Extensions.Logging;
    using SendGrid;
    using SendGrid.Helpers.Mail;

    public class SendGridEmailSender : IEmailSender
    {
        private readonly ISendGridClient client;
        private readonly ILogger<SendGridEmailSender> logger;

        public SendGridEmailSender(ISendGridClient client, ILogger<SendGridEmailSender> logger)
        {
            this.client = client;
            this.logger = logger;
        }

        public async Task SendEmailAsync(string from, string fromName, string to, string subject, string htmlContent, IEnumerable<EmailAttachment> attachments = null)
        {
            if (string.IsNullOrWhiteSpace(subject) && string.IsNullOrWhiteSpace(htmlContent))
            {
                throw new ArgumentException("Subject and message should be provided.");
            }

            if (string.IsNullOrWhiteSpace(from)) throw new ArgumentException("Sender email is required.", nameof(from));
            if (string.IsNullOrWhiteSpace(to)) throw new ArgumentException("Recipient email is required.", nameof(to));

            var fromAddress = new EmailAddress(from, fromName);
            var toAddress = new EmailAddress(to);
            var message = MailHelper.CreateSingleEmail(fromAddress, toAddress, subject, null, htmlContent);
            if (attachments?.Any() == true)
            {
                foreach (var attachment in attachments)
                {
                    message.AddAttachment(attachment.FileName, Convert.ToBase64String(attachment.Content), attachment.MimeType);
                }
            }

            try
            {
                var response = await this.client.SendEmailAsync(message);
                logger.LogInformation($"Email sent with code : {response.StatusCode}, Response body : {response.Body.ReadAsStringAsync()}");
            }
            catch (Exception e)
            {
                logger.LogError(e, $"An error occurred while sending email to {to}");
                throw;
            }
        }
    }
}
