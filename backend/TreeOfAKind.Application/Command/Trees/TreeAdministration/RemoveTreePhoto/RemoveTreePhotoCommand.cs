using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.RemoveTreePhoto
{
    public class RemoveTreePhotoCommand : TreeOperationCommandBase
    {
        public RemoveTreePhotoCommand(string requesterUserAuthId, TreeId treeId) : base(requesterUserAuthId, treeId)
        {
        }
    }
}
