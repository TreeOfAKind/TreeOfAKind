using MediatR;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Query.Trees.GetTreeStatistics
{
    public class GetTreeStatisticsQuery : TreeQueryBase<TreeStatisticsDto>
    {
        public GetTreeStatisticsQuery(string requesterUserAuthId, TreeId treeId) : base(requesterUserAuthId, treeId)
        {
        }
    }
}
