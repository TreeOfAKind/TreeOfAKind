using MediatR;
using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Command.Trees
{
    public abstract class TreeOperationCommandBase : TreeOperationCommandBase<Unit>
    {
        protected TreeOperationCommandBase(string requesterUserAuthId, TreeId treeId) : base(requesterUserAuthId, treeId)
        {
        }
    }

    public abstract class TreeOperationCommandBase<TResult> : CommandBase<TResult>
    {
        public string RequesterUserAuthId { get; }
        public TreeId TreeId { get; }

        public TreeOperationCommandBase(string requesterUserAuthId, TreeId treeId)
        {
            RequesterUserAuthId = requesterUserAuthId;
            TreeId = treeId;
        }
    }
}
