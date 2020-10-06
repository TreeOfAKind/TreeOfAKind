using System.Threading;
using System.Threading.Tasks;

namespace TreeOfAKind.Application.Configuration.Authorization
{
    public interface IAuthorizer<in T>
    {
        Task<AuthorizationResult> AuthorizeAsync(T instance, CancellationToken cancellation = default);
    }
}
