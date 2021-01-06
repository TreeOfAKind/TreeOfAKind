using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TreeOfAKind.Domain.Trees;
using TreeOfAKind.Domain.Trees.People;

namespace TreeOfAKind.Application.Query.Trees
{
    public interface ITreeQueryRepository
    {
        Task<List<Tree>> GetUsersTreesByAuthId(string userAuthId, CancellationToken cancellationToken = default!);
        Task<TreeDto> GetTree(TreeId treeId, CancellationToken cancellationToken = default!);
    }
}
