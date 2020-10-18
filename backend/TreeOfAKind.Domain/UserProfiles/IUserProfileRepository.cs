using System.Threading.Tasks;

namespace TreeOfAKind.Domain.UserProfiles
{
    public interface IUserProfileRepository
    {
        UserProfile? GetByUsername(string username);
        Task<UserProfile?> GetByIdAsync(UserId id);
        UserProfile? GetByAuthUserId(string authUserId);
        
    }
}