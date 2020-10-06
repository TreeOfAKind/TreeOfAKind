using System.Threading.Tasks;

namespace TreeOfAKind.Application.Configuration.Emails
{
    public interface IEmailSender
    {
        Task SendEmailAsync(EmailMessage message);
    }
}