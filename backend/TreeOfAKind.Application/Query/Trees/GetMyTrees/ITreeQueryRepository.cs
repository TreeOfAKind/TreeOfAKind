using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Query.Trees.GetMyTrees
{
    public interface ITreeQueryRepository
    {
        Task<List<Tree>> GetUsersTreesByAuthId(string userAuthId, CancellationToken cancellationToken = default!);
    }
}