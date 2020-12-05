using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Command.Trees.RemoveTreePhoto
{
    public class RemoveTreePhotoCommand : TreeOperationCommandBase
    {
        public RemoveTreePhotoCommand(string requesterUserAuthId, TreeId treeId) : base(requesterUserAuthId, treeId)
        {
        }
    }
}
