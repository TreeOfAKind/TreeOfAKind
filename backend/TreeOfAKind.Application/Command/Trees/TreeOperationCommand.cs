using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Command.Trees
{
    public class TreeOperationCommand : CommandBase
    {
        public string RequesterAuthUserId { get; }
        public TreeId TreeId { get; }

        public TreeOperationCommand(string requesterAuthUserId, TreeId treeId)
        {
            RequesterAuthUserId = requesterAuthUserId;
            TreeId = treeId;
        }
    }
}