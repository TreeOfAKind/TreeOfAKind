using System.Threading;
using System.Threading.Tasks;
using TreeOfAKind.Application.Configuration.Queries;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Query.Trees.GetTree
{
    public class GetTreeQueryHandler : IQueryHandler<GetTreeQuery, TreeDto>
    {
        private readonly ITreeRepository _treeRepository;

        public GetTreeQueryHandler(ITreeRepository treeRepository)
        {
            _treeRepository = treeRepository;
        }

        public async Task<TreeDto> Handle(GetTreeQuery request, CancellationToken cancellationToken)
        {
            var tree = await _treeRepository.GetByIdAsync(request.TreeId, cancellationToken);

            return tree is null ? null : new TreeDto(tree);
        }
    }
}
