using System.Threading;
using System.Threading.Tasks;
using TreeOfAKind.Application.Configuration.Queries;
using TreeOfAKind.Application.Query.Trees.GetMyTrees;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Query.Trees.GetTree
{
    public class GetTreeQueryHandler : IQueryHandler<GetTreeQuery, TreeDto>
    {
        private readonly ITreeQueryRepository _treeQueryRepository;

        public GetTreeQueryHandler(ITreeQueryRepository treeQueryRepository)
        {
            _treeQueryRepository = treeQueryRepository;
        }

        public async Task<TreeDto> Handle(GetTreeQuery request, CancellationToken cancellationToken)
        {
            return await _treeQueryRepository.GetTree(request.TreeId, cancellationToken);
        }
    }
}
