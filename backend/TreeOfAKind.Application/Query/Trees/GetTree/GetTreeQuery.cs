using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Query.Trees.GetTree
{
    public class GetTreeQuery : TreeQueryBase<TreeDto>
    {
        public GetTreeQuery(string requesterUserAuthId, TreeId treeId) : base(requesterUserAuthId, treeId)
        {
        }
    }
}
