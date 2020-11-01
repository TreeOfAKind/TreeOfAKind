using System.Threading.Tasks;

namespace TreeOfAKind.Domain.UserProfiles
{
    public interface IUserProfileRepository
    {
        Task<UserProfile?> GetByIdAsync(UserId id);
        UserProfile? GetByAuthUserId(string authUserId);
        
    }
}