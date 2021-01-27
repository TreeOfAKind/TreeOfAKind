using TreeOfAKind.Application.Configuration.Queries;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Query.Trees
{
    public abstract class TreeQueryBase<TResult> : IQuery<TResult>
    {
        public string RequesterUserAuthId { get; }
        public TreeId TreeId { get; }

        protected TreeQueryBase(string requesterUserAuthId, TreeId treeId)
        {
            RequesterUserAuthId = requesterUserAuthId;
            TreeId = treeId;
        }
    }
}
