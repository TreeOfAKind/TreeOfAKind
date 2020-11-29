using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TreeOfAKind.Application.Query.Trees.GetMyTrees;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Infrastructure.Database;

namespace TreeOfAKind.Infrastructure.Domain.Trees
{
    public class TreeQueryRepository : ITreeQueryRepository
    {
        private readonly TreesContext _treesContext;

        public TreeQueryRepository(TreesContext treesContext)
        {
            _treesContext = treesContext;
        }

        public async Task<List<Tree>> GetUsersTreesByAuthId(string userAuthId,
            CancellationToken cancellationToken = default!)
        {
            var a = await _treesContext.Users
                .Include("_ownedTrees")
                .FirstOrDefaultAsync(u => u.UserAuthId == userAuthId, cancellationToken);

            return a.OwnedTrees.ToList();
        }
    }
}