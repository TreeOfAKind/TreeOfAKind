using System.Threading;
using System.Threading.Tasks;
using TreeOfAKind.Application.Configuration.Authorisation;

namespace TreeOfAKind.Application.Ping
{
    public class PingCommandAuthorizer : IAuthorizer<PingCommand>
    {
        public async Task<AuthorizationResult> AuthorizeAsync(PingCommand instance, CancellationToken cancellation = default)
        {
            return instance.PingName == "T" ?
                await Task.FromResult(AuthorizationResult.Succeed()) 
                :
                await Task.FromResult(AuthorizationResult.Fail(":("));
        }
    }
}

