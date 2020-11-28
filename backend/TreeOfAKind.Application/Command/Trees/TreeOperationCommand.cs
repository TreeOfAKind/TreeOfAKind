using TreeOfAKind.Application.Configuration.Commands;
using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Command.Trees
{
    public class TreeOperationCommand : CommandBase
    {
        public string RequesterUserAuthId { get; }
        public TreeId TreeId { get; }

        public TreeOperationCommand(string requesterUserAuthId, TreeId treeId)
        {
            RequesterUserAuthId = requesterUserAuthId;
            TreeId = treeId;
        }
    }
}