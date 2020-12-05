using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TreeOfAKind.Application.Configuration.Queries;

namespace TreeOfAKind.Application.Query.Trees.GetMyTrees
{
    public class GetMyTreesQueryHandler : IQueryHandler<GetMyTreesQuery, TreesListDto>
    {
        private readonly ITreeQueryRepository _treesContext;

        public GetMyTreesQueryHandler(ITreeQueryRepository treesContext)
        {
            _treesContext = treesContext;
        }

        public async Task<TreesListDto> Handle(GetMyTreesQuery request, CancellationToken cancellationToken)
        {
            var trees = await _treesContext.GetUsersTreesByAuthId(request.UserAuthId, cancellationToken);

            var items = trees.Select(t => new TreeItemDto {Id = t.Id.Value, TreeName = t.Name, PhotoUri = t.Photo});

            return new TreesListDto(){Trees = items.ToList()};
        }
    }
}
