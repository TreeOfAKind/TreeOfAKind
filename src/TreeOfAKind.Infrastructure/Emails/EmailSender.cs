using System;
using System.Threading.Tasks;
using TreeOfAKind.Application.Configuration.Emails;

namespace TreeOfAKind.Infrastructure.Emails
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(EmailMessage message)
        {
            throw new NotImplementedException(nameof(SendEmailAsync));
        }
    }
}