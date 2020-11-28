using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Infrastructure.Database;

namespace TreeOfAKind.Infrastructure.Domain.Trees
{
    public class TreeRepository : ITreeRepository
    {
        private readonly TreesContext _treesContext;

        public TreeRepository(TreesContext treesContext)
        {
            _treesContext = treesContext;
        }

        public async Task<Tree?> GetByIdAsync(TreeId id, CancellationToken cancellationToken = default)
        {
            return await _treesContext.Trees
                .Include(t => t.People)
                .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
        }

        public async Task AddAsync(Tree tree, CancellationToken cancellationToken = default)
        {
            await _treesContext.AddAsync(tree, cancellationToken);
        }
    }
}