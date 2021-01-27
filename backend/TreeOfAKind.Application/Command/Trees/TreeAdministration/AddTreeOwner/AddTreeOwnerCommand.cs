using TreeOfAKind.Domain.Trees;

namespace TreeOfAKind.Application.Command.Trees.TreeAdministration.AddTreeOwner
{
    public class AddTreeOwnerCommand : TreeOperationCommandBase
    {
        public string AddedPersonMailAddress { get; }

        public AddTreeOwnerCommand(string requesterUserAuthId, TreeId treeId, string addedPersonMailAddress)
            : base(requesterUserAuthId, treeId)
        {
            AddedPersonMailAddress = addedPersonMailAddress;
        }
    }
}
