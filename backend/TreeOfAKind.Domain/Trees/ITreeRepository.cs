using System.Threading;
using System.Threading.Tasks;

namespace TreeOfAKind.Domain.Trees
{
    public interface ITreeRepository
    {
        Task<Tree?> GetByIdAsync(TreeId id, CancellationToken cancellationToken = default!);
        Task RemoveAsync(Tree id, CancellationToken cancellationToken = default!);
        Task AddAsync(Tree tree, CancellationToken cancellationToken = default!);
    }
}
