using System.Threading;
using System.Threading.Tasks;

namespace TreeOfAKind.Domain.UserProfiles
{
    public interface IUserProfileRepository
    {
        Task<UserProfile?> GetByIdAsync(UserId id, CancellationToken cancellationToken = default!);
        UserProfile? GetByUserAuthId(string userAuthId);
        Task<UserProfile?> GetByUserAuthIdAsync(string userAuthId, CancellationToken cancellationToken);

        Task AddAsync(UserProfile userProfile, CancellationToken cancellationToken = default!);

    }
}
