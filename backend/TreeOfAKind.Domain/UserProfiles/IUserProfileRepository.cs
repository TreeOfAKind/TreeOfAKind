using System.Threading;
using System.Threading.Tasks;

namespace TreeOfAKind.Domain.UserProfiles
{
    public interface IUserProfileRepository
    {
        Task<UserProfile?> GetByIdAsync(UserId id);
        UserProfile? GetByAuthUserId(string authUserId);
        Task<UserProfile?> GetByAuthUserIdAsync(string authUserId);

        Task AddAsync(UserProfile userProfile, CancellationToken cancellationToken = default!);

    }
}