using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace TreeOfAKind.Application.Services
{
    public interface IUserAuthIdProvider
    {
        public Task<string> GetUserAuthId(MailAddress mailAddress, CancellationToken cancellationToken = default!);
    }
}