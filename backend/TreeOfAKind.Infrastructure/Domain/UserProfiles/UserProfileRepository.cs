using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TreeOfAKind.Domain.UserProfiles;
using TreeOfAKind.Infrastructure.Database;

namespace TreeOfAKind.Infrastructure.Domain.UserProfiles
{
    public class UserProfileRepository : IUserProfileRepository
    {
        private readonly TreesContext _dbContext;

        public UserProfileRepository(TreesContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<UserProfile?> GetByIdAsync(UserId id)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(user => user.Id == id);
        }

        public UserProfile? GetByAuthUserId(string authUserId)
        {
            return _dbContext.Users.FirstOrDefault(user => user.AuthUserId == authUserId);
        }
    }
}