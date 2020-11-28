using System.Net.Mail;

namespace TreeOfAKind.Application.Services
{
    public interface IUserAuthIdProvider
    {
        public string GetUserAuthId(MailAddress mailAddress);
    }
}