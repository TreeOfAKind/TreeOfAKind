using System;
using System.IO;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using Serilog;
using TreeOfAKind.Application.Configuration.Emails;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Infrastructure.Emails
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailsSettings _emailsSettings;
        private readonly ILogger _logger;

        public EmailSender(EmailsSettings emailsSettings, ILogger logger)
        {
            _emailsSettings = emailsSettings;
            _logger = logger;
        }

        public async Task SendEmailAsync(EmailMessage message)
        {
            var apiKey = _emailsSettings.SendgridApiKey;
            var client = new SendGridClient(apiKey);

            var from = new EmailAddress(_emailsSettings.FromAddressEmail, "Tree Of A Kind");
            var subject = "Tree Of A Kind - tree invitation";
            var plainTextContent = "You were invited to tree";
            var to = new EmailAddress(message.To);

            var file = await System.IO.File.ReadAllTextAsync(
                Path.Combine(Environment.CurrentDirectory,
                $@".\email-template.html"));
            file = file.Replace("{0}", message.SenderName);
            file = file.Replace("{1}", message.TreeName);

            var htmlContent = file;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg);

            if (!response.IsSuccessStatusCode)
            {
                var body = await response.Body.ReadAsStringAsync();
                _logger.Error("Message {@message} was not sent with status code {@status} and body {@body}", msg,
                    response.StatusCode, body);
            }
        }
    }
}
