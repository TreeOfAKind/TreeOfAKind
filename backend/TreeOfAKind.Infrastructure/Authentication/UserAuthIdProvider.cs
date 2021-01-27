using System;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;
using FirebaseAdmin.Auth;
using Serilog;
using TreeOfAKind.Application.Services;

namespace TreeOfAKind.Infrastructure.Authentication
{
    public class UserAuthIdProvider : IUserAuthIdProvider
    {
        private readonly ILogger _logger;

        public UserAuthIdProvider(ILogger logger)
        {
            _logger = logger;
        }

        public async Task<string> GetUserAuthId(MailAddress mailAddress, CancellationToken cancellationToken = default)
        {
            string uid = null;
            try
            {
                 uid =
                    (await FirebaseAuth.DefaultInstance.GetUserByEmailAsync(mailAddress.Address, cancellationToken))?.Uid;
            }
            catch (FirebaseAuthException)
            {
                _logger.Warning("User {@mail} not found in Firebase Auth database.", mailAddress);
            }

            return uid;
        }
    }
}
