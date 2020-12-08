using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TreeOfAKind.Application.Query.Trees.GetMyTrees;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.UserProfiles;
using TreeOfAKind.Infrastructure.Database;

namespace TreeOfAKind.Infrastructure.Domain.Trees
{
    public class TreeQueryRepository : ITreeQueryRepository
    {
        private readonly TreesContext _treesContext;
        private readonly IUserProfileRepository _userProfileRepository;

        public TreeQueryRepository(TreesContext treesContext, IUserProfileRepository userProfileRepository)
        {
            _treesContext = treesContext;
            _userProfileRepository = userProfileRepository;
        }

        public async Task<List<Tree>> GetUsersTreesByAuthId(string userAuthId,
            CancellationToken cancellationToken = default!)
        {
            var user = await _userProfileRepository.GetByUserAuthIdAsync(userAuthId, cancellationToken);

            return await _treesContext.Trees
                .Where(t => t.TreeOwners.Any(o => o.UserId == user.Id))
                .OrderBy(t => t.Name)
                .ToListAsync(cancellationToken);
        }
    }
}