using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.RemoveMyselfFromTreeOwners
{
    public class RemoveMyselfFromTreeOwnersCommand : TreeOperationCommandBase
    {
        public RemoveMyselfFromTreeOwnersCommand(string requesterUserAuthId, TreeId treeId) : base(requesterUserAuthId, treeId)
        {
        }
    }
}
