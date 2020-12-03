using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using TreeOfAKind.Application.Services;

namespace TreeOfAKind.Infrastructure.Authentication
{
    public class UserAuthIdProvider : IUserAuthIdProvider
    {
        public async Task<string> GetUserAuthId(MailAddress mailAddress, CancellationToken cancellationToken = default)
        {
            var abstractFirebaseAuth =
                await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(mailAddress.Address, cancellationToken);

            return abstractFirebaseAuth.Uid;
        }
    }
}